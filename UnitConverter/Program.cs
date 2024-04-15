using System;

namespace UnitConverter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Measure S = new Measure(100, "km");
            Measure T = new Measure(1000, "s");
            Measure Velocity = S / T;
            Console.WriteLine(Velocity.ToString());

            Measure W = new Measure(100, "m");
            Measure H = new Measure(1000, "cm");
            Measure L = new Measure(100, "dm");
            Measure Volume = W*H*L;
            Measure Weight = new Measure(10, "ton");
            Measure SpecificWeight = Weight/Volume;
            Console.WriteLine(Volume.ToString());
            Console.WriteLine(SpecificWeight.ToString());

        }
    }
}
