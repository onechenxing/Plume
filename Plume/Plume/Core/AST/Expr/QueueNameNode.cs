
using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 连续调用变量节点  name.a.b.c
    /// </summary>
    class QueueNameNode : ASTNode, IExprNode
    {
        public QueueNameNode(List<IExprNode> nameList) : base(NodeType.QueueName)
        {
            foreach (var n in nameList)
            {
                AddChild((ASTNode)n);
            }
        }
    }
}
