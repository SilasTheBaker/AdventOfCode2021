using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2021.Days;

namespace AdventOfCode2021
{
    class AdventOfCode
    {
        static void Main(string[] args)
        {
            SBLog log = new SBLog(new SBLog.LogStreamType[]{
                SBLog.LogStreamType.All,
            });

            SBLog.LogLine("Dir " + System.IO.Directory.GetCurrentDirectory(), SBLog.LogStreamType.Debug);

            DayBase dayToRun = GetDay(-1);

            if (dayToRun == null)
                Console.WriteLine("No day to run found");

            string output = "FAILED";
            Console.WriteLine("=================== [Advent Of Code] ===================");
            Console.WriteLine("\n Running " + dayToRun.Name + "...\n");
            if (dayToRun.Init())
            {
                //Run the solution for part one
                PrintSolutionHeader(1);
                if (dayToRun.RunSolution1())
                {
                    output = dayToRun.GetOutput1();
                }

                Console.WriteLine();
                SBLog.LogHighlightedLine("Output: " + output, SBLog.LogStreamType.Output);

                //Run the solution for part two
                output = "FAILED";
                Console.WriteLine();
                PrintSolutionHeader(2);

                if (dayToRun.RunSolution2())
                {
                    output = dayToRun.GetOutput2();
                }

                Console.WriteLine();
                SBLog.LogHighlightedLine("Output: " + output, SBLog.LogStreamType.Output);
            }
            Console.WriteLine("========================================================");

            Console.ReadLine();
        }

        static void PrintSolutionHeader(int solution)
        {
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("                   Running Solution " + solution + "                  ");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        }

        static void PrintSeparator(string append = "")
        {
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++" + append);
        }

        private static DayBase GetDay(int day)
        {
            DayBase[] days =
            {
                new Day1(),
            };

            if (day < 0)
                day = days.Length - 1;

            if (day >= 0 && day < days.Length)
                return days[day];
            else
                return null;
        }

    }
}
