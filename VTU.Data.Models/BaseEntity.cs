using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DbModel;

public class BaseEntity
{
    [Key]
    [Description("ID")] public int Id { get; init; }
    [Description("创建时间")] public DateTime CreateDateTime { get; set; }
    [Description("更新时间时间")] public DateTime UpdateDateTime { get; set; }
    [Description("乐观锁")] [Timestamp] public byte[] Version { get; private set; }

    public void preCreateTime()
    {
        CreateDateTime = DateTime.Now;
        UpdateDateTime = DateTime.Now;
    }

    public void preUpdateTime()
    {
        UpdateDateTime = DateTime.Now;
    }
}