using Microsoft.Extensions.Configuration;

namespace VTU.Infrastructure;

public class AppSettingInstants
{
    private static IConfiguration Configuration { get; set; }
    private static AppSettings AppSettings { get; set; }

    public AppSettingInstants(IConfiguration configuration)
    {
        Configuration = configuration;
        AppSettings = new AppSettings();
        configuration.Bind(AppSettings);
    }

    /**
     * 获得系统配置信息
     */
    public static AppSettings GetAppSettings()
    {
        return AppSettings;
    }
}