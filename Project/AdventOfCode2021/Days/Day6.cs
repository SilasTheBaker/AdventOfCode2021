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
        long mTotalFishCount = 0;
        int mStartingFishCount = 0;
        long[] mGestationStageCount;

        public override string Name
        {
            get { return "Day 6: Lanternfish"; }
        }

        public Day6()
        {
            mPuzzleInputName += "Day6.txt";
            //mPuzzleInputName += "Day6Example.txt";
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

            mStartingFishCount = days.Length;

            //Index for each gestation day - lazy theres a better way to do this
            mGestationStageCount = new long[9];
            for (int i = 0; i < 7; i++)
            {
                mGestationStageCount[i] = (uint)days.Count(x => x == i);
            }

            return true;
        }

        public override bool RunSolution1()
        {
#if true
            int numDays = 80;
            int currentDay = 0;
            mTotalFishCount = mStartingFishCount;

            for (int i = 0; i < numDays; i++)
            {
                SBLog.Log(i + " Days: ", SBLog.LogStreamType.Debug);
                for (int j = 0; j < mGestationStageCount.Length; j++)
                {
                    SBLog.Log(mGestationStageCount[j] + ((j != currentDay) ? ", " : "*, "), SBLog.LogStreamType.Debug);
                }
                SBLog.Log("\n", SBLog.LogStreamType.Debug);

                long numGivingBirth = mGestationStageCount[currentDay];
                currentDay = ++currentDay % mGestationStageCount.Length;

                //Add parent back into the loop
                mGestationStageCount[(currentDay + 6) % mGestationStageCount.Length] += numGivingBirth;
                mTotalFishCount += numGivingBirth;

            }

            return mTotalFishCount > mStartingFishCount;
#else
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


                SBLog.Log(i + " Days: ", SBLog.LogStreamType.Debug);
                for (int j = 0; j < mFish.Count; j++)
                {
                    SBLog.Log(mFish[j].GetDaysTillBirth() + ", ", SBLog.LogStreamType.Debug);
                }
                SBLog.Log("\n", SBLog.LogStreamType.Debug);
            }
#endif
            return true;
        }

        public override bool RunSolution2()
        {
            int numDays = 256;
            int currentDay = 0;
            mTotalFishCount = mStartingFishCount;

            for (int i = 0; i < numDays; i++)
            {
                SBLog.Log(i + " Days: ", SBLog.LogStreamType.Debug);
                for (int j = 0; j < mGestationStageCount.Length; j++)
                {
                    SBLog.Log(mGestationStageCount[j] + ((j != currentDay) ? ", " : "*, "), SBLog.LogStreamType.Debug);
                }
                SBLog.Log("\n", SBLog.LogStreamType.Debug);

                long numGivingBirth = mGestationStageCount[currentDay];
                currentDay = ++currentDay % mGestationStageCount.Length;

                //Add parent back into the loop
                mGestationStageCount[(currentDay + 6) % mGestationStageCount.Length] += numGivingBirth;
                mTotalFishCount += numGivingBirth;

            }

            return mTotalFishCount > mStartingFishCount;
        }

        public override string GetOutput1()
        {
            return mTotalFishCount.ToString();
        }

        public override string GetOutput2()
        {
            return mTotalFishCount.ToString();
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
