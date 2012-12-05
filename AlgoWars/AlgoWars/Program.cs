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
        static int[,] Matrix;
        static int n;

        static void Main(string[] args)
        {
            Matrix = readConnectivityMatrix("input.txt");
            printMatrix();
            List<int> answer = solveProblem();
            List<int> test = new List<int>(new int[] { 1,2,3 });
            Console.WriteLine(validate("output.txt"));
            Console.ReadLine();
        }

        private static int[,] readConnectivityMatrix(string file)
        {
            char[] splitChars = {' '};
            StreamReader reader = new StreamReader(file);
            n = int.Parse(reader.ReadLine());
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

        private static List<int> solveProblem()
        {
            int starter = findMostConnections();
            return null;
        }

        private static int countConnections(int rowNum)
        {
            int sum = 0;
            for (int c = 0; c < n; c++)
                sum += Matrix[rowNum, c];
            return sum;
        }

        private static int findMostConnections()
        {
            int con = 0;
            for (int module = 0; module < n; module++)
            {
                int temp = countConnections(module);
                if (temp > con) con = temp;
            }
            return con;
        }

        private static void zero(int spaceOne, int spaceTwo)
        {
            Matrix[spaceOne, spaceTwo] = 0;
            Matrix[spaceTwo, spaceOne] = 0;
        }

        private static int getTotalLength(List<int> order)
        {
            int totalSum = 0;
            for (int start = 0; start < n-1; start++)
            {
                for (int next = start + 1; next < n; next++)
                {
                    totalSum += Matrix[order[start] - 1, order[next] - 1] * (next-start);
                }
            }
            return totalSum;
        }

        private static bool validate(string outputFile)
        {
            List<int> modOrder = new List<int>();
            char[] splitChars = { ' ' };
            StreamReader reader = new StreamReader(outputFile);
            string listLine = reader.ReadLine();
            int total = int.Parse(reader.ReadLine());
            string[] nums = listLine.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in nums)
            {
                modOrder.Add(int.Parse(s));
            }
            return total == getTotalLength(modOrder);
        }

        private static void printMatrix()
        {
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    Console.Write(Matrix[i, j] + " ");
                }
                Console.Write("| sum: " + countConnections(i));
                Console.WriteLine();
            }
        }
    }
}
