using Serilog;
using Serilog.Enrichers;
using System.Text;
using TowerSolver.Helper;

namespace TowerSolver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.With(new ThreadIdEnricher())
                .WriteTo.Console(
                    outputTemplate: "{Timestamp:HH:mm} [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}")
                .CreateLogger();

            var input = new byte[6, 6]
            {
                {1, 2, 5, 4, 6, 3 },
                {5, 3, 6, 1, 4, 2 },
                {4, 6, 3, 5, 2, 1 },
                {2, 1, 4, 3, 5, 6 },
                {3, 5, 2, 6, 1, 4 },
                {6, 4, 1, 2, 3, 5 },
            };


            byte[,] output = new byte[6, 6];


            output = new byte[6, 6]
            {
                {0,0,0,0,0,0 },
                {1,1,1,1,1,1 },
                {2,2,2,2,2,2 },
                {3,3,3,3,3,3 },
                {4,4,4,4,4,4 },
                {5,5,5,5,5,5,},
            };

            var resultLogger = Log.Logger.ForContext<Program>();
            System.Console.Out.WriteLine("Proposed solution:");
            colorPrintConsole(output);
            //resultLogger.Information("Proposed solution {Output}", printArry(input));

            var colorTowers = new HashSet<byte>[6];
            for (int i = 0; i < 6; i++)
            {
                colorTowers[i] = new HashSet<byte>();
            }

            // check all colors only once
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (!colorTowers[output[i, j]].Add(input[i, j]))
                    {
                        resultLogger.Error("Found color {Color} in the size {Size} twice", output[i, j], input[i, j]);
                    }
                }
            }

            for (int i = 0; i < 6; i++)
            {
                HashSet<byte> rowColors = new();
                HashSet<byte> colColors = new();
                for (int j = 0; j < 6; j++)
                {
                    if (!rowColors.Add(output[i, j]))
                    {
                        resultLogger.Error("Found color {Color} in the row {Row} twice", output[i, j], i);
                    }
                    if (!colColors.Add(output[j, i]))
                    {
                        resultLogger.Error("Found color {Color} in the col {Col} twice", output[i, j], i);
                    }
                }
            }
        }

        public static string printArry<T>(T[,] array)
        {
            StringBuilder output = new();
            for (int i = 0; i < array.GetLength(0); i++)
            {
                output.AppendJoin(" ", array.GetRow(i).Items());
                output.AppendLine();
            }
            return output.ToString();
        }

        private static ConsoleColor[] CollorArray = new ConsoleColor[] { ConsoleColor.DarkGreen, ConsoleColor.DarkYellow, ConsoleColor.Cyan, ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.Magenta };

        public static void colorPrintConsole(byte[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.ForegroundColor = CollorArray[array[i, j]];
                    Console.Out.Write(array[i, j]);
                    if (j < array.GetLength(1) + 1)
                    {
                        Console.Out.Write(" ");
                    }
                }
                Console.Out.WriteLine();
            }
            Console.ResetColor();
        }
    }
}