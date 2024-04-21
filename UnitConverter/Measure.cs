using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UnitConverter
{
    public readonly struct Measure
    {
        private readonly double InputValue;
        public readonly Dictionary<Unit, double> InputUnits;
        public readonly double DisplayValue;
        public readonly Dictionary<Unit, double> DisplayUnits;
        public readonly double BaseValue;
        public readonly Dictionary<Unit, double> BaseUnits;
        public readonly Dictionary<Unit, double> ExtendUnits;

        public override string ToString()
        {
            return $"{DisplayValue} {DisplayUnitsToString()}";
        }

        public string ToBaseString()
        {
            return $"{BaseValue} {BaseUnitsToString()}";
        }

        private string DisplayUnitsToString()
        {
            var Positive = DisplayUnits.Where(x => x.Value > 0).OrderBy(x => x.Key.Id).Select(x => UnitToString(x)).ToList();
            var Negative = DisplayUnits.Where(x => x.Value < 0).OrderBy(x => x.Key.Id).Select(x => UnitToString(x)).ToList();
            string positive = string.Join("*", Positive);
            string negative = string.Join("*", Negative);
            if (Negative.Count == 0)
                return positive;
            if (Positive.Count == 0)
                return $"1/({negative})";
            return $"{positive}/({negative})";
        }

        private static string UnitToString(KeyValuePair<Unit, double> unit)
        {
            if (unit.Value == 1)
                return unit.Key.Code;
            if (unit.Value == 0)
                return "";
            return $"{unit.Key.Code}^{Math.Abs(unit.Value)}";
        }


        private string BaseUnitsToString()
        {
            string[] units = BaseUnits.Select(x => $"{x.Key.Code}^{x.Value}").ToArray();
            return string.Join(".", units);
        }


        public Measure(double value, string displayUnits)
        {
            // Chuyển đổi chuỗi string đơn vị thành object
            InputValue = value;
            InputUnits = new Dictionary<Unit, double>();
            List<string> str_units = displayUnits.Split(".").ToList();
            foreach (string str in str_units)
            {
                string[] s = str.Split("^");
                if (s.Length > 0)
                {
                    Unit inputUnit = Unit.ListEnum.Where(x => x.Code == s[0]).FirstOrDefault();
                    if (s.Length > 1)
                    {
                        int count = int.Parse(s[1]);
                        InputUnits.Add(inputUnit, count);
                    }
                    else
                        InputUnits.Add(inputUnit, 1);
                }
            }

            // Đưa về dạng chuẩn của các loại đơn vị
            DisplayUnits = new Dictionary<Unit, double>();
            double displayValue = value;
            foreach (var InputUnit in InputUnits)
            {
                Unit displayUnit = Unit.ListEnum.Where(x => x.UnitTypeId == InputUnit.Key.UnitTypeId && x.Factor == 1).FirstOrDefault();
                displayValue = displayValue * Math.Round(Math.Pow(InputUnit.Key.Factor, InputUnit.Value), 10);
                DisplayUnits.Add(displayUnit, InputUnit.Value);
            }
            DisplayValue = displayValue;

            // Đưa về dạng chuẩn chỉ bao gồm Length, Mass, Time để hỗ trợ tính toán
            BaseUnits = new Dictionary<Unit, double>();
            double baseValue = displayValue;
            foreach (var displayUnit in DisplayUnits)
            {
                if (displayUnit.Key.UnitTypeId == UnitType.FORCE.Id)
                {
                    foreach (var displayBaseUnit in displayUnit.Key.BaseUnits)
                    {
                        Unit baseUnit = Unit.ListEnum.Where(x => x.UnitTypeId == displayBaseUnit.Key.UnitTypeId && x.Factor == 1).FirstOrDefault();
                        BuildUnit(BaseUnits, baseUnit, displayBaseUnit.Value);
                    }
                    baseValue = baseValue * displayUnit.Key.BaseFactor;
                }
                else
                {
                    Unit baseUnit = Unit.ListEnum.Where(x => x.UnitTypeId == displayUnit.Key.UnitTypeId && x.Factor == 1).FirstOrDefault();
                    BuildUnit(BaseUnits, baseUnit, displayUnit.Value);
                }
            }
            BaseValue = baseValue;
        }

       
        public Measure(double value, Dictionary<Unit, double> units)
        {
            InputValue = value;
            InputUnits = units;

            DisplayUnits = new Dictionary<Unit, double>();
            double displayValue = value;
            foreach (var inputUnit in InputUnits)
            {
                Unit baseUnit = Unit.ListEnum.Where(x => x.UnitTypeId == inputUnit.Key.UnitTypeId && x.Factor == 1).FirstOrDefault();
                displayValue = displayValue * Math.Pow(inputUnit.Key.Factor, inputUnit.Value);
                DisplayUnits.Add(baseUnit, inputUnit.Value);
            }
            DisplayValue = displayValue;

            // Đưa về dạng chuẩn chỉ bao gồm Length, Mass, Time để hỗ trợ tính toán
            BaseUnits = new Dictionary<Unit, double>();
            double baseValue = displayValue;
            foreach (var displayUnit in DisplayUnits)
            {
                if (displayUnit.Key.UnitTypeId == UnitType.FORCE.Id)
                {
                    foreach (var displayBaseUnit in displayUnit.Key.BaseUnits)
                    {
                        Unit baseUnit = Unit.ListEnum.Where(x => x.UnitTypeId == displayBaseUnit.Key.UnitTypeId && x.Factor == 1).FirstOrDefault();
                        BuildUnit(BaseUnits, baseUnit, displayBaseUnit.Value);
                    }
                    baseValue = baseValue * displayUnit.Key.BaseFactor;
                }
                else
                {
                    Unit baseUnit = Unit.ListEnum.Where(x => x.UnitTypeId == displayUnit.Key.UnitTypeId && x.Factor == 1).FirstOrDefault();
                    BuildUnit(BaseUnits, baseUnit, displayUnit.Value);
                }
            }
            BaseValue = baseValue;
        }

        private void BuildUnit(Dictionary<Unit, double> units, Unit unit, double count)
        {
            if (units.ContainsKey(unit))
                units[unit] += count;
            else
                units.Add(unit, count);
        }

        public static Measure operator +(Measure a, Measure b)
        {
            if (a.BaseUnitsToString() == b.BaseUnitsToString())
            {
                double value = a.DisplayValue + b.DisplayValue;
                Dictionary<Unit, double> units = a.DisplayUnits;
                Measure Result = new Measure(value, units);

                return Result;
            }
            else
            {
                throw new ArithmeticException();
            }
        }

        public static Measure operator -(Measure a, Measure b)
        {
            if (a.BaseUnitsToString() == b.BaseUnitsToString())
            {
                double value = a.DisplayValue - b.DisplayValue;
                Dictionary<Unit, double> units = a.DisplayUnits;
                Measure Result = new Measure(value, units);

                return Result;
            }
            else
            {
                throw new ArithmeticException();
            }
        }

        public static Measure operator *(Measure a, Measure b)
        {
            double value = a.DisplayValue * b.DisplayValue;
            Dictionary<Unit, double> units = new Dictionary<Unit, double>();
            foreach (var unit in a.DisplayUnits)
            {
                if (units.ContainsKey(unit.Key))
                    units[unit.Key] += unit.Value;
                else
                    units.Add(unit.Key, unit.Value);
            }
            foreach (var unit in b.DisplayUnits)
            {
                if (units.ContainsKey(unit.Key))
                    units[unit.Key] += unit.Value;
                else
                    units.Add(unit.Key, unit.Value);
            }
            Measure Result = new Measure(value, units);

            return Result;

        }

        public static Measure operator *(Measure a, double b)
        {
            double value = a.DisplayValue * b;
            Measure Result = new Measure(value, a.DisplayUnits);
            return Result;

        }
        public static Measure operator *(double a, Measure b)
        {
            double value = a * b.DisplayValue;
            Measure Result = new Measure(value, b.DisplayUnits);
            return Result;

        }
        public static Measure operator /(Measure a, Measure b)
        {
            double value = a.DisplayValue / b.DisplayValue;
            Dictionary<Unit, double> units = new Dictionary<Unit, double>();
            foreach (var unit in a.DisplayUnits)
            {
                if (units.ContainsKey(unit.Key))
                    units[unit.Key] += unit.Value;
                else
                    units.Add(unit.Key, unit.Value);
            }
            foreach (var unit in b.DisplayUnits)
            {
                if (units.ContainsKey(unit.Key))
                    units[unit.Key] -= unit.Value;
                else
                    units.Add(unit.Key, 0 - unit.Value);
            }
            Measure Result = new Measure(value, units);

            return Result;

        }

        public static Measure operator /(Measure a, double b)
        {
            double value = a.DisplayValue / b;
            Dictionary<Unit, long> units = new Dictionary<Unit, long>();
            Measure Result = new Measure(value, a.DisplayUnits);

            return Result;

        }
    }
}
