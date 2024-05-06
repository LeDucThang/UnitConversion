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

        public readonly static UnitType TIME = new UnitType(1, "Time");
        public readonly static UnitType LENGTH = new UnitType(2, "Length");
        public readonly static UnitType MASS = new UnitType(3, "Mass");
        public readonly static UnitType ELECTRIC_CURRENT = new UnitType(4, "Electric current");
        public readonly static UnitType TEMPERATURE = new UnitType(5, "Temperature");
        public readonly static UnitType AMOUNT_SUBSTANCE = new UnitType(6, "Amount of substance");
        public readonly static UnitType LUMINOUS_INTENSITY = new UnitType(7, "luminous intensity");
       
        public readonly static UnitType FORCE = new UnitType(4, "Force");
        public readonly static UnitType PRESSURE = new UnitType(5, "Pressure");
        public readonly static UnitType ENERGY = new UnitType(6, "Energy");
        public readonly static UnitType POWER = new UnitType(7, "Power");
      
    }
}
