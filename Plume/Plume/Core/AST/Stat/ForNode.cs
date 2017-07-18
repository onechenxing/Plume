using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// For In 循环节点
    /// </summary>
    class ForNode : ASTNode , IStatNode
    {
        public ForNode(NameNode paramId,IExprNode containerId,StatListNode body) : base(NodeType.For)
        {
            AddChild(paramId);
            AddChild((ASTNode)containerId);
            AddChild(body);
        }
    }
}
