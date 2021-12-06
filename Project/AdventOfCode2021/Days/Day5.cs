using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Days
{
    class Day5 : DayBase
    {
        SeaFloorGrid mSeaFloor;
        List<Line> mLines;

        public override string Name
        {
            get { return "Day 5: Hydrothermal Venture"; }
        }

        public Day5()
        {
            mPuzzleInputName += "Day5.txt";
        }

        public override bool Init()
        {
            base.Init();

            mSeaFloor = new SeaFloorGrid(1000);
            mLines = new List<Line>();

            foreach (string line in mPuzzleInput)
            {
                int[] points = line.Replace(" -> ", ",").Split(',').Select(int.Parse).ToArray();
                mLines.Add(new Line(points));
            }

            return true;
        }

        public override bool RunSolution1()
        {
            foreach (Line line in mLines)
            {
                if (line.IsAxisAligned())
                    mSeaFloor.ChartLine(line);
            }

            return true;
        }
        
        public override bool RunSolution2()
        {
            foreach (Line line in mLines)
            {
                if (!line.IsAxisAligned())
                    mSeaFloor.ChartLine(line);
            }

            return true;
        }

        public override string GetOutput1()
        {

            return mSeaFloor.GetIntersectionCount().ToString();
        }

        public override string GetOutput2()
        {
            return mSeaFloor.GetIntersectionCount().ToString();
        }

        class SeaFloorGrid
        {
            int[] mSeaFloor;
            int mSize = 0;

            public SeaFloorGrid(int size)
            {
                //I hate this - there is certainly a better way
                mSeaFloor = new int[size * size];
                mSize = size;
            }

            public void ChartLine(Line line)
            {
                List<Point> points = line.Points;
                for (int i = 0; i < points.Count; i++)
                {
                    mSeaFloor[mSize * points[i].Y + points[i].X]++;
                }
            }

            public int GetIntersectionCount(int threshold = 2)
            {
                int count = 0;
                foreach (int point in mSeaFloor)
                {
                    if (point >= threshold)
                        count++;
                }

                return count;
            }
        }

        class Line
        {
            Point mStart;
            Point mEnd;

            public Line(int[] points)
            {
                if (points.Length != 4)
                    throw new Exception("Incorrect number of components passed to Line");

                mStart = new Point(points[0], points[1]);
                mEnd = new Point(points[2], points[3]);
            }

            public Line(Point start, Point end)
            {
                mStart = start;
                mEnd = end;
            }

            public List<Point> Points
            { 
                get
                {
                    List<Point> points = new List<Point>();

                    if (mEnd.X != mStart.X)
                    {
                        //Horizontal/Diagonal line
                        float slope = (mEnd.Y - mStart.Y) / (mEnd.X - mStart.X);
                        float intercept = mStart.Y - slope * mStart.X;
                        int startX = Math.Min(mStart.X, mEnd.X);
                        int endX = Math.Max(mStart.X, mEnd.X);

                        for (int x = startX; x <= endX; x++)
                        {
                            //Good ol' Y = mx + b
                            int y = (int) (slope * x + intercept);
                            points.Add(new Point(x, y));
                        }
                    }
                    else
                    {
                        //Vertical line
                        int startY = Math.Min(mStart.Y, mEnd.Y);
                        int endY = Math.Max(mStart.Y, mEnd.Y);

                        for (int y = startY; y <= endY; y++)
                        {
                            points.Add(new Point(mStart.X, y));
                        }
                    }

                    return points;
                }
            }

            public bool IsAxisAligned()
            {
                return mStart.X == mEnd.X || mStart.Y == mEnd.Y;
            }
        }

        struct Point
        {
            public int X { get { return mX; } set { mX = value; } }
            public int Y { get { return mY; } set { mY = value; } }
            int mX, mY;

            public Point(int x, int y)
            {
                mX = x;
                mY = y;
            }
        }
    }
}
