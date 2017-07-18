
namespace Plume.Core
{
    /// <summary>
    /// 除运算
    /// </summary>
    class DivideNode : ASTNode , IExprNode
    {
        public DivideNode(IExprNode left,IExprNode right):base(NodeType.Divide)
        {
            AddChild((ASTNode)left);
            AddChild((ASTNode)right);
        }
    }
}
