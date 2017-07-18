
namespace Plume.Core
{
    /// <summary>
    /// 条件 与 and
    /// </summary>
    class AndNode : ASTNode , IExprNode
    {
        public AndNode(IExprNode left, IExprNode right):base(NodeType.And)
        {
            AddChild((ASTNode)left);
            AddChild((ASTNode)right);
        }
    }
}
