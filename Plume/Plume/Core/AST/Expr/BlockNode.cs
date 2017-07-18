
using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 代码块节点
    /// </summary>
    class BlockNode : ASTNode , IExprNode
    {
        public BlockNode( ExprListNode param, StatListNode body) : base(NodeType.Block)
        {
            AddChild(param);
            AddChild(body);
        }
    }
}
