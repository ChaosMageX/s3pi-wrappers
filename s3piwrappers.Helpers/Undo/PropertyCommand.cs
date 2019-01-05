using System;
using System.Collections.Generic;
using System.Reflection;

namespace s3piwrappers.Helpers.Undo
{
    /// <summary><para>
    /// This command changes the value of a property of type <typeparamref name="P"/> 
    /// in an object of type <typeparamref name="T"/>.
    /// </para><para>
    /// For example, if there's a Table class with an "IsRound" boolean property, 
    /// <typeparamref name="T"/> would be "Table" and 
    /// <typeparamref name="P"/> would be "bool" for this command to change that property. 
    /// </para></summary>
    /// <typeparam name="T">The object type that contains the given Property to be changed.</typeparam>
    /// <typeparam name="P">The type for the value stored in the given Property to be changed.</typeparam>
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
            this.mProperty.SetValue(this.mThing, this.mNewVal, null);
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
