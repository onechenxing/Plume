
namespace Plume.Core
{
    /// <summary>
    /// 赋值节点
    /// </summary>
    class AssignNode : ASTNode , IStatNode
    {
        public AssignNode(NameNode id, IExprNode value) : base(NodeType.Assign)
        {
            AddChild(id);
            AddChild((ASTNode)value);
        }
    }
}
