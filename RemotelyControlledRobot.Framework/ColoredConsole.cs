using System;
namespace RemotelyControlledRobot.Framework
{
	public static class ColoredConsole
	{
		public static void WriteLine<T>(T input, ConsoleColor color)
		{
			var previousColor = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.WriteLine(input);
			Console.ForegroundColor = previousColor;
        }

		public static void WriteLineGreen<T>(T input) => WriteLine(input, ConsoleColor.Green);
		public static void WriteLineYellow<T>(T input) => WriteLine(input, ConsoleColor.Yellow);
    }
}