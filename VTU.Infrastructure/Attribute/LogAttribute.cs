using VTU.Infrastructure.Enums;

namespace VTU.Infrastructure.Attribute;

/// <summary>
/// 自定义操作日志记录注解
/// </summary>
public class LogAttribute : System.Attribute
{
    public string Title { get; set; }
    public LogType BusinessType { get; set; }

    /// <summary>
    /// 是否保存请求数据
    /// </summary>
    public bool IsSaveRequestData { get; set; } = true;

    /// <summary>
    /// 是否保存返回数据
    /// </summary>
    public bool IsSaveResponseData { get; set; } = true;

    private LogAttribute()
    {
    }

    public LogAttribute(string name)
    {
        Title = name;
    }

    public LogAttribute(string name, LogType businessType, bool saveRequestData = true,
        bool saveResponseData = true)
    {
        Title = name;
        BusinessType = businessType;
        IsSaveRequestData = saveRequestData;
        IsSaveResponseData = saveResponseData;
    }
}