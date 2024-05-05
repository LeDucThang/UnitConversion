using System;

namespace UnitConverter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Measure a = new Measure(100, "cm");
            //Measure V = a * a * a;
            //Measure p = new Measure(7.8 * Math.Pow(10, 3), "kg.m^-3");
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

            //Measure x = new Measure(3, "m");
            //Measure y = new Measure(40, "dm");
            //Measure z = new Measure(500, "cm");
            //Measure peri = (x + y + z) / 2;
            //Measure S = MathExt.Sqrt(peri * (peri - x) * (peri - y) * (peri - z));
            //Measure S2 = (peri * (peri - x) * (peri - y) * (peri - z)) ^ 0.5;
            //Console.WriteLine(peri.ToString());
            //Console.WriteLine(S.ToString());
            //Console.WriteLine(S2.ToString());

            Measure newton = new Measure(100, "N");
            Measure length = new Measure(5, "m");
            Measure edge = new Measure(10, "m");
            Measure area = edge * edge;
            Measure moment = newton * length;
            //Measure displayMoment = new Measure(moment, "kN");
            Measure pressure = newton / area;
            Measure displayPressure = new Measure(pressure, "MPa");
            Console.WriteLine(moment.ToString());
            Console.WriteLine(moment.ToBaseString());
            //Console.WriteLine(displayMoment.ToString());
            //Console.WriteLine(displayMoment.ToBaseString());
            Console.WriteLine(displayPressure.ToString());
            Console.WriteLine(displayPressure.ToBaseString());

        }
    }
}
