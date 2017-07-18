
using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 返回节点
    /// </summary>
    class ReturnNode : ASTNode , IStatNode
    {
        public ReturnNode(IExprNode value = null) : base(NodeType.Return)
        {
            if (value != null)
            {
                AddChild((ASTNode)value);
            }
        }
    }
}
