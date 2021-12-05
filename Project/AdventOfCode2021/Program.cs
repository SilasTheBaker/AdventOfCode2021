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
            SBLog log = new SBLog(new SBLog.LogStreamType[]
            {
                //SBLog.LogStreamType.Debug,
                SBLog.LogStreamType.Default,
                SBLog.LogStreamType.Error,
                //SBLog.LogStreamType.StateInfo,
                SBLog.LogStreamType.Output,
            });

            SBLog.LogLine("Dir " + System.IO.Directory.GetCurrentDirectory(), SBLog.LogStreamType.StateInfo);

            DayBase dayToRun = GetDay();

            if (dayToRun == null)
                Console.WriteLine("No day to run found");

            string output = "FAILED";

            Console.WriteLine("=================== [Advent Of Code] ====================");
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

            Console.WriteLine();
            Console.WriteLine("========================= [Done] ========================");
            PrintChristmasTree();
            Console.ReadLine();
        }

        private static DayBase GetDay(int day = -1)
        {

            DayBase[] days =
            {
                new Day1(),
                new Day2(),
                new Day3(),
                new Day4(),
                new Day5(),
            };

            if (day < 0)
            {
                day = days.Length - 1;
                SBLog.LogLine("Using most recent day Added: " + (day + 1), SBLog.LogStreamType.StateInfo);
            }
            else
            {
                day--; //Arrays start at 0
            }

            if (day >= 0 && day < days.Length)
            {

                SBLog.LogLine("Loading day: " + (day + 1), SBLog.LogStreamType.StateInfo);
                return days[day];
            }
            else
            {
                return null;
            }
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

        static void PrintChristmasTree()
        { 
            //TODO: better spacing and add colors
            Console.WriteLine(@"                          .
                       __/ \__
                       \     /
                       /.'o'.\
                        .o.'.
                       .'.'o'.
                      o'.o.'.o.
                     .'.o.'.'.o.
                    .o.'.o.'.o.'.
                       [_____]
                        \___/ ");
        }
    }
}
