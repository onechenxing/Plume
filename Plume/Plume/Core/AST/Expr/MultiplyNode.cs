
namespace Plume.Core
{
    /// <summary>
    /// 乘运算
    /// </summary>
    class MultiplyNode : ASTNode , IExprNode
    {
        public MultiplyNode(IExprNode left,IExprNode right):base(NodeType.Multiply)
        {
            AddChild((ASTNode)left);
            AddChild((ASTNode)right);
        }
    }
}
