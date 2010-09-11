using System;
using System.IO;
using s3pi.Interfaces;

namespace s3piwrappers.SWB
{
    public abstract class ValueElement<T> : DataElement
        where T: struct
    {
        protected ValueElement(int APIversion, EventHandler handler, ValueElement<T> basis)
            : this(APIversion, handler, basis.mData)
        {
        }

        protected ValueElement(int apiVersion, EventHandler handler) : base(apiVersion, handler)
        {
        }
        protected ValueElement(int apiVersion, EventHandler handler,T data)
            : base(apiVersion, handler)
        {
            mData = data;
        }

        protected ValueElement(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s)
        {
        }

        protected T mData;
        protected abstract override void Parse(Stream s);
        public abstract override void UnParse(Stream s);


        public override AHandlerElement Clone(EventHandler handler)
        {
            return (AHandlerElement)Activator.CreateInstance(GetType(), new object[] { 0, handler, this, mData });
        }
    }
}