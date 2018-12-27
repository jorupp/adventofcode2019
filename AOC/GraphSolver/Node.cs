using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.GraphSolver
{
    public abstract class Node<TNode, TKey, TCost> where TNode : Node<TNode, TKey, TCost> where TCost : IComparable<TCost>
    {
        protected Node()
        {
        }

        public abstract IEnumerable<TNode> GetAdjacent();
        public abstract bool IsValid { get; }
        public abstract bool IsComplete { get; }
        public abstract TCost CurrentCost { get; }
        public abstract TCost EstimatedCost { get; }

        protected abstract TKey GetKey();

        private bool _builtKey = false;
        private TKey _keyValue = default(TKey);
        public TKey Key
        {
            get
            {
                if (!_builtKey)
                {
                    _builtKey = true;
                    _keyValue = GetKey();
                }
                return _keyValue;
            }
        }
        public abstract string Description { get; }

        public override string ToString()
        {
            return Description;
        }
    }

    public abstract class Node<TNode, TKey>: Node<TNode, TKey, decimal> where TNode : Node<TNode, TKey>
    {
        protected Node()
        {
        }
    }

    public abstract class Node<TNode> : Node<TNode, string> where TNode : Node<TNode>
    {
        public abstract object[] Keys { get; }

        protected override string GetKey() => string.Join("_", this.Keys);
    }
}
