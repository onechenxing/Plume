
namespace Plume.Core
{
    /// <summary>
    /// 容器接口
    /// </summary>
    interface IContainer
    {
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(object key, object value);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(object key);
    }
}
