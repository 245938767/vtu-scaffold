using System.ComponentModel;

namespace VTU.Infrastructure;

public class AppSettings
{
    public JwtSettings JwtSettings { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
}

public class JwtSettings
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int Expire { get; set; }
}

public class ConnectionStrings
{
    [Description("数据库类型0-MySQL 1-SQLite 2-SQLServer")]
    public int SQLType { get; set; }

    public string SQLite { get; set; }
    public string MySQL { get; set; }
    public string SQLServer { get; set; }
}