using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s3piwrappers.JazzGraph
{
    public abstract class MulticastDecisionGraphNode : DecisionGraphNode
    {
        private List<DecisionGraphNode> mTargets;

        public MulticastDecisionGraphNode(uint nodeType, string nodeTag)
            : base(nodeType, nodeTag)
        {
            this.mTargets = new List<DecisionGraphNode>();
        }

        public void AddTarget(DecisionGraphNode target)
        {
            if (!this.mTargets.Contains(target))
            {
                this.mTargets.Add(target);
            }
        }

        public bool RemoveTarget(DecisionGraphNode target)
        {
            int index = this.mTargets.IndexOf(target);
            if (index < 0)
            {
                return false;
            }
            this.mTargets.RemoveAt(index);
            return true;
        }

        public void ClearTargets()
        {
            this.mTargets.Clear();
        }

        public int TargetCount
        {
            get { return this.mTargets.Count; }
        }

        public DecisionGraphNode[] Targets
        {
            get { return this.mTargets.ToArray(); }
        }
    }
}
