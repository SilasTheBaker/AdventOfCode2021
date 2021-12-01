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
    LogStreamType[] mStreamFilter;
    string mPrefix = "-    ";
    Dictionary<LogStreamType, ConsoleColor> mLogColors;

    public SBLog(LogStreamType[] filter)
    {
        if (mInstance == null)
        {
            mInstance = this;
            mStreamFilter = filter;

            mLogColors = new Dictionary<LogStreamType, ConsoleColor>()
            {
                { LogStreamType.Error, ConsoleColor.Red},
                { LogStreamType.Debug, ConsoleColor.Green},
                { LogStreamType.Default, ConsoleColor.Cyan},
                { LogStreamType.Output, ConsoleColor.Black},
                { LogStreamType.All, ConsoleColor.Cyan},
            };

        }
        else
            throw new Exception("An SBLog object already exists");
    }

    public static void SetFilter(LogStreamType[] filter)
    {
        Get.mStreamFilter = filter;
    }

    public static void AppendFilter(LogStreamType[] filter)
    {
        Get.mStreamFilter = Get.mStreamFilter.Union(filter).ToArray();
    }

    //Mess with this more later
    public static void Log(string debug, LogStreamType stream = LogStreamType.Default)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        if (SBLog.Get.ShouldOutput(stream))
        {
            Console.ForegroundColor = SBLog.Get.mLogColors[stream];
            Console.Write(SBLog.Get.mPrefix + debug, SBLog.Get.mLogColors[stream]);
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
            mOldFilter = SBLog.mInstance.mStreamFilter;

            if (append)
                SBLog.SetFilter(filter);
            else
                SBLog.AppendFilter(filter);
        }

        ~ScopedLogFilter()
        {
            SBLog.SetFilter(mOldFilter);
        }
    }
}

