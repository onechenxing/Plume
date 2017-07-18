using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 等待语句节点
    /// </summary>
    class WaitNode : ASTNode , IStatNode
    {
        public WaitNode(IExprNode value) : base(NodeType.Wait)
        {
            AddChild((ASTNode)value);
        }
    }
}
