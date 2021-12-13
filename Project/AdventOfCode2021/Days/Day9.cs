using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Days
{
    class Day9 : DayBase
    {
        public override string Name
        {
            get { return "Day 9: Smoke Basin"; }
        }

        HeightMap mHeightMap;
        int mRiskSum = 0;
        int mBasinMult = 0;

        public Day9()
        {
            mPuzzleInputName += "Day9.txt";
            //mPuzzleInputName += "Day9Example.txt";
        }

        public override bool Init()
        {
            base.Init();
            mHeightMap = new HeightMap(mPuzzleInput);

            return mHeightMap != null;
        }

        public override bool RunSolution1()
        {
            foreach (var lowPoint in mHeightMap.GetLowPoints())
            {
                mRiskSum += 1 + lowPoint.Value;
            }

            return mRiskSum > 0;
        }

        public override bool RunSolution2()
        {
            List<int> basinSizes = mHeightMap.GetBasinSizes();

            mBasinMult = basinSizes[0] * basinSizes[1] * basinSizes[2];

            return mBasinMult > 0;// 1652;
        }

        public override string GetOutput1()
        {
            return mRiskSum.ToString();
        }

        public override string GetOutput2()
        {
            return mBasinMult.ToString();
        }

        enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            NumDir
        }

        class HeightMap
        {
            List<int> mHeightMapData;
            int mWidth;

            List<KeyValuePair<int, int>> mLowPoints;

            public int DataCount { get { return mHeightMapData.Count; } }

            public HeightMap(PuzzleInput input)
            {
                mWidth = input[0].Length;
                mHeightMapData = new List<int>(mWidth * input.LineCount);

                string rawStripped = input.Raw.Replace("\n", "").Replace("\r", "");
                foreach (char height in rawStripped)
                {
                    mHeightMapData.Add(int.Parse(new String(height, 1)));
                }
            }

            int GetAdjacentIndex(int index, Direction dir)
            {
                switch (dir)
                {
                    case Direction.Up:
                        index -= mWidth;
                        break;
                    case Direction.Down:
                        index += mWidth;
                        break;
                    case Direction.Left:
                        index--;
                        if ((index + 1) % mWidth == 0)
                            index = -1;
                        break;
                    case Direction.Right:
                        index++;
                        if (index % mWidth == 0)
                            index = -1;
                        break;
                }

                if (index >= 0 && index < DataCount)
                    return index;
                else
                    return -1;
            }

            //index - value
            public List<KeyValuePair<int, int>> GetLowPoints()
            {
                if (mLowPoints == null)
                {
                    mLowPoints = new List<KeyValuePair<int, int>>();

                    for (int i = 0; i < DataCount; i++)
                    {
                        bool lowPoint = true;
                        for (int dir = 0; dir < (int)Direction.NumDir; dir++)
                        {
                            int index = GetAdjacentIndex(i, (Direction)dir);

                            if (index >= 0 && index < mHeightMapData.Count)
                            {
                                if (mHeightMapData[index] <= mHeightMapData[i])
                                {
                                    lowPoint = false;
                                    break;                               
                                }
                            }
                        }

                        if (lowPoint)
                        {
                            mLowPoints.Add(new KeyValuePair<int, int>(i, mHeightMapData[i]));
                        }
                    }
                }

                return mLowPoints;
            }

            //Sorted with highest values first
            public List<int> GetBasinSizes()
            {
                GetLowPoints();
                List<int> basinSizes = new List<int>(mLowPoints.Count);
                foreach (var lowPoint in mLowPoints)
                {
                    int basinSize = GetBasinSize(lowPoint.Key);
                    basinSizes.Add(basinSize);
                }
                basinSizes = basinSizes.OrderByDescending(i => i).ToList();
                return basinSizes;
            }

            int GetBasinSize(int index)
            {
                HashSet<int> visitedIndices = new HashSet<int>();
                CheckBasinTiles(visitedIndices, index);
                return visitedIndices.Count;
            }

            void CheckBasinTiles(HashSet<int> visitedIndices, int index)
            {
                if (index < 0 || visitedIndices.Contains(index))
                    return;

                if (mHeightMapData[index] == 9)
                    return;

                visitedIndices.Add(index);
                int up = GetAdjacentIndex(index, Direction.Up);
                int down = GetAdjacentIndex(index, Direction.Down);
                int left = GetAdjacentIndex(index, Direction.Left);
                int right = GetAdjacentIndex(index, Direction.Right);

                CheckBasinTiles(visitedIndices, up);
                CheckBasinTiles(visitedIndices, down);
                CheckBasinTiles(visitedIndices, left);
                CheckBasinTiles(visitedIndices, right);
            }
        }
    }
}
