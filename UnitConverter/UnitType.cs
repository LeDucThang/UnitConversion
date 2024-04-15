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
        public readonly static UnitType Time = new UnitType(3, "Time");
    }
}
