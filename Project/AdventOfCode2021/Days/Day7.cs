using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Days
{
    //https://adventofcode.com/2021/day/7
    class Day7 : DayBase
    {
        int mCheapestFuel = int.MaxValue;
        int[] mCrabPositions;
        int mMaxCrab = 0;
        public override string Name
        {
            get { return "Day 7: The Treachery of Whales"; }
        }

        public Day7()
        {
            mPuzzleInputName += "Day7.txt";
            //mPuzzleInputName += "Day7Example.txt";
        }

        public override bool Init()
        {
            base.Init();

            mCrabPositions = mPuzzleInput[0].Split(',').Select(int.Parse).ToArray();

            mMaxCrab = mCrabPositions.Max();

            return mCrabPositions.Length > 0 && mMaxCrab > 0;
        }

        public override bool RunSolution1()
        {
            int cheapestFuleCost = int.MaxValue;

            for (int i = 0; i < mMaxCrab; i++)
            {
                int fuelCost = 0;
                foreach (int crabPos in mCrabPositions)
                {
                    int max = Math.Max(crabPos, i);
                    int min = Math.Min(crabPos, i);
                    fuelCost += max - min;

                    if (fuelCost >= cheapestFuleCost)
                        break;
                }

                if (fuelCost < cheapestFuleCost)
                    cheapestFuleCost = fuelCost;
            }

            mCheapestFuel = cheapestFuleCost;
            return cheapestFuleCost < int.MaxValue;
        }

        public override bool RunSolution2()
        {
            int cheapestFuleCost = int.MaxValue;

            for (int i = 0; i < mMaxCrab; i++)
            {
                int fuelCost = 0;
                foreach (int crabPos in mCrabPositions)
                {
                    int max = Math.Max(crabPos, i);
                    int min = Math.Min(crabPos, i);
                    int delta = max - min;

                    fuelCost += (delta * (delta + 1)) / 2;

                    if (fuelCost >= cheapestFuleCost)
                        break;
                }

                if (fuelCost < cheapestFuleCost)
                    cheapestFuleCost = fuelCost;
            }

            mCheapestFuel = cheapestFuleCost;
            return cheapestFuleCost < int.MaxValue;
        }

        public override string GetOutput1()
        {
            return mCheapestFuel.ToString();
        }

        public override string GetOutput2()
        {
            return mCheapestFuel.ToString();
        }
    }
}
