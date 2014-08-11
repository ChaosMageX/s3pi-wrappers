using System;
using System.Threading;

namespace s3piwrappers.Helpers.Cryptography
{
    public class FNVUnhasher64 : IFNVUnhasher
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

        private readonly FNVSearchTable mSearchTable;
        private int mLowerCharsLength;
        private byte[] mLowerChars;

        private int mPrefixLength;
        private ulong mPrefixHash;
        private string mPrefixStr;
        
        private int mSuffixLength;
        private byte[] mSuffixChars;
        private string mSuffixStr;

        private ulong mFilter;
        private bool mXorFold;
        private readonly ulong mTargetHash;
        private int mMaxLevel;
        private int mMaxMatches;
        private ulong mMaxIterations;

        private int mMatchCount;
        private byte[][] mMatch;
        private Matcher[] mMatchers;

        private bool bStarted;
        private bool bFinished;
        private string[] mResults;
        private DateTime mStartTime;
        private DateTime[] mEndTimes;
        private ulong[] mItersArray;

        public FNVSearchTable SearchTable
        {
            get { return mSearchTable; }
        }

        public int PrefixLength
        {
            get { return mPrefixLength; }
        }

        public int SuffixLength
        {
            get { return mSuffixLength; }
        }

        public ulong Filter
        {
            get { return mFilter; }
        }

        public bool XorFold
        {
            get { return mXorFold; }
        }

        public ulong TargetHash
        {
            get { return mTargetHash; }
        }

        public int MaxResultCharCount
        {
            get { return mMaxLevel; }
        }

        public int MaxResultCount
        {
            get { return mMaxMatches; }
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
                for (int i = mMatchers.Length - 1; i >= 0; i--)
                {
                    iterations += mMatchers[i].Iterations;
                }
                return iterations;
            }
        }

        public bool Started
        {
            get { return bStarted; }
        }

        public bool Finished
        {
            get { return bFinished; }
        }

        public TimeSpan ElapsedTime
        {
            get { return this.bStarted ? DateTime.Now - this.mStartTime : new TimeSpan(); }
        }

        public int ResultCount
        {
            get { return mMatchCount; }
        }

        public string[] Results
        {
            get { return GetResults(); }
        }

        public TimeSpan[] ElapsedTimeAtResults
        {
            get
            {
                if (mEndTimes == null)
                    return null;
                TimeSpan[] elapsed = new TimeSpan[mMatchCount];
                for (int i = 0; i < mMatchCount; i++)
                {
                    elapsed[i] = mEndTimes[i] - mStartTime;
                }
                return elapsed;
            }
        }

        public string[] ElapsedTimeAtResultStrings
        {
            get
            {
                if (mEndTimes == null)
                    return null;
                string[] elapsed = new string[mMatchCount];
                for (int i = 0; i < mMatchCount; i++)
                {
                    elapsed[i] = (mEndTimes[i] - mStartTime).ToString();
                }
                return elapsed;
            }
        }

        public ulong[] IterationsAtResults
        {
            get
            {
                if (mItersArray == null)
                    return null;
                ulong[] iters = new ulong[mMatchCount];
                for (int i = 0; i < mMatchCount; i++)
                {
                    iters[i] = mItersArray[i];
                }
                return iters;
            }
        }

        public string[] IterationsAtResultStrings
        {
            get
            {
                if (mItersArray == null)
                    return null;
                string[] iters = new string[mMatchCount];
                for (int i = 0; i < mMatchCount; i++)
                {
                    iters[i] = mItersArray[i].ToString("##,#");
                }
                return iters;
            }
        }

        private string[] GetResults()
        {
            if (mResults == null)
                return null;
            //string prefixStr = BytesToString(mPrefixChars, mPrefixLength);
            //string suffixStr = BytesToString(mSuffixChars, mSuffixLength);
            for (int i = mMatchCount - 1; i >= 0; i--)
            {
                if (mResults[i] == null)
                {
                    mResults[i] = mPrefixStr;
                    for (int j = 0; j < mMaxLevel; j++)
                    {
                        if (mMatch[i][j] == kNotFoundChar)
                            break;
                        mResults[i] += (char) mMatch[i][j];
                    }
                    mResults[i] += mSuffixStr;
                }
            }
            return mResults;
        }

        public FNVUnhasher64(ulong hash, FNVSearchTable searchTable,
                             int maxChars = 10, int maxMatches = 5,
                             bool xorFold = false, ulong filter = ulong.MaxValue)
        {
            mSearchTable = searchTable;
            mTargetHash = hash;
            Reset(maxChars, maxMatches, xorFold, filter);
        }

        public void Reset(int maxChars = 10, int maxMatches = 5,
                          bool xorFold = false, ulong filter = ulong.MaxValue)
        {
            int i, j;
            // Only reset if the it hasn't started yet 
            // or it's been started and has finished
            if (bStarted && !bFinished) // !(!this.mStarted || this.mFinished)
                return;
            bStarted = false;
            bFinished = false;

            // Copied from the search table in case the user modifies it
            // while the unhasher is running.
            mLowerCharsLength = mSearchTable.Count;
            mLowerChars = mSearchTable.Table;

            mPrefixStr = mSearchTable.Prefix; 
            byte[] pChars = mSearchTable.PrefixBytes;
            mPrefixLength = pChars.Length;
            mPrefixHash = FNVHash.TS3Offset64;
            for (i = 0; i < mPrefixLength; i++)
            {
                mPrefixHash = (mPrefixHash * FNVHash.TS3Prime32) ^ pChars[i];
            }

            mSuffixStr = mSearchTable.Suffix;
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
            mItersArray = new ulong[maxMatches];
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
                mMatchers[i] = new Matcher(this, i * step, (i + 1) * step);
            }
            mMatchers[threadCount] = new Matcher(this, i * step, mLowerCharsLength);
        }

        ~FNVUnhasher64()
        {
            Stop();
        }

        public void Dispose()
        {
            Stop();
        }

        public void Stop()
        {
            if (bStarted && !bFinished)
            {
                int i;
                bFinished = true;
                for (i = mMatchers.Length - 1; i >= 0; i--)
                {
                    mMatchers[i].StopThread();
                }
            }
        }

        public void Start()
        {
            if (!bStarted)
            {
                mStartTime = DateTime.Now;
                int i;
                for (i = 0; i < mMaxMatches; i++)
                {
                    mEndTimes[i] = mStartTime;
                }
                for (i = mMatchers.Length - 1; i >= 0; i--)
                {
                    mMatchers[i].InitThread();
                }
                for (i = mMatchers.Length - 1; i >= 0; i--)
                {
                    mMatchers[i].Thread.Start();
                }
                bStarted = true;
            }
        }

        private class Matcher
        {
            public Thread Thread;
            private readonly FNVUnhasher64 mParent;
            private readonly int mMinIndex;
            private readonly int mMaxIndex;

            private readonly int mMaxLevel;
            private readonly ulong mPrefixHash;

            public ulong Iterations;
            private bool bFinished;

            public Matcher(FNVUnhasher64 parent, int minIndex, int maxIndex)
            {
                mParent = parent;
                mMinIndex = minIndex;
                mMaxIndex = maxIndex;

                mMaxLevel = parent.mMaxLevel;
                mPrefixHash = parent.mPrefixHash;
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
                bFinished = true;
                /*if (Thread != null)
                {
                    Thread.Join();
                    Thread = null;
                }/* */
            }

#if UNSAFE
            unsafe
#endif

            private void MatchFinder()
            {
                bool xorFold = this.mParent.mXorFold;
                int i, j, stop, offset, level;
                int lcLength = this.mParent.mLowerCharsLength;
                int maxLevel = this.mMaxLevel;
                int minIndex = this.mMinIndex;
                int maxIndex = this.mMaxIndex;
                ulong result, product, current, tester;
                ulong filter = this.mParent.mFilter;
                ulong target = this.mParent.mTargetHash & filter;
                //bool finished = false;
                ulong prefixHash = this.mPrefixHash;
                int suffixLength = this.mParent.mSuffixLength;
#if UNSAFE
                int* index = stackalloc int[maxLevel];
                ulong* prevResult = stackalloc ulong[maxLevel];

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
                ulong[] prevResult = new ulong[maxLevel];

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
                for (i = 0; i < maxLevel; i++)
                {
                    index[i] = 0;
                    prevResult[i] = 0;
                }
                index[0] = minIndex;

                for (level = 0; level < maxLevel; level++)
                {
                    // Start at the first level
                    offset = 0;
                    //current = FNVHash.TS3Offset64;
                    current = prefixHash;
                    i = index[offset];
                    stop = maxIndex;
                    product = current * FNVHash.TS3Prime64;

                    while (!this.bFinished)
                    {
                        if (i < stop)
                        {
                            result = product ^ chars[i];
                            tester = result;
                            if (suffixLength > 0)
                            {
                                for (j = 0; j < suffixLength; j++)
                                {
                                    tester = (tester * FNVHash.TS3Prime64) ^ suffixChars[j];
                                }
                            } /**/
                            if (xorFold)
                                tester = ((tester >> 0x30) ^ (tester & 0xffffffffffffUL));
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
                                        this.mParent.mItersArray[matchCount] = this.mParent.Iterations;
                                        this.mParent.mMatch[matchCount][offset] = chars[i];
                                        for (k = offset - 1; k >= 0; k--)
                                        {
                                            this.mParent.mMatch[matchCount][k] = chars[index[k]];
                                        }
                                        this.mParent.mMatchCount++;
                                        if (this.mParent.mMatchCount >= this.mParent.mMaxMatches)
                                        {
                                            //this.mParent.mFinished = true;
                                            this.bFinished = true;
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
                                product = current * FNVHash.TS3Prime64;
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
                                product = current * FNVHash.TS3Prime64;
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
