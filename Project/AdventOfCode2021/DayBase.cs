using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class DayBase
    {
        protected string mPuzzleInputName = "";

        public PuzzleInput mPuzzleInput;

        public virtual string Name
        {
            get { return "Not Set"; }
        }

        public virtual bool Init()
        {
            mPuzzleInput = new PuzzleInput(mPuzzleInputName);

            return mPuzzleInput.IsValid();
        }

        public virtual bool RunSolution1()
        {
            SBLog.LogLine("Solution1 not implemented");
            return false;
        }

        public virtual bool RunSolution2()
        {
            SBLog.LogLine("Solution2 not implemented");
            return false;
        }

        public virtual string GetOutput1()
        {
            return "none";
        }

        public virtual string GetOutput2()
        {
            return "none";
        }
    }
}
