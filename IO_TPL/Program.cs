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
        static long min = maxRand + 1;
        static Stopwatch sw;
        static long quantThr;
        static long countThr = 0;
        static public Random random;

        static public void randValueForTab(long size)
        {
            tab = new long[size];

            Parallel.Invoke(() =>
            {
                initTabThr(1);
            },
            () =>
            {
                initTabThr(2);
            },
            () =>
            {
                initTabThr(3);
            },
            () =>
            {
                initTabThr(4);
            },
            () =>
            {
                initTabThr(5);
            },
            () =>
            {
                initTabThr(6);
            },
            () =>
            {
                initTabThr(7);
            },
            () =>
            {
                initTabThr(8);
            },
            () =>
            {
                initTabThr(9);
            },
            () =>
            {
                initTabThr(10);
            }
            );

        }

        static public void initTabThr(long cT)
        {
            long startOfRange = 0;
            long endOfRange = (tab.Length / quantThr) * cT;
            startOfRange = ((tab.Length / quantThr) * cT - (tab.Length / quantThr));

            for (long i = startOfRange; i < endOfRange; i++)
                tab[i] = random.Next(minRand, maxRand);

        }

        static public void minOneThr(long[] tab)
        {
            long minV = maxRand + 1;

            sw.Start();
            foreach (var elem in tab)
                if (minV > elem)
                    minV = elem;
            sw.Stop();

            Console.WriteLine("Minimalna wartość w tablicy to: " + minV);
            Console.WriteLine("Czas obliczania dla jednego watku: " + sw.ElapsedMilliseconds + " [ms]");
            sw.Reset();
        }

        static private System.Object lockThis = new System.Object();
        static void minOneThrFromAll(object cT)
        {
            long startOfRange = 0;
            long minThr = maxRand + 1;
            long endOfRange = (tab.Length / quantThr) * (long)cT;
            startOfRange = ((tab.Length / quantThr) * (long)cT - (tab.Length / quantThr));

            for (long i = startOfRange; i < endOfRange; i++)
                if (minThr > tab[i])
                    minThr = tab[i];

            Console.Out.WriteLine("Minimalna wartość w tablicy dla watku: " + minThr + " cT: " + (long)cT);

            lock (lockThis)
            {
                if (min > minThr)
                {
                    //Thread.Sleep((int)((long)cT) * 1000);
                    min = minThr;
                }
            }
        }


        static void minAllThr(long[] tab)
        {
            sw.Start();
            Parallel.For(0, quantThr, (cT) =>
            {
                minOneThrFromAll(++countThr);
            });
            Console.Out.WriteLine("Minimalna wartość w tablicy to: " + min);
            sw.Stop();
            Console.WriteLine("Czas obliczanie dla Parallel.For(): " + sw.ElapsedMilliseconds + " [ms]");
            sw.Reset();

        }

        static void Main(string[] args)
        {
            random = new Random(System.Environment.TickCount);
            quantThr = 10;
            sw = new Stopwatch();
            long size = 100000000;
            randValueForTab(size);
            minOneThr(tab);
            Console.Out.WriteLine("------------------------------------------------------------------");
            minAllThr(tab);
            Console.Out.WriteLine("------------------------------------------------------------------");
        }
    }
}