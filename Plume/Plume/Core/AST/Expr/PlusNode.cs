
namespace Plume.Core
{
    /// <summary>
    /// 加运算
    /// </summary>
    class PlusNode : ASTNode , IExprNode
    {
        public PlusNode(IExprNode left, IExprNode right):base(NodeType.Plus)
        {
            AddChild((ASTNode)left);
            AddChild((ASTNode)right);
        }
    }
}
