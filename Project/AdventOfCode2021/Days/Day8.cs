using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Days
{
    class Day8 : DayBase
    {
        //Segments - Output Num
        static Dictionary<int, string> UniqueSegmentCounts = new Dictionary<int, string>()
        {
            { 2, "1" },
            { 3, "7" },
            { 4, "4" },
            { 7, "8" },
        };

        NotesEntry[] mNotes;
        int mUniqueSegmentCountOutput = 0;
        int mOutputTotal = 0;
        public override string Name
        {
            get { return "Day 8: Seven Segment Search"; }
        }

        public Day8()
        {
            mPuzzleInputName += "Day8.txt";
            //mPuzzleInputName += "Day8Example.txt";
        }

        public override bool Init()
        {
            base.Init();
            mNotes = new NotesEntry[mPuzzleInput.LineCount];
            for (int i = 0; i < mPuzzleInput.LineCount; i++)
            {
                mNotes[i] = new NotesEntry(mPuzzleInput[i]);
            }

            return mNotes.Length > 0;
        }

        public override bool RunSolution1()
        {
            foreach (NotesEntry entry in mNotes)
            {
                mUniqueSegmentCountOutput += entry.GetNumUniqueSegments();
            }

            return mUniqueSegmentCountOutput > 0;
        }

        public override bool RunSolution2()
        {
            foreach (NotesEntry entry in mNotes)
            {
                mOutputTotal += entry.GetOutputValue();
            }

            return mOutputTotal > 0;
        }

        public override string GetOutput1()
        {
            return mUniqueSegmentCountOutput.ToString();
        }

        public override string GetOutput2()
        {
            return mOutputTotal.ToString();
        }

        class NotesEntry
        {
            string[] mSignalPatterns;
            string[] mOutputs;

            //number - signal
            Dictionary<string, string> mSignalRemap;

            public NotesEntry(string line)
            {
                mSignalPatterns = line.Split('|')[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                mOutputs = line.Split('|')[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < mSignalPatterns.Length; i++)
                {
                    char[] characters = mSignalPatterns[i].ToArray();
                    Array.Sort(characters);
                    mSignalPatterns[i] = new string(characters);
                }

                for (int i = 0; i < mOutputs.Length; i++)
                {
                    char[] characters = mOutputs[i].ToArray();
                    Array.Sort(characters);
                    mOutputs[i] = new string(characters);
                }

                //Value - signal
                mSignalRemap = new Dictionary<string, string>(10);
            }

            public int GetNumUniqueSegments(bool inOutput = true)
            {
                return GetUniqueSegments(inOutput).Count;
            }

            List<string> GetUniqueSegments(bool inOutput)
            {
                string[] toSearch = inOutput ? mOutputs : mSignalPatterns;
                List<string> result = new List<string>();
                foreach (string item in toSearch)
                {
                    if (Day8.UniqueSegmentCounts.Keys.Contains(item.Length))
                        result.Add(item);
                }

                return result;
            }

            bool CalculateDigitRemap()
            {
                foreach (string unique in GetUniqueSegments(false))
                {
                    string realValue = Day8.UniqueSegmentCounts[unique.Length];
                    mSignalRemap.Add(realValue, unique);
                }

                do
                {
                    TryToIdentifyValues();
                } while (mSignalRemap.Count < 10);

                return false;
            }

            private bool TryToIdentifyValues()
            {
                bool foundAValue = false;

                foreach (string signal in mSignalPatterns)
                {
                    if (mSignalRemap.ContainsValue(signal))
                        continue;

                    if (signal.Length == 5)
                    {
                        if (FiveSignalDigit(signal) && !foundAValue)
                            foundAValue = true;
                    }
                    else if (signal.Length == 6)
                    {
                        if (SixSignalDigit(signal) && !foundAValue)
                            foundAValue = true;
                    }
                }

                return foundAValue;
            }

            bool SixSignalDigit(string signal)
            {
                //0, 6, or 9
                if (mSignalRemap.ContainsKey("4"))
                {
                    if (ContainsSignal(signal, mSignalRemap["4"]))
                    {
                        //This is a nine
                        mSignalRemap.Add("9", signal);
                        return true;
                    }
                    else
                    {
                        if (mSignalRemap.ContainsKey("7"))
                        {
                            if (ContainsSignal(signal, mSignalRemap["7"]))
                                mSignalRemap.Add("0", signal);
                            else
                                mSignalRemap.Add("6", signal);

                            return true;
                        }
                    }
                }

                return false;
            }

            bool FiveSignalDigit(string signal)
            {
                //2, 3, or 5
                if (mSignalRemap.ContainsKey("7"))
                {
                    if (ContainsSignal(signal, mSignalRemap["7"]))
                    {
                        //This is a three
                        mSignalRemap.Add("3", signal);
                        return true;
                    }
                    else
                    {
                        if (mSignalRemap.ContainsKey("4"))
                        {
                            //This is a 2 or a 5 - 4 intersects 5 in 3 places
                            if (GetSignalIntersections(signal, mSignalRemap["4"]) == 3)
                                mSignalRemap.Add("5", signal);
                            else
                                mSignalRemap.Add("2", signal);

                            return true;
                        }
                    }
                }

                return true;
            }

            bool ContainsSignal(string number, string other)
            {
                return GetSignalIntersections(number, other) == other.Length;
            }

            int GetSignalIntersections(string number, string other)
            {
                int count = 0;
                foreach (char ch in other)
                {
                    if (number.Contains(ch))
                        count++;
                }

                return count;
            }

            int GetNumber(string inSignal)
            {
                foreach (var remap in mSignalRemap)
                {
                    if (remap.Value == inSignal)
                        return int.Parse(remap.Key);
                }

                return -1;
            }

            public int GetOutputValue()
            {
                if (mSignalRemap.Count < 10)
                    CalculateDigitRemap();

                string output = "";

                for (int i = 0; i < mOutputs.Length; i++)
                {
                    output += GetNumber(mOutputs[i]);
                }

                return int.Parse(output);
            }
        }

    }
}
