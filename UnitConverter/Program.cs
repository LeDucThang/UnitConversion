using System;

namespace UnitConverter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Measure a = new Measure(100, "cm");
            //Measure V = a * a * a;
            //Measure p = new Measure(7800, "kg.m^-3");
            //Measure g = new Measure(9.80665, "m.s^-2");
            //Measure m = V * p;
            //Measure Q = m * g;
            //Measure A = a * a;
            //Measure press = Q / A;

            //Console.WriteLine("V: " + V.ToString());
            //Console.WriteLine("m: " + m.ToString());
            //Console.WriteLine("Q: " + Q.ToString());
            //Console.WriteLine("A: " + A.ToString());
            //Console.WriteLine("press: " + press.ToString());

            Measure a = new Measure(3, "m");
            Measure b = new Measure(40, "dm");
            Measure c = new Measure(500, "cm");
            Measure p = (a + b + c) / 2;
            Measure S = MathExt.Sqrt(p * (p - a) * (p - b) * (p - c));
            Measure S2 = MathExt.Pow(p * (p - a) * (p - b) * (p - c), 0.5);
            Console.WriteLine(p.ToString());
            Console.WriteLine(S.ToString());
            Console.WriteLine(S2.ToString());

        }
    }
}
