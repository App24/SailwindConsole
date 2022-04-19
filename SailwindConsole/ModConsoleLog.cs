using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace SailwindConsole
{
    public static class ModConsoleLog
    {
        private static string[] logColors = new string[] { "fuchsia", "blue", "orange", "red" };

        public static void WriteNewLine()
        {
            ModConsole.logText.text += Environment.NewLine;
            ModConsole.MoveScrollToEnd();
        }

        internal static void Write(params object[] message)
        {
            string str = "";
            foreach (object item in message)
            {
                str += GetString(item);
            }
            ModConsole.logText.text += str;
            WriteNewLine();
        }

        static string GetString(object obj)
        {
            string str = "";
            if(obj is IEnumerable enumerable)
            {
                foreach(object item in enumerable)
                {
                    str += GetString(item);
                }
            }
            else
            {
                str += obj.ToString();
            }
            return str;
        }

        private static void Log(LogLevel logLevel, params object[] message)
        {
            Write($"[<color={logColors[(int)logLevel]}>{logLevel}</color>] ", message);
        }

        internal static void Log(params object[] message)
        {
            Log(LogLevel.Info, message);
        }

        internal static void Error(params object[] message)
        {
            Log(LogLevel.Error, message);
        }

        internal static void Warn(params object[] message)
        {
            Log(LogLevel.Warn, message);
        }

        internal static void Debug(params object[] message)
        {
            Log(LogLevel.Debug, message);
        }

        public static void Log(UnityModManager.ModEntry mod, params object[] message)
        {
            Log(LogLevel.Info, $"[<color=blue>{mod.Info.DisplayName}</color>]: ", message);
        }

        public static void Error(UnityModManager.ModEntry mod, params object[] message)
        {
            Log(LogLevel.Error, $"[<color=blue>{mod.Info.DisplayName}</color>]: ", message);
        }

        public static void Warn(UnityModManager.ModEntry mod, params object[] message)
        {
            Log(LogLevel.Warn, $"[<color=blue>{mod.Info.DisplayName}</color>]: ", message);
        }

        public static void Debug(UnityModManager.ModEntry mod, params object[] message)
        {
            Log(LogLevel.Debug, $"[<color=blue>{mod.Info.DisplayName}</color>]: ", message);
        }

        private enum LogLevel { Debug, Info, Warn, Error }
    }
}
