using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IO_TPL
{
    class Program
    {
        static public long[] tab;
        static public int maxRand = 100000000;
        static public int minRand = 100;
        static long min = minRand*minRand;
        static Stopwatch sw;
        static long quantThr;
        static long countThr = 0;

        static public void randValueForTab(long size)
        {
            tab = new long[size];
            Random random = new Random();

            for (long i = 0; i < size; i++) 
                tab[i] = random.Next(1000,100000000);
        }

        static public void minOneThr(long[] tab)
        {
            long minV = 100001;

            sw.Start();
            foreach (var elem in tab) 
                if (minV > elem) 
                    minV = elem;
            sw.Stop();

            Console.WriteLine("Minimalna wartość w tablicy to: " + minV);
            Console.WriteLine("Czas obliczania dla jednego watku: " + sw.ElapsedMilliseconds + " [ms]");
            sw.Reset();
        }

        static void minOneThrFromAll(object cT)
        {
            countThr++;
            cT = countThr;

            long startOfRange = 0;
            long minThr = 100001;
            long endOfRange = (tab.Length / quantThr) * (long)cT;
            startOfRange = ((tab.Length / quantThr) * (long)cT - (tab.Length / quantThr));

            for (long i = startOfRange; i < endOfRange; i++) 
                if (minThr > tab[i]) 
                    minThr = tab[i];

            if (min > minThr) 
                min = minThr;
        }


        static void minAllThr(long[] tab)
        {
            sw.Start();
            Parallel.For(0, quantThr, (i) =>
            {
                minOneThrFromAll(i + 1);
            });
            Console.Out.WriteLine("\n\nMinimalna wartość w tablicy to: " + min);
            sw.Stop();
            Console.WriteLine("Czas obliczanie dla Parallel.For(): " + sw.ElapsedMilliseconds + " [ms]");
            sw.Reset();

        }

        static void Main(string[] args)
        {
            quantThr = 10;
            sw = new Stopwatch();
            long size = 100000000;
            randValueForTab(size);
            minOneThr(tab);
            Console.Out.WriteLine("\n------------------------------------------------------------------");
            minAllThr(tab);

        }
    }
}