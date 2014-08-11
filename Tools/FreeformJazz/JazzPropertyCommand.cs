using System;
using System.Collections.Generic;
using System.Reflection;
using s3piwrappers.Helpers;

namespace s3piwrappers.FreeformJazz
{
    public class JazzPropertyCommand<T, P> : JazzCommand
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

        public JazzPropertyCommand(JazzGraphContainer container, 
            T thing, string property, P newValue, bool extendable)
            : base(container)
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
            JazzPropertyCommand<T, P> jpc 
                = possibleExt as JazzPropertyCommand<T, P>;
            if (jpc == null || jpc.mContainer != this.mContainer ||
                !sTEC.Equals(jpc.mThing, this.mThing) ||
                !string.Equals(jpc.mPropName, this.mPropName) ||
                sPEC.Equals(jpc.mNewVal, this.mOldVal))
            {
                return false;
            }
            return true;
        }

        public override void Extend(Command possibleExt)
        {
            JazzPropertyCommand<T, P> jpc
                = possibleExt as JazzPropertyCommand<T, P>;
            this.mNewVal = jpc.mNewVal;
        }
    }
}
