using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 加载代码表达式节点
    /// </summary>
    class LoadExprNode : ASTNode , IExprNode
    {
        public LoadExprNode(StringNode path) : base(NodeType.Load_Expr)
        {
            AddChild(path);
        }
    }
}
