using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 单独调用函数的语句节点
    /// </summary>
    class CallStatNode : ASTNode , IStatNode
    {
        public CallStatNode(QueueNameNode id, ExprListNode value) : base(NodeType.Call_Stat)
        {
            AddChild(id);
            AddChild(value);
        }
    }
}
