
namespace Plume.Core
{
    /// <summary>
    /// 判断 == != > >= < <=
    /// </summary>
    class CheckNode : ASTNode , IExprNode
    {
        public CheckNode(NodeType type, IExprNode left, IExprNode right):base(type)
        {
            AddChild((ASTNode)left);
            AddChild((ASTNode)right);
        }
    }
}
