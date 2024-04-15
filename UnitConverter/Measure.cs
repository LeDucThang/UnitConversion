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
        public readonly long InputTenPow;
        public readonly Dictionary<Unit, long> InputUnits;
        public readonly double DisplayValue;
        public readonly double DisplayTenPow;
        public readonly Dictionary<Unit, long> DisplayUnits;
        public readonly double BaseValue;
        public readonly double BaseTenPow;
        public readonly Dictionary<Unit, long> BaseUnits;

        public string ToString()
        {
            return $"{BaseValue} {UnitsToString()}";
        }

        private string UnitsToString()
        {
            var Positive = BaseUnits.Where(x => x.Value > 0).OrderBy(x => x.Key.Id).Select(x => UnitToString(x)).ToList();
            var Negative = BaseUnits.Where(x => x.Value < 0).OrderBy(x => x.Key.Id).Select(x => UnitToString(x)).ToList();
            string positive = string.Join("*", Positive);
            string negative = string.Join("*", Negative);
            if (Negative.Count == 0)
                return positive;
            if (Positive.Count == 0)
                return $"1/{negative}";
            return $"{positive}/{negative}";
        }

        private static string UnitToString(KeyValuePair<Unit, long> unit)
        {
            if (unit.Value > 1)
                return $"{unit.Key.Code}^{unit.Value}";
            if (unit.Value == 1)
                return unit.Key.Code;
            if (unit.Value == 0)
                return "";
            if (unit.Value == -1)
                return unit.Key.Code;
            if (unit.Value < -1)
                return $"{unit.Key.Code}^{0 - unit.Value}";
            return "";

        }

        public Measure(double value, string unit)
        {
            InputValue = value;
            InputUnits = new Dictionary<Unit, long>();
            Unit inputUnit = Unit.ListEnum.Where(x => x.Code == unit).FirstOrDefault();
            InputUnits.Add(inputUnit, 1);

            Unit baseUnit = Unit.ListEnum.Where(x => x.UnitTypeId == inputUnit.UnitTypeId && x.Factor == 1).FirstOrDefault();
            BaseValue = value * inputUnit.Factor;
            BaseUnits = new Dictionary<Unit, long>();
            BaseUnits.Add(baseUnit, 1);
        }
        public Measure(double value, Dictionary<Unit, long> units)
        {
            InputValue = value;
            InputUnits = units;

            BaseUnits = new Dictionary<Unit, long>();
            foreach (var inputUnit in InputUnits)
            {
                Unit baseUnit = Unit.ListEnum.Where(x => x.UnitTypeId == inputUnit.Key.UnitTypeId && x.Factor == 1).FirstOrDefault();
                value = value * Math.Pow(inputUnit.Key.Factor, inputUnit.Value);
                BaseUnits.Add(baseUnit, inputUnit.Value);
            }
            BaseValue = value;
        }

        public static Measure operator +(Measure a, Measure b)
        {
            if (a.UnitsToString() == b.UnitsToString())
            {
                double value = a.BaseValue + b.BaseValue;
                Dictionary<Unit, long> units = a.BaseUnits;
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
            if (a.UnitsToString() == b.UnitsToString())
            {
                double value = a.BaseValue - b.BaseValue;
                Dictionary<Unit, long> units = a.BaseUnits;
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
            double value = a.BaseValue * b.BaseValue;
            Dictionary<Unit, long> units = new Dictionary<Unit, long>();
            foreach (var unit in a.BaseUnits)
            {
                if (units.ContainsKey(unit.Key))
                    units[unit.Key] += unit.Value;
                else
                    units.Add(unit.Key, unit.Value);
            }
            foreach (var unit in b.BaseUnits)
            {
                if (units.ContainsKey(unit.Key))
                    units[unit.Key] += unit.Value;
                else
                    units.Add(unit.Key, unit.Value);
            }
            Measure Result = new Measure(value, units);

            return Result;

        }

        public static Measure operator /(Measure a, Measure b)
        {
            double value = a.BaseValue / b.BaseValue;
            Dictionary<Unit, long> units = new Dictionary<Unit, long>();
            foreach (var unit in a.BaseUnits)
            {
                if (units.ContainsKey(unit.Key))
                    units[unit.Key] += unit.Value;
                else
                    units.Add(unit.Key, unit.Value);
            }
            foreach (var unit in b.BaseUnits)
            {
                if (units.ContainsKey(unit.Key))
                    units[unit.Key] -= unit.Value;
                else
                    units.Add(unit.Key, 0 - unit.Value);
            }
            Measure Result = new Measure(value, units);

            return Result;

        }
    }
}
