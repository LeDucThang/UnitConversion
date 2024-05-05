using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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
            var ExtendPositive = ExtendUnits.Where(x => x.Value > 0).OrderBy(x => x.Key.Id).Select(x => UnitToString(x)).ToList();
            var ExtendNegative = ExtendUnits.Where(x => x.Value < 0).OrderBy(x => x.Key.Id).Select(x => UnitToString(x)).ToList();
            string extendPositive = string.Join("*", ExtendPositive);
            string extendNegative = string.Join("*", ExtendNegative);
            var DisplayPositive = DisplayUnits.Where(x => x.Value > 0).OrderBy(x => x.Key.Id).Select(x => UnitToString(x)).ToList();
            var DisplayNegative = DisplayUnits.Where(x => x.Value < 0).OrderBy(x => x.Key.Id).Select(x => UnitToString(x)).ToList();
            string displayPositive = string.Join("*", DisplayPositive);
            string displayNegative = string.Join("*", DisplayNegative);

            string extend = string.Empty;
            if (ExtendPositive.Count == 0 && ExtendNegative.Count == 0)
            {

            }
            else
            {
                if (ExtendNegative.Count == 0)
                    extend = extendPositive;
                else if (ExtendPositive.Count == 0)
                    extend = $"1/({extendNegative})";
                else
                    extend = $"{extendPositive}/({extendNegative})";
            }

            string display = string.Empty;
            if (DisplayNegative.Count == 0)
                display = displayPositive;
            else if (DisplayPositive.Count == 0)
                display = $"1/({displayNegative})";
            else
                display = $"{displayPositive}/({displayNegative})";

            if (extend == string.Empty)
                return display;
            else if (display == string.Empty)
                return extend;
            else
                return $"{extend}.{display}";
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
            string[] units = BaseUnits.Where(x => x.Value != 0).Select(x => $"{x.Key.Code}^{x.Value}").ToArray();
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
            BuildDisplayUnits(InputUnits, DisplayUnits, ref displayValue);
            DisplayValue = displayValue;

            // Đưa về dạng chuẩn chỉ bao gồm Length, Mass, Time để hỗ trợ tính toán
            BaseUnits = new Dictionary<Unit, double>();
            double baseValue = displayValue;
            BuildBaseUnits(DisplayUnits, BaseUnits, ref baseValue);
            BaseValue = baseValue;
            ExtendUnits = new Dictionary<Unit, double>();
        }

        public Measure(double value, Dictionary<Unit, double> units)
        {
            InputValue = value;
            InputUnits = units;


            DisplayUnits = new Dictionary<Unit, double>();
            double displayValue = value;
            BuildDisplayUnits(InputUnits, DisplayUnits, ref displayValue);
            DisplayValue = displayValue;

            // Đưa về dạng chuẩn chỉ bao gồm Length, Mass, Time để hỗ trợ tính toán
            BaseUnits = new Dictionary<Unit, double>();
            double baseValue = displayValue;
            BuildBaseUnits(DisplayUnits, BaseUnits, ref baseValue);
            BaseValue = baseValue;

            ExtendUnits = new Dictionary<Unit, double>();
        }

        private void BuildDisplayUnits(Dictionary<Unit, double> InputUnits, Dictionary<Unit, double> DisplayUnits, ref double displayValue)
        {
            foreach (var inputUnit in InputUnits)
            {
                Unit baseUnit = Unit.ListEnum.Where(x => x.UnitTypeId == inputUnit.Key.UnitTypeId && x.Factor == 1).FirstOrDefault();
                displayValue = displayValue * Math.Pow(inputUnit.Key.Factor, inputUnit.Value);
                DisplayUnits.Add(baseUnit, inputUnit.Value);
            }
        }

        private void BuildBaseUnits(Dictionary<Unit, double> DisplayUnits, Dictionary<Unit, double> BaseUnits, ref double baseValue)
        {
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
        }

        public Measure(Measure measure, string extUnit) : this(measure.BaseValue, measure.BaseUnits, extUnit)
        {

        }

        public Measure(double value, Dictionary<Unit, double> units, string extUnit)
        {
            InputValue = value;
            InputUnits = units;

            // Đưa về dạng chuẩn chỉ bao gồm Length, Mass, Time để hỗ trợ tính toán
            BaseUnits = new Dictionary<Unit, double>();
            double baseValue = value;
            BuildBaseUnits(InputUnits, BaseUnits, ref baseValue);
            BaseValue = baseValue;

            // chuyển đổi từ chuỗi ext thành Dictionary.
            ExtendUnits = new Dictionary<Unit, double>();
            List<string> str_units = extUnit.Split(".").ToList();
            foreach (string str in str_units)
            {
                string[] s = str.Split("^");
                if (s.Length > 0)
                {
                    Unit extendUnit = Unit.ListEnum.Where(x => x.Code == s[0]).FirstOrDefault();
                    if (s.Length > 1)
                    {
                        int count = int.Parse(s[1]);
                        ExtendUnits.Add(extendUnit, count);
                    }
                    else
                        ExtendUnits.Add(extendUnit, 1);
                }
            }

            //// Đưa về dạng chuẩn của các loại đơn vị
            //DisplayUnits = new Dictionary<Unit, double>();
            //double displayValue = value;
            //BuildDisplayUnits(InputUnits, BaseUnits, ref displayValue);
            //DisplayValue = displayValue;



            // Khởi tạo lại DisplayUnit để tạo lại DisplayUnit mới dựa trên BaseUnits và ExtendUnits
            double displayValue = value;
            DisplayUnits = new Dictionary<Unit, double>();
            // Khởi tạo standardUnits từ ExtendUnits. Chuyển đổi về 3 loại Length,Mass,Time cơ bản
            Dictionary<Unit, double> standardUnits = new Dictionary<Unit, double>();
            double standardDisplayValue = value;
            foreach (var extendUnit in ExtendUnits)
            {
                standardDisplayValue = standardDisplayValue / extendUnit.Key.Factor;
                Unit extendBaseUnit = Unit.ListEnum.Where(x => x.UnitTypeId == extendUnit.Key.UnitTypeId && x.Factor == 1).FirstOrDefault();
                standardDisplayValue = standardDisplayValue / extendBaseUnit.BaseFactor;
                if (extendBaseUnit.BaseUnits != null)
                {
                    foreach(var baseUnit in  extendBaseUnit.BaseUnits)
                    {
                        standardUnits.Add(baseUnit.Key, baseUnit.Value);
                    }
                }    
            }

            // Xác định DisplayUnit mới và tính lại DisplayValue
            displayValue = standardDisplayValue;
            foreach (var baseUnit in BaseUnits)
            {
                DisplayUnits.Add(baseUnit.Key, baseUnit.Value);
                if (standardUnits.ContainsKey(baseUnit.Key))
                {
                    DisplayUnits[baseUnit.Key] = DisplayUnits[baseUnit.Key] - standardUnits[baseUnit.Key];
                }
            }
            DisplayValue = displayValue;
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

        public static Measure operator ^(Measure a, double b)
        {
            double value = Math.Pow(a.BaseValue, b);
            Dictionary<Unit, double> units = new Dictionary<Unit, double>();
            foreach (var unit in a.BaseUnits)
            {
                if (units.ContainsKey(unit.Key))
                    units[unit.Key] += unit.Value * b;
                else
                    units.Add(unit.Key, unit.Value * b);
            }
            Measure result = new Measure(value, units);
            return result;
        }
    }
}
