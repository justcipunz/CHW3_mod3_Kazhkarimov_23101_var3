using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using System.Diagnostics;
using static CHW3_mod3_Kazhkarimov_23101_var3.ConstantProperties;  

namespace CHW3_mod3_Kazhkarimov_23101_var3;

/// <summary>
/// Class for creating and recording logs in console and in text files.
/// </summary>
public class Logger
{
    private readonly static ILogger<Logger> logger;
    private readonly static string logFilePath;

    /// <summary>
    /// Static initialization of logger and creating necessary directories.
    /// </summary>
    static Logger()
    {
        logFilePath += LogPath + $"{Path.DirectorySeparatorChar}Console_log_{DateTime.Now:dd-MM}.txt";
        
        if (!File.Exists(logFilePath))
        {
            var st = File.Create(logFilePath);
            st.Close();
        }

        using StreamWriter logFileWriter = new(logFilePath, append: true);
        ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        logger = loggerFactory.CreateLogger<Logger>();
    }

    /// <summary>
    /// Records info about method started to execute.
    /// </summary>
    /// <param name="methodName"> Name of method started to execute. </param>
    public static void WriteStartLog(string methodName)
    {
        string text = $"{methodName} request at {DateTime.Now:HH:mm:ss}";
        logger.LogInformation(text);
        using StreamWriter logFileWriter = new(logFilePath, append: true);
        logFileWriter.WriteLine(text);
    }

    /// <summary>
    /// Records info about method ended to execute.
    /// </summary>
    /// <param name="methodName"> Name of method ended to execute. </param>
    public static void WriteStopLog(string methodName)
    {
        string text = $"{methodName} successfully completed at {DateTime.Now:HH:mm:ss}";
        logger.LogInformation(text);
        using StreamWriter logFileWriter = new(logFilePath, append: true);
        logFileWriter.WriteLine(text);
    }

    /// <summary>
    /// Records info about error.
    /// </summary>
    /// <param name="methodName"> Name of method called error. </param>
    /// <param name="ex"> Occured error. </param>
    public static void WriteErrorLog(string methodName, Exception ex)
    {
        string text = $"An error occurred in {methodName} at {DateTime.Now:HH:mm:ss}\n{ex.GetType()}\n{ex.Message}";
        logger.LogError(text);
        using StreamWriter logFileWriter = new(logFilePath, append: true);
        logFileWriter.WriteLine(text);
    }
}