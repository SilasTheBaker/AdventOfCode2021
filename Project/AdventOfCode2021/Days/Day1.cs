using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Days
{
    class Day1 : DayBase
    {
        int mIncreaseCount = 0;
        int mSumIncreaseCount = 0;

        public override string Name
        {
            get { return "Day 1: Sonar Sweep"; }
        }

        public Day1()
        {
            mPuzzleInputName += "Day1.txt";
        }

        public override bool RunSolution1()
        {
            int previousMeasurement = int.MaxValue;
            int count = 0;

            SBLog.LogLine("Looping through entries");
            foreach (int depth in mPuzzleInput.GetIntIter())
            {
                if (count != 0 && previousMeasurement < depth)
                    mIncreaseCount++;

                previousMeasurement = depth;
                count++;
            }
            
            SBLog.LogLine("Found " + mIncreaseCount + " increases");
            return mIncreaseCount > 0;
        }

        public override bool RunSolution2()
        {
            SBLog.LogLine("Looping through entries");
            const int sampleSize = 3;
            for (int i = 0; i < mPuzzleInput.LineCount; i++)
            {
                if (i >= sampleSize)
                {
                    //We have enough data to compare two samples
                    int prevSum = GetSumOfPrevEntries(i-1, sampleSize);
                    int currSum = GetSumOfPrevEntries(i, sampleSize);

                    if (prevSum < currSum)
                        mSumIncreaseCount++;
                }
            }

            SBLog.LogLine("Found " + mSumIncreaseCount + " increases");
            return mIncreaseCount > 0;
        }

        public int GetSumOfPrevEntries(int lastIndex, int entriesToSample)
        {
            int sum = 0;
            for (int j = 0; j < entriesToSample; j++)
            {
                int depth = Int32.Parse(mPuzzleInput[lastIndex - j]);
                sum += depth;
            }
            return sum;
        }

        public override string GetOutput1()
        {
            return mIncreaseCount.ToString();
        }

        public override string GetOutput2()
        {
            return mSumIncreaseCount.ToString();
        }
    }
}
