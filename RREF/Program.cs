#define DEBUG
//#undef DEBUG

using System;
using System.Diagnostics;

namespace RREF
{
    class Program
    {
        private static double[,] matrix;
        private static double[,] gaussReducedMatrix;
        private static double[,] backReducedMatrix;
        private static double[,] gaussJordanReducedMatrix;
        private static bool test;
        private static bool fail;
        private static bool gaussBack;
        private static bool bothMethods;

        static void Main(string[] args)
        {
            // Default values loaded
            test = false;
            fail = false;
            bothMethods = false;
            gaussBack = true;

            // Get user values
#if DEBUG
            FindOutIfTest();
#endif
            if (test)
                LoadTestMatrix();
            else
                ParseMatrix();
            FindOutIfGaussBack();
            Console.WriteLine();

            // Actually perform reduction, print out stats about the reduction
            if (!fail)
            {
                
                if (gaussBack)
                {
                    GaussBackReduce();
                }
                else
                {
                    if (!bothMethods)
                    {
                        GaussJordanReduce();
                    }
                    else
                    {
                        GaussBackReduce();
                        Console.WriteLine();
                        GaussJordanReduce();
                    }
                }
                
            }

            // End execution
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        static void FindOutIfTest()
        {
            Console.Write("Is this a test run? (y/n): ");
            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Y:
                        test = true;
                        Console.WriteLine();
                        return;
                    case ConsoleKey.N:
                        test = false;
                        Console.WriteLine();
                        return;
                    default:
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        break;
                }
            }
        }

        static void FindOutIfGaussBack()
        {
            Console.Write("Which method should be used: [G]auss Reduction + Back Reduction, Gauss-[J]ordan Reduction, or [B]oth? (g/j/b): ");
            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.G:
                        gaussBack = true;
                        Console.WriteLine();
                        return;
                    case ConsoleKey.J:
                        gaussBack = false;
                        Console.WriteLine();
                        return;
                    case ConsoleKey.B:
                        bothMethods = true;
                        gaussBack = false;
                        return;
                    default:
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        break;
                }
            }
        }

        static void ParseMatrix()
        {
            // Get rows and columns of the matrix
            Console.Write("Enter the number of rows in the matrix: ");
            string rowsIn = Console.ReadLine();
            if (!int.TryParse(rowsIn, out int rows))
            {
                Console.WriteLine("Error: Expected integer value. Exiting...");
                fail = true;
                return;
            }
            Console.Write("Enter the number of columns in the matrix: ");
            string colsIn = Console.ReadLine();
            if (!int.TryParse(colsIn, out int cols))
            {
                Console.WriteLine("Error: Expected integer value. Exiting...");
                fail = true;
                return;
            }

            // Initialize matrix, ask for each entry
            matrix = new double[rows, cols];

            Console.WriteLine(@"
NOTE: all entries will be converted to double if not inputted as a double. Do not try to put in fractions, please convert them to a double value.

NOTE 2: the ordered pair for the entry is not 0-based, for easy reading. (1,1) corresponds to the first entry of the first row of the matrix.
");

            string rawEntry;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write($"Entry at ({i + 1}, {j + 1}): ");
                    rawEntry = Console.ReadLine();
                    if (!double.TryParse(rawEntry, out double entry))
                    {
                        Console.WriteLine("Error: Expected double value. Exiting...");
                        fail = true;
                        return;
                    }
                    matrix[i, j] = entry;
                }
            }
            Console.WriteLine();
            Console.WriteLine("Matrix input complete. Here is the entered matrix:");

            // Print out matrix for confirmation
            PrintMatrix(matrix);
            Console.WriteLine();
        }

        static void LoadTestMatrix()
        {
            // This matrix is known to have an RREF that is the 3x3 identitiy matrix
            matrix = new double[3, 3] 
            {
                {-1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            };
            Console.WriteLine("Loaded default testing matrix. Default matrix:");
            PrintMatrix(matrix);
            Console.WriteLine();
        }

        static void PrintMatrix(double[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Console.Write("|");
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    string num = matrix[i, j].ToString("0.000");
                    if ((matrix[i, j] % 1) != 0)
                    {
                        Console.Write(String.Format("{0,6}", num));
                    }
                    else
                    {
                        Console.Write(String.Format("{0,6}", matrix[i, j]));
                    }

                    // Check to not print commas on last column of a row
                    if (j != matrix.GetLength(1) - 1)
                        Console.Write(", ");
                }
                Console.Write("|");
                Console.WriteLine();
            }
        }

        static void GaussBackReduce()
        {
            int totalOperations = 0;
            Stopwatch timer;

            Console.WriteLine("Using Gauss Reduction followed by Back Reduction...");

            timer = Stopwatch.StartNew();
            gaussReducedMatrix = RREF.GaussReduction(matrix);
            timer.Stop();
            totalOperations += RREF.NumRowOps;
            Console.WriteLine("REF of matrix:");
            PrintMatrix(gaussReducedMatrix);
            Console.WriteLine();

            timer.Start();
            backReducedMatrix = RREF.BackReduction(gaussReducedMatrix);
            timer.Stop();
            totalOperations += RREF.NumRowOps;
            Console.WriteLine("RREF of matrix:");
            PrintMatrix(backReducedMatrix);
            Console.WriteLine();

            Console.WriteLine($"Time elapsed for Gauss & Back reduction: {timer.ElapsedMilliseconds}ms");
            Console.WriteLine($"Gauss & Back reduction took {totalOperations} row operation(s)");
        }

        static void GaussJordanReduce()
        {
            Stopwatch timer;

            Console.WriteLine("Using Gauss-Jordan Reduction...");

            timer = Stopwatch.StartNew();
            gaussJordanReducedMatrix = RREF.GaussJordanReduction(matrix);
            timer.Stop();
            Console.WriteLine("RREF of matrix:");
            PrintMatrix(gaussJordanReducedMatrix);
            Console.WriteLine();

            Console.WriteLine($"Time elapsed for Gauss-Jordan reduction: {timer.ElapsedMilliseconds}ms");
            Console.WriteLine($"Gauss-Jordan reduction took {RREF.NumRowOps} row operation(s)");
        }
    }
}
