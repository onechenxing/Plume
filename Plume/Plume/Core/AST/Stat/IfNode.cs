using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// If判断节点
    /// </summary>
    class IfNode : ASTNode , IStatNode
    {
        public IfNode(List<IExprNode> ifExprs, List<StatListNode> ifBodys,StatListNode elseBody = null) : base(NodeType.If)
        {
            for (int i = 0; i < ifExprs.Count; i++)
            {
                AddChild((ASTNode)ifExprs[i]);
                AddChild(ifBodys[i]);
            }
            AddChild(elseBody);
        }
    }
}
