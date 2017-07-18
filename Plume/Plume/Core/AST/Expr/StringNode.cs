
namespace Plume.Core
{
    /// <summary>
    /// 字符串节点
    /// </summary>
    class StringNode : ASTNode , IExprNode
    {
        public StringNode(string value) : base(NodeType.String)
        {
            this.data = value;
        }
    }
}
