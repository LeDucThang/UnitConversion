using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitConverter
{
    public static class MathExt
    {
        public static Measure Sqrt(Measure a)
        {
            double value = Math.Sqrt(a.BaseValue);
            Dictionary<Unit, double> units = new Dictionary<Unit, double>();
            foreach (var unit in a.BaseUnits)
            {
                if (units.ContainsKey(unit.Key))
                    units[unit.Key] = unit.Value / 2;
                else
                    units.Add(unit.Key, unit.Value / 2);
            }
            Measure result = new Measure(value, units);
            return result;
        }

        public static Measure Pow(Measure a, double b)
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
