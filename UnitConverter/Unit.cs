using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitConverter
{
    public readonly struct Unit
    {
        public readonly long Id;
        public readonly string Code;
        public readonly string Name;
        public readonly long UnitTypeId;
        public readonly double Factor;
        public Unit(long id, string code, string name, long unitTypeId, double factor)
        {
            this.Id = id;   
            this.Code = code;
            this.Name = name;
            this.UnitTypeId = unitTypeId;
            this.Factor = factor;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Unit))
                return false;

            Unit mys = (Unit)obj;
            return Id == mys.Id;
        }

        public readonly static Unit m = new Unit(150, "m", "Meter", UnitType.LENGTH.Id, 1);
        public readonly static Unit dam = new Unit(151, "dam","Decameter", UnitType.LENGTH.Id, 10);
        public readonly static Unit hm = new Unit(152, "hm", "Hectometer", UnitType.LENGTH.Id, 100);
        public readonly static Unit km = new Unit(153, "km", "Kilometer",UnitType.LENGTH.Id, 1000);
        public readonly static Unit dm = new Unit(149, "dm", "Decimeter",UnitType.LENGTH.Id, 0.1);
        public readonly static Unit cm = new Unit(148, "cm", "Centimeter", UnitType.LENGTH.Id, 0.01);
        public readonly static Unit mm = new Unit(147, "mm", "Milimeter", UnitType.LENGTH.Id, 0.001);

        public readonly static Unit kg = new Unit(250, "kg", "Kilogram ", UnitType.MASS.Id, 1);
        public readonly static Unit yen = new Unit(250, "yen", "yen ", UnitType.MASS.Id, 10);
        public readonly static Unit cwt = new Unit(252, "cwt", "quintal", UnitType.MASS.Id, 100);
        public readonly static Unit ton = new Unit(253, "ton", "ton", UnitType.MASS.Id, 1000);
        public readonly static Unit hg = new Unit(249, "dam", "Hectogram", UnitType.MASS.Id, 0.1);
        public readonly static Unit dag = new Unit(248, "hm", "Decagram", UnitType.MASS.Id, 0.01);
        public readonly static Unit g = new Unit(247, "g", "gram", UnitType.MASS.Id, 0.001);
        public readonly static Unit dg = new Unit(246, "dg", "decigram ", UnitType.MASS.Id, 0.0001);
        public readonly static Unit cg = new Unit(245, "cg", "centigram", UnitType.MASS.Id, 0.00001);
        public readonly static Unit mg = new Unit(244, "mg", "Miligram", UnitType.MASS.Id, 0.000001);


        public readonly static Unit s = new Unit(350, "s", "Second", UnitType.Time.Id, 1);
        public readonly static Unit ms = new Unit(347, "ms", "Milisecond", UnitType.Time.Id, 0.001);
        public readonly static Unit min = new Unit(351, "min", "Minute", UnitType.Time.Id, 60);

        public readonly static List<Unit> ListEnum = new List<Unit>()
        {
            m, dam, hm, km, dm, cm, mm,
            s, ms, min,
            kg, yen, cwt, ton, hg, dag, g, dg, cg, mg,

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
