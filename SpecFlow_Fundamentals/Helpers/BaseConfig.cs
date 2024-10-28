using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

public static class BaseConfig
{
    private static readonly string ProjectDirectory = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(
                                                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))),"..");

    private static readonly string AppSettingsPath = Path.Combine(ProjectDirectory, "appsettings.json");

    private static readonly IConfigurationRoot Config = new ConfigurationBuilder()
        .AddJsonFile(AppSettingsPath)
        .Build();

    public static string BaseUrl => Config["BaseUrl"];
    public static string DefaultUser => Config["DefaultUser"];
    public static string LockedOutUser => Config["LockedOutUser"];
    public static string ProblemUser => Config["ProblemUser"];
    public static string PerformanceGlitchUser => Config["PerformanceGlitchUser"];
    public static string Password => Config["Password"];
    public static string IncorrectUser => Config["IncorrectUser"];
    public static string IncorrectPassword => Config["IncorrectPassword"];
}
