using System.Collections.Generic;
using System.Text;

namespace Plume.Core
{
    /// <summary>
    /// 指令列表节点
    /// </summary>
    class StatListNode : ASTNode
    {
        public StatListNode():base(NodeType.Stat_List)
        {
        }

        public void Add(IStatNode node)
        {
            this.AddChild((ASTNode)node);
        }

        /// <summary>
        /// 遍历子节点输出
        /// </summary>
        /// <returns></returns>
        public override string ToStringTree(int level = 0)
        {
            if (children == null || children.Count == 0)
                return this.ToString();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            AddSpace(sb, level);
            sb.Append("(");
            sb.AppendLine(this.ToString());
            for (int i = 0; i < children.Count; i++)
            {
                ASTNode child = children[i];
                AddSpace(sb, level + 1);
                sb.Append(child.ToStringTree(level + 1));
                sb.AppendLine();
            }
            AddSpace(sb, level);
            sb.Append(")");
            sb.AppendLine();
            AddSpace(sb, level - 1);
            return sb.ToString();
        }

        public void AddSpace(StringBuilder sb ,int level)
        {
            for (int l = 0; l < level; l++)
            {
                sb.Append(" ");
            }
        }
    }
}
