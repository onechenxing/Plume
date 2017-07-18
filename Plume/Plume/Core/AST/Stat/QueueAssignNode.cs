
namespace Plume.Core
{
    /// <summary>
    /// 连续变量赋值节点 x.a.b.c = ..
    /// </summary>
    class QueueAssignNode : ASTNode , IStatNode
    {
        public QueueAssignNode(QueueNameNode id, IExprNode value) : base(NodeType.QueueAssign)
        {
            AddChild(id);
            AddChild((ASTNode)value);
        }
    }
}
