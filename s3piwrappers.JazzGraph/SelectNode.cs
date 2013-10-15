using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s3piwrappers.JazzGraph
{
    public abstract class SelectNode<CaseType> : DecisionGraphNode
    {
        public class Case
        {
            public readonly CaseType Value;
            public readonly DecisionGraphNode[] Targets;

            public Case(CaseType value, DecisionGraphNode[] targets)
            {
                this.Value = value;
                this.Targets = targets;
            }
        }

        private class CaseImpl
        {
            public CaseType Value;
            public List<DecisionGraphNode> Targets;

            public CaseImpl(CaseType value)
            {
                this.Value = value;
                this.Targets = new List<DecisionGraphNode>();
            }
        }

        private List<CaseImpl> mCases;

        public SelectNode(uint nodeType, string nodeTag)
            : base(nodeType, nodeTag)
        {
            this.mCases = new List<CaseImpl>();
        }

        public void AddCaseTarget(
            CaseType caseValue, DecisionGraphNode target)
        {
            CaseImpl ci = null;
            if (this.mCases.Count == 0)
            {
                ci = new CaseImpl(caseValue);
                ci.Targets.Add(target);
                this.mCases.Add(ci);
            }
            else
            {
                int i;
                EqualityComparer<CaseType> ec 
                    = EqualityComparer<CaseType>.Default;
                for (i = this.mCases.Count - 1; i >= 0; i--)
                {
                    ci = this.mCases[i];
                    if (ec.Equals(caseValue, ci.Value))
                    {
                        break;
                    }
                }
                if (i < 0)
                {
                    ci = new CaseImpl(caseValue);
                    this.mCases.Add(ci);
                }
                if (!ci.Targets.Contains(target))
                {
                    ci.Targets.Add(target);
                }
            }
        }

        public bool RemoveCaseTarget(
            CaseType caseValue, DecisionGraphNode target)
        {
            if (this.mCases.Count == 0)
            {
                return false;
            }
            int i;
            CaseImpl ci = null;
            EqualityComparer<CaseType> ec
                = EqualityComparer<CaseType>.Default;
            for (i = this.mCases.Count - 1; i >= 0; i--)
            {
                ci = this.mCases[i];
                if (ec.Equals(caseValue, ci.Value))
                {
                    break;
                }
            }
            if (i < 0)
            {
                return false;
            }
            int index = ci.Targets.IndexOf(target);
            if (index < 0)
            {
                return false;
            }
            ci.Targets.RemoveAt(index);
            return true;
        }

        public int CaseCount
        {
            get { return this.mCases.Count; }
        }

        public Case[] Cases
        {
            get
            {
                CaseImpl ci;
                Case[] cases = new Case[this.mCases.Count];
                for (int i = this.mCases.Count - 1; i >= 0; i--)
                {
                    ci = this.mCases[i];
                    cases[i] = new Case(ci.Value, ci.Targets.ToArray());
                }
                return cases;
            }
        }
    }
}
