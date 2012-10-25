using System;
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
            var result = new char[length];
            if (length > 0)
                Array.Copy(value, 0, result, 0, length);
            else
                return "";
            return new string(result);
        }

        private readonly FNVSearchTable mSearchTable;
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

        private bool mStarted;
        private bool mFinished;
        private string[] mResults;
        private DateTime mStartTime;
        private DateTime[] mEndTimes;

        public FNVSearchTable SearchTable
        {
            get { return mSearchTable; }
        }

        public uint Filter
        {
            get { return mFilter; }
        }

        public bool XorFold
        {
            get { return mXorFold; }
        }

        public uint TargetHash
        {
            get { return mTargetHash; }
        }

        public int MaxResultLength
        {
            get { return mMaxLevel; }
        }

        public ulong MaxIterations
        {
            get { return mMaxIterations; }
        }

        public ulong Iterations
        {
            get
            {
                if (mMatchers == null)
                    return 0;
                ulong iterations = 0;
                for (int i = 0; i < mMatchers.Length; i++)
                {
                    iterations += mMatchers[i].Iterations;
                }
                return iterations;
            }
        }

        public bool Finished
        {
            get { return mFinished; }
        }

        public int ResultCount
        {
            get { return mMatchCount; }
        }

        public string[] Results
        {
            get { return GetResults(); }
        }

        public TimeSpan[] ElapsedTimes
        {
            get
            {
                if (mEndTimes == null)
                    return null;
                var elapsed = new TimeSpan[mMatchCount];
                for (int i = 0; i < mMatchCount; i++)
                {
                    elapsed[i] = mEndTimes[i] - mStartTime;
                }
                return elapsed;
            }
        }

        public string[] ElapsedTimeStrings
        {
            get
            {
                if (mEndTimes == null)
                    return null;
                var elapsed = new string[mMatchCount];
                for (int i = 0; i < mMatchCount; i++)
                {
                    elapsed[i] = (mEndTimes[i] - mStartTime).ToString();
                }
                return elapsed;
            }
        }

        private string[] GetResults()
        {
            if (mResults == null)
                return null;
            string prefixStr = BytesToString(mPrefixChars, mPrefixLength);
            string suffixStr = BytesToString(mSuffixChars, mSuffixLength);
            for (int i = mMatchCount - 1; i >= 0; i--)
            {
                if (mResults[i] == null)
                {
                    mResults[i] = prefixStr;
                    for (int j = 0; j < mMaxLevel; j++)
                    {
                        if (mMatch[i][j] == kNotFoundChar)
                            break;
                        mResults[i] += (char) mMatch[i][j];
                    }
                    mResults[i] += suffixStr;
                }
            }
            return mResults;
        }

        public FNVUnhasher32(uint hash, FNVSearchTable searchTable,
                             int maxChars = 10, int maxMatches = 5,
                             bool xorFold = false, uint filter = uint.MaxValue)
        {
            mSearchTable = searchTable;
            mTargetHash = hash;
            Reset(maxChars, maxMatches, xorFold, filter);
        }

        public void Reset(int maxChars = 10, int maxMatches = 5,
                          bool xorFold = false, uint filter = uint.MaxValue)
        {
            // Only reset if the it hasn't started yet 
            // or it's been started and has finished
            if (mStarted && !mFinished) // !(!this.mStarted || this.mFinished)
                return;
            mStarted = false;
            mFinished = false;

            // Copied from the search table in case the user modifies it
            // while the unhasher is running.
            mLowerCharsLength = mSearchTable.Count;
            mLowerChars = mSearchTable.Table;
            mPrefixChars = mSearchTable.PrefixBytes;
            mPrefixLength = mPrefixChars.Length;
            mSuffixChars = mSearchTable.SuffixBytes;
            mSuffixLength = mSuffixChars.Length;

            mMaxLevel = maxChars;
            mMaxMatches = maxMatches;
            mFilter = filter;
            mXorFold = xorFold;

            // Fail-safe in case another thread finds a match
            // between the time the final match is found
            // and the unhasher stops all threads.
            // Technically this should never occur since
            // the parent stops all threads within a locked 
            // block of code, but just in case.
            maxMatches += Environment.ProcessorCount << 1;
            mMatch = new byte[maxMatches][];
            mResults = new string[maxMatches];
            mEndTimes = new DateTime[maxMatches];
            int i, j;
            for (i = 0; i < maxMatches; i++)
            {
                mResults[i] = null;
                mMatch[i] = new byte[maxChars];
                for (j = 0; j < maxChars; j++)
                {
                    mMatch[i][j] = kNotFoundChar;
                }
            }
            mMaxIterations = 1;
            for (j = 0; j < maxChars; j++)
            {
                mMaxIterations *= (ulong) mLowerCharsLength;
            }
            InitMatchers();
        }

        private void InitMatchers()
        {
            int i, step, threadCount = Environment.ProcessorCount << 1;
            step = mLowerCharsLength/threadCount;
            mMatchers = new Matcher[threadCount];
            mMatchers[0] = new Matcher(this, 0, step);
            threadCount--;
            for (i = 1; i < threadCount; i++)
            {
                mMatchers[i] = new Matcher(this, i*step, (i + 1)*step);
            }
            mMatchers[threadCount] = new Matcher(this, i*step, mLowerCharsLength);
        }

        ~FNVUnhasher32()
        {
            Stop();
        }

        public void Dispose()
        {
            Stop();
        }

        public void Stop()
        {
            if (mStarted && !mFinished)
            {
                mFinished = true;
                for (int i = 0; i < mMatchers.Length; i++)
                {
                    mMatchers[i].StopThread();
                }
            }
        }

        public void Start()
        {
            if (!mStarted)
            {
                mStartTime = DateTime.Now;
                int i;
                for (i = 0; i < mMaxMatches; i++)
                {
                    mEndTimes[i] = mStartTime;
                }
                for (i = 0; i < mMatchers.Length; i++)
                {
                    mMatchers[i].InitThread();
                }
                for (i = 0; i < mMatchers.Length; i++)
                {
                    mMatchers[i].Thread.Start();
                }
                mStarted = true;
            }
        }

        private class Matcher
        {
            public Thread Thread;
            private readonly FNVUnhasher32 mParent;
            private readonly int mMinIndex;
            private readonly int mMaxIndex;

            public ulong Iterations;
            private bool mFinished;

            public Matcher(FNVUnhasher32 parent, int minIndex, int maxIndex)
            {
                mParent = parent;
                mMinIndex = minIndex;
                mMaxIndex = maxIndex;
            }

            public void InitThread()
            {
                Thread = new Thread(MatchFinder);
                Thread.Priority = ThreadPriority.BelowNormal;
                Thread.Name = string.Format("0x{0:X8}:({1}-{2})", mParent.mTargetHash,
                                            (char) mParent.mLowerChars[mMinIndex],
                                            (char) mParent.mLowerChars[mMaxIndex - 1]);
            }

            public void StopThread()
            {
                mFinished = true;
                if (Thread != null)
                {
                    Thread.Join();
                    Thread = null;
                }
            }

#if UNSAFE
            unsafe
#endif

            private void MatchFinder()
            {
                bool xorFold = mParent.mXorFold;
                int i, j, stop, offset, lcLength = mParent.mLowerCharsLength;
                int level, maxLevel = mParent.mMaxLevel;
                int minIndex = mMinIndex;
                int maxIndex = mMaxIndex;
                uint result, product, current, target, tester, filter = mParent.mFilter;
                target = mParent.mTargetHash & filter;
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
                var index = new int[maxLevel];
                var prevResult = new uint[maxLevel];
                for (i = 0; i < maxLevel; i++)
                {
                    index[i] = 0;
                    prevResult[i] = 0;
                }
                index[0] = minIndex;

                uint prefixHash = FNVHash.TS3Offset32;
                int suffixLength;
                var chars = new byte[lcLength];
                //lock (this.mParent.mLowerChars)
                {
                    byte[] lcChars = mParent.mLowerChars;
                    {
                        for (i = 0; i < lcLength; i++)
                        {
                            chars[i] = lcChars[i];
                        }
                    }
                }
                //lock (this.mParent.mPrefixChars)
                {
                    suffixLength = mParent.mPrefixLength;
                    byte[] pChars = mParent.mPrefixChars;
                    {
                        for (i = 0; i < suffixLength; i++)
                        {
                            prefixHash = (prefixHash*FNVHash.TS3Prime32) ^ pChars[i];
                        }
                    }
                    suffixLength = mParent.mSuffixLength;
                }
                var suffixChars = new byte[suffixLength];
                //lock (this.mParent.mSuffixChars)
                {
                    byte[] sChars = mParent.mSuffixChars;
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
                    product = current*FNVHash.TS3Prime32;

                    while (!mFinished)
                    {
                        if (i < stop)
                        {
                            result = product ^ chars[i];
                            tester = result;
                            if (suffixLength > 0)
                            {
                                for (j = 0; j < suffixLength; j++)
                                {
                                    tester = (tester*FNVHash.TS3Prime32) ^ suffixChars[j];
                                }
                            } /**/
                            if (xorFold)
                                tester = ((tester >> 0x18) ^ (tester & 0xffffffU));
                            tester &= filter;
                            if (tester == target)
                            {
                                lock (mParent.mMatch)
                                {
                                    bool matchExists = false;
                                    int k, matchCount = mParent.mMatchCount;
                                    for (j = 0; j < matchCount && !matchExists; j++)
                                    {
                                        matchExists = true;
                                        for (k = offset - 1; k >= 0 && matchExists; k--)
                                        {
                                            matchExists = matchExists &&
                                                mParent.mMatch[j][k] == chars[index[k]];
                                        }
                                    }
                                    if (!matchExists)
                                    {
                                        mParent.mEndTimes[matchCount] = DateTime.Now;
                                        mParent.mMatch[matchCount][offset] = chars[i];
                                        for (k = offset - 1; k >= 0; k--)
                                        {
                                            mParent.mMatch[matchCount][k] = chars[index[k]];
                                        }
                                        mParent.mMatchCount++;
                                        if (mParent.mMatchCount >= mParent.mMaxMatches)
                                        {
                                            //this.mParent.mFinished = true;
                                            mFinished = true;
                                            mParent.Stop();
                                        }
                                    }
                                }
                            }
                            Iterations++;
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
                                product = current*FNVHash.TS3Prime32;
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
                                product = current*FNVHash.TS3Prime32;
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
