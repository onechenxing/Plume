
using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 表节点
    /// </summary>
    class TableNode : ASTNode , IExprNode
    {
        public TableNode():base(NodeType.Table)
        {

        }

        public void Add(IExprNode node)
        {
            this.AddChild((ASTNode)node);
        }
    }
}
