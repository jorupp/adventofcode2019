using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.GraphSolver
{
    public abstract class Node<TNode, TKey> where TNode : Node<TNode, TKey>
    {
        protected Node()
        {
        }

        public abstract IEnumerable<TNode> GetAdjacent();
        public abstract bool IsValid { get; }
        public abstract bool IsComplete { get; }
        public abstract decimal CurrentCost { get; }
        public abstract decimal EstimatedCost { get; }

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
    }

    public abstract class Node<TNode> : Node<TNode, string> where TNode : Node<TNode>
    {
        public abstract object[] Keys { get; }

        protected override string GetKey() => string.Join("_", this.Keys);
    }
}
