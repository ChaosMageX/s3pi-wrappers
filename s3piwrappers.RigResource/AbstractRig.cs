using System;
using System.IO;
using s3pi.Interfaces;
using System.Collections.Generic;
using s3pi.Settings;

namespace s3piwrappers
{
    [DataGridExpandable(true)]
    public abstract class AbstractRig : AHandlerElement
    {
        public static AbstractRig CreateRig(RigType type,params object[] args)
        {
            Type t = null;
            switch(type)
            {
                case RigType.Object:
                    t = typeof (ObjectRig);
                    break;
                case RigType.Body:
                    t = typeof (BodyRig);
                    break;
                default:
                    throw new Exception(String.Format("RigType: {0} is not supported.",type));
            }
            return (AbstractRig) Activator.CreateInstance(t, args);
        }
        protected static bool checking = Settings.Checking;
        public AbstractRig(int APIversion, EventHandler handler) 
            : base(APIversion, handler)
        {
            mGrannyData = GrannyData.CreateInstance(0, handler);
        }
        public AbstractRig(int APIversion, EventHandler handler,GrannyData grannyData)
            : base(APIversion, handler)
        {
            mGrannyData = grannyData;
        }
        public AbstractRig(int APIversion, EventHandler handler,Stream s)
            : base(APIversion, handler)
        {
            Parse(s);
        }

        protected abstract void Parse(Stream s);
        public abstract void UnParse(Stream s);
        protected GrannyData mGrannyData;
        [ElementPriority(0)]
        public GrannyData GrannyData
        {
            get { return mGrannyData; }
            set { if(mGrannyData!=value){mGrannyData = value; OnElementChanged();} }
        }
        public virtual string Value
        {
            get { return mGrannyData.Value; }
        }

        public abstract override AHandlerElement Clone(EventHandler handler);

        public override List<string> ContentFields
        {
            get { return GetContentFields(requestedApiVersion,GetType()); }
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
    }
}