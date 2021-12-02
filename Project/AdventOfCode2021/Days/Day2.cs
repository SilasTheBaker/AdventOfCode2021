using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Days
{
    class Day2 : DayBase
    {
        int mMoveDeltaMult = 0;
        int mAimDeltaMult = 0;

        public override string Name
        {
            get { return "Day 2: Dive!"; }
        }

        public Day2()
        {
            mPuzzleInputName += "Day2.txt";
        }

        public override bool RunSolution1()
        {
            int hMove = 0;
            int vMove = 0;

            foreach (string[] line in mPuzzleInput.GetSplitLinesIter())
            {
                string dir = GetDirection(line);
                int delta = GetMoveDelta(line);

                switch (dir)
                {
                    case "forward":
                        hMove += delta;
                        break;
                    case "up":
                        vMove -= delta;
                        break;
                    case "down":
                        vMove += delta;
                        break;
                    default:
                        SBLog.LogLine("ERROR: Unknown Direction found: " + dir, SBLog.LogStreamType.Error);
                        return false;
                }
            }

            SBLog.LogLine("Horizontal Delta: " + hMove + " Depth Delta: " + vMove, SBLog.LogStreamType.Default);

            mMoveDeltaMult = hMove * vMove;
            return mMoveDeltaMult > 0;
        }

        public override bool RunSolution2()
        {
            int hMove = 0;
            int vMove = 0;
            int aim = 0;

            foreach (string[] line in mPuzzleInput.GetSplitLinesIter())
            {
                string dir = GetDirection(line);
                int delta = GetMoveDelta(line);

                switch (dir)
                {
                    case "forward":
                        hMove += delta;
                        vMove += aim * delta;
                        break;
                    case "up":
                        aim -= delta;
                        break;
                    case "down":
                        aim += delta;
                        break;
                    default:
                        SBLog.LogLine("ERROR: Unknown Direction found: " + dir, SBLog.LogStreamType.Error);
                        return false;
                }
            }

            SBLog.LogLine("Horizontal Delta: " + hMove + " Depth Delta: " + vMove, SBLog.LogStreamType.Default);

            mAimDeltaMult = hMove * vMove;
            return mAimDeltaMult > 0;
        }

        public string GetDirection(string[] line)
        {
            if (line == null)
                return "none";

            return line[0];
        }

        public int GetMoveDelta(string[] line)
        {
            if (line == null)
                return 0;

            return Int32.Parse(line[1]);
        }

        public override string GetOutput1()
        {
            return mMoveDeltaMult.ToString();
        }

        public override string GetOutput2()
        {
            return mAimDeltaMult.ToString();
        }
    }
}