
namespace Plume.Core
{
    /// <summary>
    /// 减运算
    /// </summary>
    class MinusNode : ASTNode , IExprNode
    {
        public MinusNode(IExprNode left, IExprNode right):base(NodeType.Minus)
        {
            AddChild((ASTNode)left);
            AddChild((ASTNode)right);
        }
    }
}
