using VTU.Infrastructure.Enums;
using VTU.Infrastructure.Extension;

namespace VTU.Infrastructure.Models;

public class JsonObject<T>
{
    public CodeEnums code { get; set; }

    private string? _msg;
    public string? msg
    {
        get => !string.IsNullOrEmpty(_msg) ? _msg : code.ToDescription();
        set => _msg = value;
    }

    public T reslut { get; set; }

    public static JsonObject<T> success(T value)
    {
        return new JsonObject<T> { code = CodeEnums.Success, reslut = value };
    }

    public static JsonObject<T> success(T value, CodeEnums codeEnums)
    {
        return new JsonObject<T> { code = codeEnums, reslut = value };
    }

    public static JsonObject<T> success(T value, CodeEnums codeEnums, string msg)
    {
        return new JsonObject<T> { code = codeEnums, reslut = value, msg = msg };
    }

    public static JsonObject<T> Fail(T value)
    {
        return new JsonObject<T> { code = CodeEnums.Fail, reslut = value };
    }

    public static JsonObject<T> Fail(T value, CodeEnums codeEnums)
    {
        return new JsonObject<T> { code = codeEnums, reslut = value };
    }

    public static JsonObject<T> Fail(T value, string msg)
    {
        return new JsonObject<T> { code = CodeEnums.Fail, reslut = value, msg = msg };
    }

    public static JsonObject<T> Fail(T value, CodeEnums codeEnums, string msg)
    {
        return new JsonObject<T> { code = codeEnums, reslut = value, msg = msg };
    }

    /// <summary>
    /// 隐式将T转化为ResponseResult<T>
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator JsonObject<T>(T value)
    {
        return new JsonObject<T> { reslut = value, code = CodeEnums.Success };
    }
}