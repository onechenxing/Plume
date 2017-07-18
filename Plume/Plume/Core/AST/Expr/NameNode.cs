
namespace Plume.Core
{
    /// <summary>
    /// 变量节点
    /// </summary>
    class NameNode : ASTNode , IExprNode
    {
        public NameNode(string name) : base(NodeType.Name)
        {
            this.data = name;
        }
    }
}
