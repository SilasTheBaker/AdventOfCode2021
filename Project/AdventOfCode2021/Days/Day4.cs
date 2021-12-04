using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Days
{
    //https://adventofcode.com/2021/day/4
    class Day4 : DayBase
    {
        List<BingoBoard> mBoards;
        int[] mNumbersToCall;

        int mWinningBoardScore = -1;
        int mLastWinnerBoardScore = -1;
        int mNumbersCalled = 0;

        public override string Name
        {
            get { return "Day 4: Giant Squid"; }
        }

        public Day4()
        {
            mPuzzleInputName += "Day4.txt";
        }

        public override bool Init()
        {
            base.Init();

            //Fist line in the input is the called numbers
            mNumbersToCall = mPuzzleInput[0].Split(',').Select(int.Parse).ToArray();

            mBoards = new List<BingoBoard>();
            int index = 1;
            do
            {
                if (mPuzzleInput[index] == "")
                {
                    //New board
                    mBoards.Add(new BingoBoard());
                    index++;
                }

                List<int> boardNumbers = new List<int>();
                do
                {
                    string[] splitRow = mPuzzleInput[index].Replace("  ", " ").TrimStart(' ').Split(' ');
                    boardNumbers.AddRange(splitRow.Select(int.Parse).ToArray());
                    if (++index == mPuzzleInput.LineCount)
                        break;

                } while (mPuzzleInput[index] != "");

                mBoards.Last().SetBoard(boardNumbers);
            } while (index != mPuzzleInput.LineCount);

            return true;
        }

        public override bool RunSolution1()
        {
            bool bingoCalled = false;
            foreach (int call in mNumbersToCall)
            {
                mNumbersCalled++;
                foreach (BingoBoard board in mBoards)
                {
                    if (board.CallNumber(call))
                    {

                        //We have a bingo!
                        mWinningBoardScore = board.GetBoardScore();
                        bingoCalled = true; //Make sure the last call is finished
                    }
                }

                if (bingoCalled)
                    return true;
            }

            return false;
        }

        public override bool RunSolution2()
        {
            int losingBoards = mBoards.Count(x => !x.HasBingo());
            int call = 0;
            BingoBoard lastWinner = null;

            for (int i = mNumbersCalled; i < mNumbersToCall.Length; i++)
            {
                call = mNumbersToCall[i];
                foreach (BingoBoard board in mBoards)
                {
                    if (!board.HasBingo() && board.CallNumber(call))
                    {
                        //We have a bingo!
                        mWinningBoardScore = board.GetBoardScore();
                        if (--losingBoards == 0)
                            break;
                    }
                }

                if (losingBoards == 1 && lastWinner == null)
                    lastWinner = mBoards.Find(x => !x.HasBingo());
            }

            mLastWinnerBoardScore = lastWinner != null ? lastWinner.GetBoardScore() : -1;

            return mLastWinnerBoardScore > 0;
        }

        public override string GetOutput1()
        {
            return mWinningBoardScore.ToString();
        }

        public override string GetOutput2()
        {
            return mLastWinnerBoardScore.ToString();
        }
        
        class BingoBoard
        {
            int mBoardSize;

            int[] mBoard;
            int mBoardScore;
            bool mHasBingo = false;

            //We don't actually need to track called numbers
            //Just if a row or column has had 5 found numbers in it
            int[] mFoundColumn;
            int[] mFoundRows;

            public void SetBoard(List<int> boardNumbers)
            {
                mBoardSize = (int)Math.Sqrt(boardNumbers.Count);
                mBoard = boardNumbers.ToArray();
                mFoundColumn = new int[mBoardSize];
                mFoundRows = new int[mBoardSize];

                foreach (int number in mBoard)
                {
                    mBoardScore += number;
                }
            }

            //-1 if not found
            int SearchFor(int num)
            {
                return Array.IndexOf(mBoard, num);
            }

            //Returns true on a bingo
            public bool CallNumber(int num)
            {
                int index = SearchFor(num);

                if (index != -1)
                {
                    mBoardScore -= num;
                    int x, y;
                    GetXY(index, out x, out y);
                    mFoundColumn[x]++;
                    mFoundRows[y]++;
                    if (mFoundColumn[x] == mBoardSize || mFoundRows[y] == mBoardSize)
                    {
                        mBoardScore *= num;
                        mHasBingo = true;
                        return true;
                    }
                }

                return false;
            }

            public bool HasBingo(bool forceCheck = false)
            {
                return !forceCheck ? mHasBingo : mFoundColumn.Contains(mBoardSize) || mFoundRows.Contains(mBoardSize); 
            }

            //Only valid if a bingo has been called
            public int GetBoardScore()
            {
                return mBoardScore;
            }

            void GetXY(int index, out int x, out int y)
            {
                x = index % mBoardSize;
                y = index / mBoardSize;
            }
        }
    }

}
