using System.Collections.Generic;

namespace Plume.Core
{
    /// <summary>
    /// 加载代码运行节点
    /// </summary>
    class LoadStatNode : ASTNode , IStatNode
    {
        public LoadStatNode(StringNode path) : base(NodeType.Load_Stat)
        {
            AddChild(path);
        }
    }
}
