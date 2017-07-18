
namespace Plume.Core
{
    /// <summary>
    /// 默认值设置表达式 t=1
    /// </summary>
    class InnerAssignNode : ASTNode , IExprNode
    {
        public InnerAssignNode(NameNode key,IExprNode value) : base(NodeType.InnerAssign)
        {
            this.AddChild(key);
            this.AddChild((ASTNode)value);
        }
    }
}
