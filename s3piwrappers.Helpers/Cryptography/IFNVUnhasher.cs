using System;
using System.Collections.Generic;
using System.Text;

namespace s3piwrappers.Helpers.Cryptography
{
    public interface IFNVUnhasher : IDisposable
    {
        FNVSearchTable SearchTable { get; }

        int PrefixLength { get; }

        int SuffixLength { get; }

        bool XorFold { get; }

        int MaxResultCharCount { get; }

        int MaxResultCount { get; }

        ulong MaxIterations { get; }

        ulong Iterations { get; }

        TimeSpan ElapsedTime { get; }

        bool Started { get; }

        bool Finished { get; }

        int ResultCount { get; }

        string[] Results { get; }

        TimeSpan[] ElapsedTimeAtResults { get; }

        string[] ElapsedTimeAtResultStrings { get; }

        ulong[] IterationsAtResults { get; }

        string[] IterationsAtResultStrings { get; }

        void Start();

        void Stop();
    }
}
