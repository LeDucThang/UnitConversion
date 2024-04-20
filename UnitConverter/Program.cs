using System;

namespace UnitConverter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Measure SpecificWeight01 = new Measure(1000, "kg.cm^-3");
            Console.WriteLine(SpecificWeight01.ToString());

        }
    }
}
