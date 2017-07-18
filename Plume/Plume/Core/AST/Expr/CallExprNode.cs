using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 调用函数并获取返回的表达式节点
    /// </summary>
    class CallExprNode : ASTNode , IExprNode
    {
        public CallExprNode(QueueNameNode id, ExprListNode value) : base(NodeType.Call_Expr)
        {
            AddChild(id);
            AddChild(value);
        }
    }
}
