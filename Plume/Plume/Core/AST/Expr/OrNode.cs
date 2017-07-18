
namespace Plume.Core
{
    /// <summary>
    /// 条件 或 or
    /// </summary>
    class OrNode : ASTNode , IExprNode
    {
        public OrNode(IExprNode left, IExprNode right):base(NodeType.Or)
        {
            AddChild((ASTNode)left);
            AddChild((ASTNode)right);
        }
    }
}
