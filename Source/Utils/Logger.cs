#nullable enable
using System;
using System.IO;
using System.Text;

namespace Kiero.Net;

public enum LogLevel
{
    Debug = 0,
    Info = 1,
    Warn = 2,
    Error = 3,
    None = 4,
}

public static class Logger
{
    private static readonly object sync = new();
    private static StreamWriter? file;
    private static string? logPath;
    private static long maxBytes = 10 * 1024 * 1024;
    private static int maxFiles = 5;

    public static LogLevel minLevel { get; set; } = LogLevel.Debug;

    public static void Initialize(string? directory = null, string fileName = "Kiero.log")
    {
        lock (sync)
        {
            if (file != null) return;

            try
            {
                directory ??= Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Kiero.Net");

                Directory.CreateDirectory(directory);

                logPath = Path.Combine(directory, fileName);
                RotateIfNeeded(logPath, maxBytes, maxFiles);

                file = new StreamWriter(
                    new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.Read),
                    new UTF8Encoding(false))
                {
                    AutoFlush = true,
                };

                WriteRaw(LogLevel.Info, "Logger", "========== session start ==========");
            }
            catch
            {
                file = null;
            }
        }
    }

    public static void Shutdown()
    {
        lock (sync)
        {
            if (file == null) return;

            try
            {
                WriteRaw(LogLevel.Info, "Logger", "========== session end ==========");
                file.Flush();
                file.Dispose();
            }
            catch { }
            finally
            {
                file = null;
                logPath = null;
            }
        }
    }

    public static void Debug(string tag, string message) => Write(LogLevel.Debug, tag, message);
    public static void Info(string tag, string message) => Write(LogLevel.Info, tag, message);
    public static void Warn(string tag, string message) => Write(LogLevel.Warn, tag, message);
    public static void Error(string tag, string message) => Write(LogLevel.Error, tag, message);

    public static void Error(string tag, Exception ex)
        => Write(LogLevel.Error, tag, $"{ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");

    private static void Write(LogLevel level, string tag, string message)
    {
        if (level < minLevel) return;

        lock (sync)
        {
            if (file == null) Initialize();
            WriteRaw(level, tag, message);
        }
    }

    private static void WriteRaw(LogLevel level, string tag, string message)
    {
        string stamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        string prefix = level switch
        {
            LogLevel.Debug => "DBG",
            LogLevel.Info => "INF",
            LogLevel.Warn => "WRN",
            LogLevel.Error => "ERR",
            _ => "???",
        };

        string line = $"[{stamp}] {prefix} [{tag}] {message}";

        try
        {
            file?.WriteLine(line);
            file?.Flush();
        }
        catch { }

        System.Diagnostics.Debug.WriteLine(line);
    }

    private static void RotateIfNeeded(string path, long maxBytes, long maxFiles)
    {
        try
        {
            if (!File.Exists(path)) return;
            var fi = new FileInfo(path);
            if (fi.Length < maxBytes) return;

            string oldest = $"{path}.{maxFiles}";
            if (File.Exists(oldest)) File.Delete(oldest);

            for (int i = (int)maxFiles - 1; i >= 1; i--)
            {
                string src = $"{path}.{i}";
                string dst = $"{path}.{i + 1}";
                if (File.Exists(src))
                {
                    if (File.Exists(dst)) File.Delete(dst);
                    File.Move(src, dst);
                }
            }

            File.Move(path, $"{path}.1");
        }
        catch { }
    }
}