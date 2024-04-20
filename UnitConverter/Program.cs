using System;

namespace UnitConverter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Measure a = new Measure(100, "cm");
            Measure V = a * a * a;
            Measure p = new Measure(7800, "kg.m^-3");
            Measure g = new Measure(9.80665, "m.s^-2");
            Measure m = V * p;
            Measure Q = m * g;
            Measure A = a * a;
            Measure press = Q / A;

            Console.WriteLine("V: " + V.ToString());
            Console.WriteLine("m: " + m.ToString());
            Console.WriteLine("Q: " + Q.ToString());
            Console.WriteLine("A: " + A.ToString());
            Console.WriteLine("press: " + press.ToString());
           
        }
    }
}
