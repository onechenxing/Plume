
namespace Plume.Core
{
    /// <summary>
    /// 数值节点
    /// </summary>
    class NumberNode : ASTNode , IExprNode
    {
        public NumberNode(string value) : base(NodeType.Number)
        {
            this.data = value;
        }
    }
}
