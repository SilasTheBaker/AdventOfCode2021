using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SBLog
{
    public enum LogStreamType
    {
        Error,
        Debug,
        Default,
        Output,
        StateInfo,
        All
    }

    public static SBLog Get
    {
        get
        {
            if (mInstance != null)
                return mInstance;
            else
                throw new Exception("An SBLog object doesn't exist and Get was called");
        }
    }

    static SBLog mInstance = null;
    List<LogStreamType> mStreamFilter;
    string mPrefix = "-    ";
    Dictionary<LogStreamType, ConsoleColor> mLogColors;

    public SBLog(LogStreamType[] filter)
    {
        if (mInstance == null)
        {
            mInstance = this;
            mStreamFilter = new List<LogStreamType>(filter);

            mLogColors = new Dictionary<LogStreamType, ConsoleColor>()
            {
                { LogStreamType.Error, ConsoleColor.Red},
                { LogStreamType.Debug, ConsoleColor.Green},
                { LogStreamType.Default, ConsoleColor.Cyan},
                { LogStreamType.Output, ConsoleColor.Black},
                { LogStreamType.StateInfo, ConsoleColor.Yellow},
                { LogStreamType.All, ConsoleColor.Cyan},
            };

        }
        else
            throw new Exception("An SBLog object already exists");
    }

    public static void SetFilter(LogStreamType[] filter)
    {
        Get.mStreamFilter = new List<LogStreamType>(filter);
    }

    internal static void Log(object p, LogStreamType debug)
    {
        throw new NotImplementedException();
    }

    public static void SetFilter(LogStreamType filter)
    {
        Get.mStreamFilter.Clear();
        Get.mStreamFilter.Add(filter);

    }

    public static void AppendFilter(LogStreamType[] filter)
    {
        Get.mStreamFilter = new List<LogStreamType>(Get.mStreamFilter.Union(filter));
    }

    public static void AppendFilter(LogStreamType filter)
    {
        if (!Get.mStreamFilter.Contains(filter))
        {
            Get.mStreamFilter.Add(filter);
        }
    }

    public static void Log(string debug, LogStreamType stream = LogStreamType.Default)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        if (SBLog.Get.ShouldOutput(stream))
        {
            Console.ForegroundColor = SBLog.Get.mLogColors[stream];
            Console.Write(debug, SBLog.Get.mLogColors[stream]);
        }
        Console.ForegroundColor = oldColor;
    }

    public static void LogLine(string debug, LogStreamType stream = LogStreamType.Default)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        if (SBLog.Get.ShouldOutput(stream))
        {
            Console.ForegroundColor = SBLog.Get.mLogColors[stream];
            Console.WriteLine(SBLog.Get.mPrefix + debug, SBLog.Get.mLogColors[stream]);
        }
        Console.ForegroundColor = oldColor;
    }

    public static void LogHighlightedLine(string debug, LogStreamType stream = LogStreamType.Default)
    {
        ConsoleColor oldFrontColor = Console.ForegroundColor;
        ConsoleColor oldBackColor = Console.BackgroundColor;

        if (SBLog.Get.ShouldOutput(stream))
        {
            Console.ForegroundColor = SBLog.Get.mLogColors[stream];
            Console.Write(SBLog.Get.mPrefix);
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(debug, SBLog.Get.mLogColors[stream]);
        }

        Console.ForegroundColor = oldFrontColor;
        Console.BackgroundColor = oldBackColor;
    }

    bool ShouldOutput(LogStreamType streamType)
    {
        return mStreamFilter.Contains(streamType) || mStreamFilter.Contains(LogStreamType.All);
    }

    public class ScopedLogFilter
    {
        LogStreamType[] mOldFilter;

        public ScopedLogFilter(LogStreamType[] filter, bool append)
        {
            mOldFilter = SBLog.Get.mStreamFilter.ToArray();

            if (append)
                SBLog.SetFilter(filter);
            else
                SBLog.AppendFilter(filter);
        }

        public ScopedLogFilter(LogStreamType filter, bool append)
        {
            mOldFilter = SBLog.Get.mStreamFilter.ToArray(); ;

            if (append)
                SBLog.SetFilter(filter);
            else
                SBLog.AppendFilter(filter);
        }

        ~ScopedLogFilter() //TODO - fix this
        {
            SBLog.SetFilter(mOldFilter);
        }
    }
}

