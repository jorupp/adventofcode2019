namespace AoC.GraphSolver
{
    public interface ISolver
    {
        TNode Evaluate<TNode, TKey>(TNode start, TKey key) where TNode : Node<TNode, TKey>;
    }
}