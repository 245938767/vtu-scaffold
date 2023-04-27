using System.ComponentModel;

namespace VTU.Infrastructure.Models;

public class Response
{
    [Description("ID")] public int Id { get; init; }
    [Description("创建时间")] public DateTime CreateDateTime { get; set; }
    [Description("更新时间时间")] public DateTime UpdateDateTime { get; set; }
}