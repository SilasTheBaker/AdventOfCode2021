using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class PuzzleInput : IEnumerable<String>
    {
        string mRawInput = "empty";
        List<string> mInput;
        protected string mInputDir = "";

        int mLinesRead = -1;
        public int LineCount { get { return mLinesRead;  } }

        public string this[int i]
        {
            get { return mInput[i]; }
        }

        public PuzzleInput(string puzzleInputPath)
        {
            mInputDir = Directory.GetCurrentDirectory();
            mInputDir = Path.GetFullPath(Path.Combine(mInputDir, @"..\..\..\..\PuzzleInput\"));
            LoadInput(puzzleInputPath);
        }

        public virtual bool IsValid()
        {
            return mInput != null && mInput.Count > 0;
        }

        public virtual void LoadInput(string inputFile)
        {
            SBLog.LogLine("Attempting to load: " + inputFile, SBLog.LogStreamType.StateInfo);

            string toOpen = mInputDir + inputFile;
            mInput = new List<string>();
            if (!System.IO.File.Exists(toOpen))
            {
                SBLog.LogLine("ERROR: Filepath " + toOpen + " does not exist", SBLog.LogStreamType.Error);
                return;
            }

            mRawInput = System.IO.File.ReadAllText(toOpen);
            foreach (string line in System.IO.File.ReadLines(toOpen))
            {
                mInput.Add(line);
            }
            mLinesRead = mInput.Count;
            SBLog.LogLine("Loaded " + mLinesRead, SBLog.LogStreamType.StateInfo);
        }

        public IntInputEnumerator GetIntIter()
        {
            return new IntInputEnumerator(this);
        }

        public IEnumerator<string> GetEnumerator()
        {
            foreach (string line in mInput)
            {
                yield return line;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class IntInputEnumerator : IEnumerable<int>
        {
            PuzzleInput mPuzzleInput;

            public IntInputEnumerator(PuzzleInput puzzleInput)
            {
                mPuzzleInput = puzzleInput;
            }

            public IEnumerator<int> GetEnumerator()
            {
                foreach (string line in mPuzzleInput.mInput)
                {
                    yield return Int32.Parse(line);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}
