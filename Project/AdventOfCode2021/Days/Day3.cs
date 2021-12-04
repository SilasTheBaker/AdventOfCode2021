using SBExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Days
{
    //https://adventofcode.com/2021/day/3
    class Day3 : DayBase
    {
        DiagnosticReadout mReadout;
        int mPowerUsage = 0;
        int mLifeSupportRating = 0;

        public override string Name
        {
            get { return "Day 3: Binary Diagnostic"; }
        }

        public Day3()
        {
            mPuzzleInputName += "Day3.txt";
            //mPuzzleInputName += "Day3Example.txt";
        }

        public override bool Init()
        {
            base.Init();

            mReadout = new DiagnosticReadout(mPuzzleInput);

            return true;
        }

        public override bool RunSolution1()
        {
            mPowerUsage = mReadout.GetGammaRate() * mReadout.GetEpsilonRate();

            return mPowerUsage > 0;
        }

        public override bool RunSolution2()
        {
            mLifeSupportRating = mReadout.GetOxygenGeneratorRating() * mReadout.GetCO2ScrubberRating();

            return mLifeSupportRating > 0;
        }

        public override string GetOutput1()
        {
            return mPowerUsage.ToString();
        }

        public override string GetOutput2()
        {
            return mLifeSupportRating.ToString();
        }

        class DiagnosticEntry
        {
            byte[] mBits;

            public int BitCount { get { return mBits.Length; } }

            public DiagnosticEntry(string line)
            {
                mBits = new byte[line.Length];

                for (int i = 0; i < line.Length; i++)
                {
                    mBits[i] = (byte)(line[i] - '0'); //spooky hack
                }
            }

            public DiagnosticEntry(int size)
            {
                mBits = new byte[size];
            }

            public DiagnosticEntry(DiagnosticEntry other)
            {
                mBits = new byte[other.BitCount];

                for (int i = 0; i < other.BitCount; i++)
                {
                    mBits[i] = (byte)other[i];
                }
            }

            public int this[int i]
            {
                get { return mBits[i]; }
                set { mBits[i] = (byte)value; }
            }

            public int GetInt()
            {
                int decimalNum = 0;
                int lastIdx = mBits.Length - 1;

                for (int i = 0; i < mBits.Length; i++)
                {
                    decimalNum += mBits[i] * (int)Math.Pow(2, lastIdx - i);
                }

                return decimalNum;
            }

            public string ToBitString()
            {
                var sb = new StringBuilder();

                for (int i = 0; i < mBits.Length; i++)
                {
                    char c = mBits[i] == 1 ? '1' : '0';
                    sb.Append(c);
                }

                return sb.ToString() + " (" + GetInt() + ")";
            }
        }

        class DiagnosticReadout
        {
            List<DiagnosticEntry> mEntries;

            int mGammaRate = -1;
            int mEpsilonRate = -1;
            int mOxygenGeneratorRating = -1;
            int mCO2ScrubberRating = -1;

            public DiagnosticReadout(PuzzleInput puzzleInput)
            {
                mEntries = new List<DiagnosticEntry>();
                foreach (string line in puzzleInput)
                {
                    mEntries.Add(new DiagnosticEntry(line));
                }
            }

            public int GetGammaRate()
            {
                if (mGammaRate < 0)
                {
                    ParsePower();
                }

                return mGammaRate;
            }

            public int GetEpsilonRate()
            {
                if (mEpsilonRate < 0)
                {
                    ParsePower();
                }

                return mEpsilonRate;
            }

            void ParsePower()
            {
                if (mEntries != null && mEntries.Count > 0)
                {
                    int bitsPerEntry = mEntries[0].BitCount;
                    int numEntries = mEntries.Count;

                    int[] zeroCount = new int[bitsPerEntry];
                    foreach (DiagnosticEntry entry in mEntries)
                    {
                        for (int i = 0; i < bitsPerEntry; i++)
                        {
                            if (entry[i] == 0)
                                zeroCount[i]++;
                        }
                    }

                    DiagnosticEntry gammaRate = new DiagnosticEntry(bitsPerEntry);
                    DiagnosticEntry epsilonRate = new DiagnosticEntry(bitsPerEntry);

                    for (int i = 0; i < bitsPerEntry; i++)
                    {
                        int mostCommonBit = zeroCount[i] < numEntries / 2 ? 1 : 0;
                        gammaRate[i] = mostCommonBit;
                        epsilonRate[i] = mostCommonBit == 1 ? 0 : 1;
                    }

                    SBLog.LogLine("Gamma Rate: " + gammaRate.GetInt().ToString());
                    SBLog.LogLine("Epsilon Rate: " + epsilonRate.GetInt().ToString());

                    mGammaRate = gammaRate.GetInt();
                    mEpsilonRate = epsilonRate.GetInt();
                }
            }

            public int GetOxygenGeneratorRating()
            {
                if (mOxygenGeneratorRating > 0)
                    return mOxygenGeneratorRating;

                if (mEntries != null && mEntries.Count > 0)
                {
                    DiagnosticEntry result = FilterResultsByIndexMode(mEntries, true);

                    mOxygenGeneratorRating = result.GetInt();
                    SBLog.LogLine("Oxygen Generator Rating: " + mOxygenGeneratorRating);
                    return mOxygenGeneratorRating;
                }

                return -1;
            }

            public int GetCO2ScrubberRating()
            {
                if (mCO2ScrubberRating > 0)
                    return mCO2ScrubberRating;

                if (mEntries != null && mEntries.Count > 0)
                {
                    DiagnosticEntry result = FilterResultsByIndexMode(mEntries, false);

                    mCO2ScrubberRating = result.GetInt();
                    SBLog.LogLine("CO2 Scrubber Rating: " + mCO2ScrubberRating);
                    return mCO2ScrubberRating;
                }

                return -1;
            }

            //What the hell should this be named
            DiagnosticEntry FilterResultsByIndexMode(List<DiagnosticEntry> entries, bool getMostCommon)
            {
                int bitsPerEntry = entries[0].BitCount;
                List<DiagnosticEntry> validEntries = new List<DiagnosticEntry>(entries);

                for (int i = 0; i < bitsPerEntry; i++)
                {
                    int mostCommon = GetMostCommonBit(i, validEntries);
                    bool tie = mostCommon == 2;
                    int toRemove = -1;

                    if (getMostCommon)
                    {
                        toRemove = mostCommon == 1 || tie ? 0 : 1;
                    }
                    else
                    {
                        toRemove = mostCommon == 1 || tie ? 1 : 0;
                    }

                    validEntries.RemoveAll(x => x[i] == toRemove);

                    if (validEntries.Count == 1)
                    {
                        SBLog.LogLine("FilterResultsByIndexMode() took " + (i + 1) + " itterations for " + (getMostCommon ? "most common. " : "least common. ") + "Found: " + validEntries[0].ToBitString(), SBLog.LogStreamType.Debug);
                        return validEntries[0];
                    }
                }

                return null;
            }

            //Returns the most common bit value - 2 if there is a tie
            int GetMostCommonBit(int index, List<DiagnosticEntry> entries)
            {
                //SBLog.ScopedLogFilter debugFilter = new SBLog.ScopedLogFilter(new SBLog.LogStreamType[]{ SBLog.LogStreamType.Debug }, true);
                if (entries != null && entries.Count > 0)
                {
                    int zeroCount = 0;
                    foreach (DiagnosticEntry entry in entries)
                    {
                        SBLog.LogLine("GetMostCommonBit() Index: " + index + " Entry: " + entry.ToBitString(), SBLog.LogStreamType.Debug);
                        if (entry[index] == 0)
                            zeroCount++;
                    }
                    
                    SBLog.LogLine("GetMostCommonBit() zeroCount: " + zeroCount + " Entry count: " + entries.Count, SBLog.LogStreamType.Debug);
                    
                    if (zeroCount == entries.Count - zeroCount)
                    {
                        return 2; //was a tie
                    }
                    
                    return zeroCount > entries.Count - zeroCount ? 0 : 1;
                }

                return -1;
            }
        }
    }
}
