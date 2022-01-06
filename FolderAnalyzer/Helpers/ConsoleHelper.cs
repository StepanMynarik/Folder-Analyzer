using System;

namespace FolderAnalyzer.Helpers
{
    public static class ConsoleHelper
    {
        #region Write

        public static void Write(string value, VerbosityLevel verbosityLevel = VerbosityLevel.Info)
        {
            ConsoleColor foregroundColor;

            switch (verbosityLevel)
            {
                case VerbosityLevel.Info:
                    Console.Write(value);
                    return;
                case VerbosityLevel.Success:
                    foregroundColor = ConsoleColor.Green;
                    break;
                case VerbosityLevel.Warning:
                    foregroundColor = ConsoleColor.Yellow;
                    break;
                case VerbosityLevel.Error:
                    foregroundColor = ConsoleColor.Red;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(verbosityLevel));
            }

            Write(value, foregroundColor);
        }

        public static void Write(string value, ConsoleColor foregroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.Write(value);
            Console.ResetColor();
        }

        public static void Write(string value, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(value);
            Console.ResetColor();
        }

        #endregion

        #region WriteLine

        public static void WriteEmptyLine()
        {
            WriteLine("");
        }
        
        public static void WriteLine(string value, VerbosityLevel verbosityLevel = VerbosityLevel.Info)
        {
            ConsoleColor foregroundColor;

            switch (verbosityLevel)
            {
                case VerbosityLevel.Info:
                    Console.WriteLine(value);
                    return;
                case VerbosityLevel.Success:
                    foregroundColor = ConsoleColor.Green;
                    break;
                case VerbosityLevel.Warning:
                    foregroundColor = ConsoleColor.Yellow;
                    break;
                case VerbosityLevel.Error:
                    foregroundColor = ConsoleColor.Red;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(verbosityLevel));
            }

            WriteLine(value, foregroundColor);
        }

        public static void WriteLine(string value, ConsoleColor foregroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static void WriteLine(string value, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        #endregion
    }

    public enum VerbosityLevel
    {
        Info,
        Success,
        Warning,
        Error
    }
}
