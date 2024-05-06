using System;
using System.Collections.Generic;

namespace UnitConverter
{
    public readonly struct Unit
    {
        public readonly long Id;
        public readonly string Code;
        public readonly string Name;
        public readonly long UnitTypeId;
        public readonly double Factor;
        public readonly Dictionary<Unit, double> BaseUnits;
        public readonly double BaseFactor;
        public Unit(long id, string code, string name, long unitTypeId, double factor)
        {
            this.Id = id;
            this.Code = code;
            this.Name = name;
            this.UnitTypeId = unitTypeId;
            this.Factor = factor;
        }

        public Unit(long id, string code, string name, long unitTypeId, double factor, Dictionary<Unit, double> baseUnits, double baseFactor)
        {
            this.Id = id;
            this.Code = code;
            this.Name = name;
            this.UnitTypeId = unitTypeId;
            this.Factor = factor;
            this.BaseUnits = baseUnits;
            this.BaseFactor = baseFactor;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Unit))
                return false;

            Unit mys = (Unit)obj;
            return Id == mys.Id;
        }

        public readonly static Unit s = new Unit(UnitType.TIME.Id * 100 + 50, "s", "Second", UnitType.TIME.Id, 1, null, 1);
        public readonly static Unit ms = new Unit(s.Id - 3, "ms", "Milisecond", UnitType.TIME.Id, 0.001);
        public readonly static Unit min = new Unit(s.Id - 1, "min", "Minute", UnitType.TIME.Id, 60);

        public readonly static Unit m = new Unit(UnitType.LENGTH.Id * 100 + 50, "m", "Meter", UnitType.LENGTH.Id, 1, null, 1);
        public readonly static Unit dam = new Unit(m.Id + 1, "dam", "Decameter", UnitType.LENGTH.Id, Math.Pow(10, 1));
        public readonly static Unit hm = new Unit(m.Id + 2, "hm", "Hectometer", UnitType.LENGTH.Id, Math.Pow(10, 2));
        public readonly static Unit km = new Unit(m.Id + 3, "km", "Kilometer", UnitType.LENGTH.Id, Math.Pow(10, 3));
        public readonly static Unit dm = new Unit(m.Id - 1, "dm", "Decimeter", UnitType.LENGTH.Id, Math.Pow(10, -1));
        public readonly static Unit cm = new Unit(m.Id - 2, "cm", "Centimeter", UnitType.LENGTH.Id, Math.Pow(10, -2));
        public readonly static Unit mm = new Unit(m.Id - 3, "mm", "Milimeter", UnitType.LENGTH.Id, Math.Pow(10, -3));

        public readonly static Unit kg = new Unit(UnitType.MASS.Id * 100 + 50, "kg", "Kilogram ", UnitType.MASS.Id, 1, null, 1);
        public readonly static Unit yen = new Unit(kg.Id + 1, "yen", "yen ", UnitType.MASS.Id, 10);
        public readonly static Unit cwt = new Unit(kg.Id + 2, "cwt", "quintal", UnitType.MASS.Id, 100);
        public readonly static Unit ton = new Unit(kg.Id + 3, "ton", "ton", UnitType.MASS.Id, 1000);
        public readonly static Unit hg = new Unit(kg.Id - 1, "dam", "Hectogram", UnitType.MASS.Id, 0.1);
        public readonly static Unit dag = new Unit(kg.Id - 2, "hm", "Decagram", UnitType.MASS.Id, 0.01);
        public readonly static Unit g = new Unit(kg.Id - 3, "g", "gram", UnitType.MASS.Id, 0.001);
        public readonly static Unit dg = new Unit(kg.Id - 4, "dg", "decigram ", UnitType.MASS.Id, 0.0001);
        public readonly static Unit cg = new Unit(kg.Id - 5, "cg", "centigram", UnitType.MASS.Id, 0.00001);
        public readonly static Unit mg = new Unit(kg.Id - 6, "mg", "Miligram", UnitType.MASS.Id, 0.000001);


        public readonly static Unit N = new Unit(UnitType.FORCE.Id * 100 + 50, "N", "Newton", UnitType.FORCE.Id, 1, new Dictionary<Unit, double> { { kg, 1 }, { m, 1 }, { s, -2 } }, 0.101972);
        public readonly static Unit kN = new Unit(N.Id + 3, "kN", "Newton", UnitType.FORCE.Id, Math.Pow(10, 3));

        public readonly static Unit Pa = new Unit(UnitType.PRESSURE.Id * 100 + 50, "Pa", "Pascal", UnitType.PRESSURE.Id, 1, new Dictionary<Unit, double> { { kg, 1 }, { m, -1 }, { s, -2 } }, 0.101972);
        public readonly static Unit MPa = new Unit(Pa.Id + 3, "MPa", "Mega Pascal", UnitType.PRESSURE.Id, Math.Pow(10, 3));

        public readonly static List<Unit> ListEnum = new List<Unit>()
        {
            m, dam, hm, km, dm, cm, mm,
            s, ms, min,
            kg, yen, cwt, ton, hg, dag, g, dg, cg, mg,
            N, kN,
            Pa, MPa,
        };

        public static bool operator ==(Unit left, Unit right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Unit left, Unit right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
