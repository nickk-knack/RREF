using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREF
{
    class RREF
    {
        public static int NumRowOps { get; set; }
        private static double[,] matrix;
        private static double[] tempRow;
        private static int leadingEntry;

        public static double[,] GaussReduction(double[,] inputMatrix)
        {
            // Set the matrix variable using the matrix that is passed in
            matrix = inputMatrix;
            NumRowOps = 0;

            // If its a row vector, we're done
            if (matrix.GetLength(0) == 1)
                return matrix;

            // Getting our first leading entry
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                if (matrix[0, i] == 0)
                {
                    for (int j = 0; j < matrix.GetLength(0); j++)
                    {
                        if (matrix[j, i] != 0)
                        {
                            RowExchange(0, j);
                            break;
                        }
                    }

                    if (matrix[0, i] != 0)
                    {
                        leadingEntry = i;
                        break;
                    }
                }
            }
            // Delete all non-zero entries below the leading entry
            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                if (matrix[i, leadingEntry] != 0)
                {
                    RowTransvection(i, 0, matrix[i, leadingEntry] / matrix[0, leadingEntry]);
                }
            }

            // Go through remaining rows, get leading entries, continue putting into REF
            for (int row = 1; row < matrix.GetLength(0); row++)
            {
                // Check if its a 0 row, if so then swap with below and execute the check again
                if ((row != matrix.GetLength(0) - 1) && IsZeroRow(row))
                {
                    for (int i = row + 1; i < matrix.GetLength(0); i++)
                    {
                        if (!IsZeroRow(i))
                        {
                            RowExchange(row, i);
                            break;
                        }
                    }
                }

                // Get leading entry
                for (int col = 1; col < matrix.GetLength(1); col++)
                {
                    if (matrix[row, col] != 0)
                    {
                        leadingEntry = col;
                        break;
                    }
                }

                // Zero out all entries below leading entry
                for (int i = row + 1; i < matrix.GetLength(0); i++)
                {
                    if (matrix[i, leadingEntry] != 0)
                    {
                        RowTransvection(i, row, matrix[i, leadingEntry] / matrix[row, leadingEntry]);
                    }
                }
            }

            // Return the strict Gauss reduced matrix
            return matrix;
        }

        public static double[,] BackReduction(double[,] inputMatrix)
        {
            // Set the matrix variable using the matrix that is passed in
            matrix = inputMatrix;
            NumRowOps = 0;

            // If its a row vector, we're done
            if (matrix.GetLength(0) == 1)
                return matrix;

            // Go through each row, get leading entry (if its not a zero row), normalize leading entry, zero out entries above it
            for (int row = matrix.GetLength(0) - 1; row > 0; row--)
            {
                // If the current row is a zero row, skip this iteration and move up a row.
                if (IsZeroRow(row))
                {
                    continue;
                }

                // Find leading entry of the current row
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    if (matrix[row, col] != 0)
                    {
                        leadingEntry = col;
                        break;
                    }
                }

                // Check if leading entry is 1. If not, do row op to make it 1
                if (matrix[row, leadingEntry] != 1)
                {
                    RowMultiplication(row, 1 / matrix[row, leadingEntry]);
                }

                // Go through each row above current row, zero out each non-zero entry
                for (int i = row - 1; i >= 0; i--)
                {
                    if (matrix[i, leadingEntry] != 0)
                    {
                        RowTransvection(i, row, matrix[i, leadingEntry]);
                    }
                }
            }

            // Normalize the first row
            if (matrix[0, 0] != 1)
            {
                RowMultiplication(0, 1 / matrix[0, 0]);
            }

            // Return the strict back reduced matrix
            return matrix;
        }

        public static double[,] GaussJordanReduction(double[,] inputMatrix)
        {
            matrix = inputMatrix;
            NumRowOps = 0;

            // TODO: Implement

            return matrix;
        }

        public static void RowTransvection(int row1, int row2, double factor)
        {
            //Ri -> Ri - kRj
            tempRow = new double[matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                matrix[row1, i] -= (factor * matrix[row2, i]);
            }
            NumRowOps++;
        }

        public static void RowMultiplication(int row, double factor)
        {
            // Ri -> kRi
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                matrix[row, i] *= factor;
            }
            NumRowOps++;
        }

        public static void RowExchange(int row1, int row2)
        {
            // Ri <-> Rj
            tempRow = new double[matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                tempRow[i] = matrix[row1, i];
                matrix[row1, i] = matrix[row2, i];
                matrix[row2, i] = tempRow[i];
            }
            NumRowOps++;
        }

        public static bool IsZeroRow(int row)
        {
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                if (matrix[row, i] != 0)
                    return false;
            }
            return true;
        }
    }
}
