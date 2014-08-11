using System;
using System.Collections;

namespace s3piwrappers.Helpers.Collections
{
    public interface IHasReverse
    {
        void Reverse();

        void Reverse(int index, int count);
    }
}
