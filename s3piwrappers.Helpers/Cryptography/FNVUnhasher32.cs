﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace s3piwrappers.Helpers.Cryptography
{
    public class FNVUnhasher32
    {
        public const byte kNotFoundChar = 0x50; // P = 0x50

        private static string BytesToString(byte[] value, int length)
        {
            if (value == null || length < 0 || length > value.Length)
                return "";
            char[] result = new char[length];
            if (length > 0)
                Array.Copy(value, 0, result, 0, length);
            else
                return "";
            return new string(result);
        }

        private FNVSearchTable mSearchTable;
        private int mLowerCharsLength;
        private byte[] mLowerChars;
        private int mPrefixLength;
        private byte[] mPrefixChars;
        private int mSuffixLength;
        private byte[] mSuffixChars;

        private uint mFilter;
        private bool mXorFold;
        private readonly uint mTargetHash;
        private int mMaxLevel;
        private int mMaxMatches;
        private ulong mMaxIterations;

        private int mMatchCount;
        private byte[][] mMatch;
        private Matcher[] mMatchers;

        private bool mStarted = false;
        private bool mFinished = false;
        private string[] mResults;
        private DateTime mStartTime;
        private DateTime[] mEndTimes;

        public FNVSearchTable SearchTable
        {
            get { return this.mSearchTable; }
        }

        public uint Filter
        {
            get { return this.mFilter; }
        }

        public bool XorFold
        {
            get { return this.mXorFold; }
        }

        public uint TargetHash
        {
            get { return this.mTargetHash; }
        }

        public int MaxResultLength
        {
            get { return this.mMaxLevel; }
        }

        public ulong MaxIterations
        {
            get { return this.mMaxIterations; }
        }

        public ulong Iterations
        {
            get
            {
                if (this.mMatchers == null)
                    return 0;
                ulong iterations = 0;
                for (int i = 0; i < this.mMatchers.Length; i++)
                {
                    iterations += this.mMatchers[i].Iterations;
                }
                return iterations;
            }
        }

        public bool Finished
        {
            get { return this.mFinished; }
        }

        public int ResultCount
        {
            get { return this.mMatchCount; }
        }

        public string[] Results
        {
            get { return this.GetResults(); }
        }

        public TimeSpan[] ElapsedTimes
        {
            get
            {
                if (this.mEndTimes == null)
                    return null;
                TimeSpan[] elapsed = new TimeSpan[this.mMatchCount];
                for (int i = 0; i < this.mMatchCount; i++)
                {
                    elapsed[i] = this.mEndTimes[i] - this.mStartTime;
                }
                return elapsed;
            }
        }

        public string[] ElapsedTimeStrings
        {
            get
            {
                if (this.mEndTimes == null)
                    return null;
                string[] elapsed = new string[this.mMatchCount];
                for (int i = 0; i < this.mMatchCount; i++)
                {
                    elapsed[i] = (this.mEndTimes[i] - this.mStartTime).ToString();
                }
                return elapsed;
            }
        }

        private string[] GetResults()
        {
            if (this.mResults == null)
                return null;
            string prefixStr = BytesToString(this.mPrefixChars, this.mPrefixLength);
            string suffixStr = BytesToString(this.mSuffixChars, this.mSuffixLength);
            for (int i = this.mMatchCount - 1; i >= 0; i--)
            {
                if (this.mResults[i] == null)
                {
                    this.mResults[i] = prefixStr;
                    for (int j = 0; j < this.mMaxLevel; j++)
                    {
                        if (this.mMatch[i][j] == kNotFoundChar)
                            break;
                        this.mResults[i] += (char)this.mMatch[i][j];
                    }
                    this.mResults[i] += suffixStr;
                }
            }
            return this.mResults;
        }

        public FNVUnhasher32(uint hash, FNVSearchTable searchTable, 
            int maxChars = 10, int maxMatches = 5, 
            bool xorFold = false, uint filter = uint.MaxValue)
        {
            this.mSearchTable = searchTable;
            this.mTargetHash = hash;
            this.Reset(maxChars, maxMatches, xorFold, filter);
        }

        public void Reset(int maxChars = 10, int maxMatches = 5, 
            bool xorFold = false, uint filter = uint.MaxValue)
        {
            // Only reset if the it hasn't started yet 
            // or it's been started and has finished
            if (this.mStarted && !this.mFinished) // !(!this.mStarted || this.mFinished)
                return;
            this.mStarted = false;
            this.mFinished = false;

            // Copied from the search table in case the user modifies it
            // while the unhasher is running.
            this.mLowerCharsLength = this.mSearchTable.Count;
            this.mLowerChars = this.mSearchTable.Table;
            this.mPrefixChars = this.mSearchTable.PrefixBytes;
            this.mPrefixLength = this.mPrefixChars.Length;
            this.mSuffixChars = this.mSearchTable.SuffixBytes;
            this.mSuffixLength = this.mSuffixChars.Length;

            this.mMaxLevel = maxChars;
            this.mMaxMatches = maxMatches;
            this.mFilter = filter;
            this.mXorFold = xorFold;

            // Fail-safe in case another thread finds a match
            // between the time the final match is found
            // and the unhasher stops all threads.
            // Technically this should never occur since
            // the parent stops all threads within a locked 
            // block of code, but just in case.
            maxMatches += Environment.ProcessorCount << 1;
            this.mMatch = new byte[maxMatches][];
            this.mResults = new string[maxMatches];
            this.mEndTimes = new DateTime[maxMatches];
            int i, j;
            for (i = 0; i < maxMatches; i++)
            {
                this.mResults[i] = null;
                this.mMatch[i] = new byte[maxChars];
                for (j = 0; j < maxChars; j++)
                {
                    this.mMatch[i][j] = kNotFoundChar;
                }
            }
            this.mMaxIterations = 1;
            for (j = 0; j < maxChars; j++)
            {
                this.mMaxIterations *= (ulong)this.mLowerCharsLength;
            }
            this.InitMatchers();
        }

        private void InitMatchers()
        {
            int i, step, threadCount = Environment.ProcessorCount << 1;
            step = this.mLowerCharsLength / threadCount;
            this.mMatchers = new Matcher[threadCount];
            this.mMatchers[0] = new Matcher(this, 0, step);
            threadCount--;
            for (i = 1; i < threadCount; i++)
            {
                this.mMatchers[i] = new Matcher(this, i * step, (i + 1) * step);
            }
            this.mMatchers[threadCount] = new Matcher(this, i * step, this.mLowerCharsLength);
        }

        ~FNVUnhasher32()
        {
            this.Stop();
        }

        public void Dispose()
        {
            this.Stop();
        }

        public void Stop()
        {
            if (this.mStarted && !this.mFinished)
            {
                this.mFinished = true;
                for (int i = 0; i < this.mMatchers.Length; i++)
                {
                    this.mMatchers[i].StopThread();
                }
            }
        }

        public void Start()
        {
            if (!this.mStarted)
            {
                this.mStartTime = DateTime.Now;
                int i;
                for (i = 0; i < this.mMaxMatches; i++)
                {
                    this.mEndTimes[i] = this.mStartTime;
                }
                for (i = 0; i < this.mMatchers.Length; i++)
                {
                    this.mMatchers[i].InitThread();
                }
                for (i = 0; i < this.mMatchers.Length; i++)
                {
                    this.mMatchers[i].Thread.Start();
                }
                this.mStarted = true;
            }
        }

        private class Matcher
        {
            public Thread Thread;
            private readonly FNVUnhasher32 mParent;
            private readonly int mMinIndex;
            private readonly int mMaxIndex;

            public ulong Iterations = 0;
            private bool mFinished = false;

            public Matcher(FNVUnhasher32 parent, int minIndex, int maxIndex)
            {
                this.mParent = parent;
                this.mMinIndex = minIndex;
                this.mMaxIndex = maxIndex;
            }

            public void InitThread()
            {
                this.Thread = new Thread(new ThreadStart(this.MatchFinder));
                this.Thread.Priority = ThreadPriority.BelowNormal;
                this.Thread.Name = string.Format("0x{0:X8}:({1}-{2})", this.mParent.mTargetHash,
                    (char)this.mParent.mLowerChars[this.mMinIndex],
                    (char)this.mParent.mLowerChars[this.mMaxIndex - 1]);
            }

            public void StopThread()
            {
                this.mFinished = true;
                if (this.Thread != null)
                {
                    this.Thread.Join();
                    this.Thread = null;
                }
            }

#if UNSAFE
            unsafe
#endif
            private void MatchFinder()
            {
                bool xorFold = this.mParent.mXorFold;
                int i, j, stop, offset, lcLength = this.mParent.mLowerCharsLength;
                int level, maxLevel = this.mParent.mMaxLevel;
                int minIndex = this.mMinIndex;
                int maxIndex = this.mMaxIndex;
                uint result, product, current, target, tester, filter = this.mParent.mFilter;
                target = this.mParent.mTargetHash & filter;
                //bool finished = false;
#if UNSAFE
                int* index = stackalloc int[maxLevel];
                uint* prevResult = stackalloc uint[maxLevel];
                for (i = 0; i < maxLevel; i++)
                {
                    index[i] = 0;
                    prevResult[i] = 0;
                }
                index[0] = minIndex;

                uint prefixHash = FNVHash.TS3Offset32;
                int suffixLength;
                byte* chars = stackalloc byte[lcLength];
                //lock (this.mParent.mLowerChars)
                {
                    fixed (byte* lcChars = this.mParent.mLowerChars)
                    {
                        for (i = 0; i < lcLength; i++)
                        {
                            chars[i] = lcChars[i];
                        }
                    }
                }
                //lock (this.mParent.mPrefixChars)
                {
                    suffixLength = this.mParent.mPrefixLength;
                    fixed (byte* pChars = this.mParent.mPrefixChars)
                    {
                        for (i = 0; i < suffixLength; i++)
                        {
                            prefixHash = (prefixHash * FNVHash.TS3Prime32) ^ pChars[i];
                        }
                    }
                    suffixLength = this.mParent.mSuffixLength;
                }
                byte* suffixChars = stackalloc byte[suffixLength];
                //lock (this.mParent.mSuffixChars)
                {
                    fixed (byte* sChars = this.mParent.mSuffixChars)
                    {
                        for (i = 0; i < suffixLength; i++)
                        {
                            suffixChars[i] = sChars[i];
                        }
                    }
                }
#else
                int[] index = new int[maxLevel];
                uint[] prevResult = new uint[maxLevel];
                for (i = 0; i < maxLevel; i++)
                {
                    index[i] = 0;
                    prevResult[i] = 0;
                }
                index[0] = minIndex;

                uint prefixHash = FNVHash.TS3Offset32;
                int suffixLength;
                byte[] chars = new byte[lcLength];
                //lock (this.mParent.mLowerChars)
                {
                    byte[] lcChars = this.mParent.mLowerChars;
                    {
                        for (i = 0; i < lcLength; i++)
                        {
                            chars[i] = lcChars[i];
                        }
                    }
                }
                //lock (this.mParent.mPrefixChars)
                {
                    suffixLength = this.mParent.mPrefixLength;
                    byte[] pChars = this.mParent.mPrefixChars;
                    {
                        for (i = 0; i < suffixLength; i++)
                        {
                            prefixHash = (prefixHash * FNVHash.TS3Prime32) ^ pChars[i];
                        }
                    }
                    suffixLength = this.mParent.mSuffixLength;
                }
                byte[] suffixChars = new byte[suffixLength];
                //lock (this.mParent.mSuffixChars)
                {
                    byte[] sChars = this.mParent.mSuffixChars;
                    {
                        for (i = 0; i < suffixLength; i++)
                        {
                            suffixChars[i] = sChars[i];
                        }
                    }
                }
#endif
                for (level = 0; level < maxLevel; level++)
                {
                    // Start at the first level
                    offset = 0;
                    //current = FNVHash.TS3Offset32;
                    current = prefixHash;
                    i = index[offset];
                    stop = maxIndex;
                    product = current * FNVHash.TS3Prime32;

                    while (!this.mFinished)
                    {
                        if (i < stop)
                        {
                            result = product ^ chars[i];
                            tester = result;
                            if (suffixLength > 0)
                            {
                                for (j = 0; j < suffixLength; j++)
                                {
                                    tester = (tester * FNVHash.TS3Prime32) ^ suffixChars[j];
                                }
                            }/**/
                            if (xorFold)
                                tester = ((tester >> 0x18) ^ (tester & 0xffffffU));
                            tester &= filter;
                            if (tester == target)
                            {
                                lock (this.mParent.mMatch)
                                {
                                    bool matchExists = false;
                                    int k, matchCount = this.mParent.mMatchCount;
                                    for (j = 0; j < matchCount && !matchExists; j++)
                                    {
                                        matchExists = true;
                                        for (k = offset - 1; k >= 0 && matchExists; k--)
                                        {
                                            matchExists = matchExists &&
                                                this.mParent.mMatch[j][k] == chars[index[k]];
                                        }
                                    }
                                    if (!matchExists)
                                    {
                                        this.mParent.mEndTimes[matchCount] = DateTime.Now;
                                        this.mParent.mMatch[matchCount][offset] = chars[i];
                                        for (k = offset - 1; k >= 0; k--)
                                        {
                                            this.mParent.mMatch[matchCount][k] = chars[index[k]];
                                        }
                                        this.mParent.mMatchCount++;
                                        if (this.mParent.mMatchCount >= this.mParent.mMaxMatches)
                                        {
                                            //this.mParent.mFinished = true;
                                            this.mFinished = true;
                                            this.mParent.Stop();
                                        }
                                    }
                                }
                            }
                            this.Iterations++;
                            if (offset < level)
                            {
                                // Save the current level index and hash
                                index[offset] = i;
                                prevResult[offset] = current;
                                // Go to the next level
                                offset++;
                                current = result;
                                i = index[offset];
                                stop = (offset == 0) ? maxIndex : lcLength;
                                product = current * FNVHash.TS3Prime32;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        else
                        {
                            // Reset the current level index and hash
                            if (offset < level)
                            {
                                index[offset] = 0;
                                prevResult[offset] = 0;
                            }
                            if (offset > 0)
                            {
                                // Go to the previous level
                                offset--;
                                current = prevResult[offset];
                                i = index[offset] + 1;
                                stop = (offset == 0) ? maxIndex : lcLength;
                                product = current * FNVHash.TS3Prime32;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
