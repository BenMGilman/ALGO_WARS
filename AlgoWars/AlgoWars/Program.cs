using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoWars
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] M;
            M = readConnectivityMatrix("input.txt");
            printMatrix(M);
            Console.ReadLine();
        }

        private static int[,] readConnectivityMatrix(string file)
        {
            char[] splitChars = {' '};
            StreamReader reader = new StreamReader(file);
            int n = int.Parse(reader.ReadLine());
            int[,] Matrix = new int[n,n];
            for (int i = 0; i < n; i++)
            {
                string line = reader.ReadLine();
                string[] nums = line.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < nums.Length; j++)
                {
                    Matrix[i,j] = int.Parse(nums[j]);
                }
            }
            return Matrix;
        }

        private static void printMatrix(int[,] M)
        {
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    Console.Write(M[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
