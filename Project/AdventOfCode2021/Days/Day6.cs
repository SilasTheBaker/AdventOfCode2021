using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Days
{
    class Day6 : DayBase
    {
        List<LanternFish> mFish;
        int mTotalFishCount = 0;

        public override string Name
        {
            get { return "Day 6: Lanternfish"; }
        }

        public Day6()
        {
            mPuzzleInputName += "Day6.txt";
        }

        public override bool Init()
        {
            base.Init();

            mFish = new List<LanternFish>();

            int[] days = mPuzzleInput[0].Split(',').Select(int.Parse).ToArray();

            foreach (int day in days)
            {
                mFish.Add(new LanternFish(day));
                mTotalFishCount++;
            }

            return true;
        }

        public override bool RunSolution1()
        {
            int numDaysToRun = 80;
            int gestationDays = 6;

            for (int i = 0; i < numDaysToRun; i++)
            {
                SBLog.LogLine("Day: " + i);
                for (int f = mFish.Count - 1; f >= 0; f--)
                {
                    bool gaveBirth = mFish[f].ShouldGiveBirth();

                    mFish[f].DecrimentDay();

                    if (gaveBirth)
                    {
                        mTotalFishCount++;
                        mFish.Add(new LanternFish());
                        mFish[f].SetDaysTillBirth(gestationDays);
                    }
                }

                if (mFish.Count % 100 == 0)
                    SBLog.LogLine("Fish Count: " + mFish.Count);

                SBLog.Log(i + " Days: ", SBLog.LogStreamType.Debug);
                for (int j = 0; j < mFish.Count; j++)
                {
                    SBLog.Log(mFish[j].GetDaysTillBirth() + ", ", SBLog.LogStreamType.Debug);
                }
                SBLog.Log("\n", SBLog.LogStreamType.Debug);
            }

            return true;
        }

        public override bool RunSolution2()
        {
            return false;
        }

        public override string GetOutput1()
        {
            return mTotalFishCount.ToString();
        }

        public override string GetOutput2()
        {
            return "None";
        }

        class LanternFish
        {
            int mDaysTillBirth;

            public LanternFish(int daysTillBirth = 8)
            {
                mDaysTillBirth = daysTillBirth;
            }

            public void DecrimentDay()
            {
                mDaysTillBirth--;
            }

            public void SetDaysTillBirth(int days)
            {
                mDaysTillBirth = days;
            }

            public int GetDaysTillBirth()
            {
                return mDaysTillBirth;
            }

            internal bool ShouldGiveBirth()
            {
                return mDaysTillBirth <= 0;
            }
        }
    }
}
