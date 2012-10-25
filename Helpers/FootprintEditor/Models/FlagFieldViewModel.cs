using System;

namespace s3piwrappers.Models
{
    public abstract class FlagFieldViewModel<T> : AbstractViewModel
        where T : struct, IConvertible
    {
        protected bool Get<T>(T flag)
        {
            return (Convert.ToUInt32(Actual) & Convert.ToUInt32(flag)) == Convert.ToUInt32(flag);
        }

        protected void Set<T>(T flag, bool val)
        {
            Actual = ((Convert.ToUInt32(Actual)) | (Convert.ToUInt32(flag)));
            if (!val)
                Actual = ((Convert.ToUInt32(Actual)) ^ (Convert.ToUInt32(flag)));
        }

        protected abstract uint Actual { get; set; }
    }
}
