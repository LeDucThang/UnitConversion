using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitConverter
{
    public class UnitType
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public UnitType(long id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public readonly static UnitType LENGTH = new UnitType(1, "Length");
        public readonly static UnitType MASS = new UnitType(2, "Mass");
        public readonly static UnitType TIME = new UnitType(3, "Time");
        public readonly static UnitType FORCE = new UnitType(4, "Force");
        public readonly static UnitType PRESSURE = new UnitType(5, "Pressure");
        public readonly static UnitType ENERGY = new UnitType(6, "Energy");
        public readonly static UnitType POWER = new UnitType(7, "Power");
      
    }
}
