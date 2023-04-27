namespace VTU.Infrastructure.Exceptions;

/// <summary>
/// 业务错误异常类
/// </summary>
public class BusinessException : Exception
{
    public BusinessException(string? message) : base(message)
    {
    }
}