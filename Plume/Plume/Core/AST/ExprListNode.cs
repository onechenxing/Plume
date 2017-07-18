using System.Collections.Generic;
namespace Plume.Core
{
    /// <summary>
    /// 表达式列表节点
    /// </summary>
    class ExprListNode : ASTNode
    {
        public ExprListNode():base(NodeType.Expr_List)
        {

        }

        public void Add(IExprNode node)
        {
            this.AddChild((ASTNode)node);
        }
    }
}
