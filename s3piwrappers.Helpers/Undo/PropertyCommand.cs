using System;
using System.Collections.Generic;
using System.Reflection;

namespace s3piwrappers.Helpers.Undo
{
    public class PropertyCommand<T, P> : Command
    {
        private static readonly EqualityComparer<T> sTEC 
            = EqualityComparer<T>.Default;
        private static readonly EqualityComparer<P> sPEC 
            = EqualityComparer<P>.Default;

        private T mThing;
        private string mPropName;
        private PropertyInfo mProperty;
        protected P mOldVal;
        protected P mNewVal;
        private bool bExtendable;

        public PropertyCommand(T thing, string property, P newValue, 
            bool extendable)
        {
            this.mThing = thing;
            this.mPropName = property;
            this.mProperty = typeof(T).GetProperty(property);
            if (this.mProperty == null)
            {
                throw new ArgumentException(typeof(T).Name + "." + property +
                    " does not exist or is inaccessible.", "property");
            }
            object oldVal = this.mProperty.GetValue(this.mThing, null);
            if (typeof(P).IsPrimitive)
            {
                this.mOldVal = (P)Convert.ChangeType(oldVal, typeof(P));
            }
            else
            {
                this.mOldVal = (P)oldVal;
            }
            this.mNewVal = newValue;
            this.bExtendable = extendable;
        }

        public override bool Execute()
        {
            object newVal = this.mNewVal;
            if (this.mProperty.PropertyType.IsPrimitive)
            {
                newVal = Convert.ChangeType(this.mNewVal, 
                    this.mProperty.PropertyType);
            }
            this.mProperty.SetValue(this.mThing, newVal, null);
            return true;
        }

        public override void Undo()
        {
            this.mProperty.SetValue(this.mThing, this.mOldVal, null);
        }

        public override bool IsExtendable(Command possibleExt)
        {
            if (!this.bExtendable)
            {
                return false;
            }
            PropertyCommand<T, P> pc 
                = possibleExt as PropertyCommand<T, P>;
            if (pc == null ||
                !sTEC.Equals(pc.mThing, this.mThing) ||
                !string.Equals(pc.mPropName, this.mPropName) ||
                sPEC.Equals(pc.mNewVal, this.mOldVal))
            {
                return false;
            }
            return true;
        }

        public override void Extend(Command possibleExt)
        {
            PropertyCommand<T, P> pc
                = possibleExt as PropertyCommand<T, P>;
            this.mNewVal = pc.mNewVal;
        }
    }
}
