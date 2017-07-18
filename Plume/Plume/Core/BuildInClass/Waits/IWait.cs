
namespace Plume.Core
{
    /// <summary>
    /// 等待-接口 (适用于wait支持)
    /// </summary>
    public interface IWait
    {
        /// <summary>
        /// 是否等待完成
        /// </summary>
        /// <returns></returns>
        bool WaitOK();
    }
}
