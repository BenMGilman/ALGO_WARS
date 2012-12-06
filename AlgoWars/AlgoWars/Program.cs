using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoWars
{
    class Program {

		public class HashDequeue {

			public class Node {

				public int fromRight {
					get {
						if (dir) {
							return right.next.count + count + 1 + right.count;
						}
						return right.next.count - count + right.data;
					}
				}

				public int fromLeft {
					get {
						if (!dir) {
							return left.next.count + count + 1 + left.data;
						}
						return left.next.count - count + left.count;
					}
				}

				public Node(Node next, Node prev, int data) {
					this.prev = prev;
					this.next = next;
					this.data = data;
				}

				public Node prev;
				public Node next;
				public int data;

				// for static nodes: count - left, data - right
				public Node left = null;  // left.next will point to left 
				public int count = 0;  // how far from center
				public Node right = null;  // right.next will point to far right
				public bool dir = false;  // true for left of center, false for right

				public override string ToString() {
					return data.ToString();
				}
			}

			public Node root;
			public Dictionary<int, Node> hash = new Dictionary<int, Node>();

			public Node getInt(int i) {
				Node n;
				hash.TryGetValue(i, out n);
				return n;
			}

			public void insert(Node n) {
				hash[n.data] = n;
			}

			public bool Contains(int i) {
				return getInt(i) != null;
			}

			public List<int> toList() {
				List<int> list = new List<int>();

				if (root == null) {
					answer.root = answer.hash.ElementAt(0).Value;
					while (answer.root.prev != null)
						answer.root = answer.root.prev;
				}
				while (root != null) {
					list.Add(root.data + 1);
					root = root.next;
				}
				return list;
			}
		}

		static HashDequeue answer = new HashDequeue();
		static int[,] Matrix;
		static int[,] Matrix2;
        static int n;

        static void Main(string[] args)
        {
            Matrix = readConnectivityMatrix("input.txt");
			Matrix2 = (int[,]) Matrix.Clone();
            //printMatrix();
			outputAnswer(solveProblem(Matrix2), "output.txt");
            Console.WriteLine(validate("output.txt"));
            Console.ReadKey();
        }

		static void printList(List<int> list) {
			foreach (int i in list) {
				Console.Write(i);
				Console.Write(" ");
			}

			Console.WriteLine();
			Console.Write(getTotalLength(list));
		}

        private static void outputAnswer(List<int> answer, string file)
        {
            StreamWriter writer = new StreamWriter(file);
            foreach (int i in answer)
                writer.Write(i.ToString() + " ");
            writer.WriteLine();
            writer.WriteLine(getTotalLength(answer));
            writer.Close();
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
            reader.Close();
            return Matrix;
        }

		// does everything - finds closest node to the one we want to be next too and places it there
		private static void findBest(HashDequeue.Node n, HashDequeue.Node connect) {		

			HashDequeue.Node t = n;
			while (t != null)
				if (t.data == connect.data)
					return;
				else t = t.next;

			t = n;
			while (t != null)
				if (t.data == connect.data)
					return;
				else t = t.prev;

			HashDequeue.Node Lconnect = fromLeft(connect);
			HashDequeue.Node Rconnect = fromRight(connect);
			HashDequeue.Node L = fromLeft(n);
			HashDequeue.Node R = fromRight(n);

			answer.insert(connect);
			if (L.data + Rconnect.data < R.data + Lconnect.data) {
				L.next.next = Rconnect.next;
				Rconnect.next.prev = L.next;
				return;
			}
			R.next.prev = Lconnect.next;
			Lconnect.next.next = R.next;
		}

		private static HashDequeue.Node fromRight(HashDequeue.Node n){
			int i = 0;
			while (n.prev != null) {
				n = n.prev;
				++i;
			}
			return new HashDequeue.Node(n, null, i);
		}

		private static HashDequeue.Node fromLeft(HashDequeue.Node n) {
			int i = 0;
			while (n.next != null) {
				n = n.next;
				++i;
			}
			return new HashDequeue.Node(n, null, i);
		}

        private static List<int> solveProblem(int[,] matrix)
        {
			int row;
			int col;
			while ((row = findMostConnections(matrix)) >= 0) {
				col = getRowHigh(row, matrix);
				if (answer.Contains(col)) {
					if (answer.Contains(row))
						findBest(answer.getInt(col), answer.getInt(row));
					else
						findBest(answer.getInt(col), new HashDequeue.Node(null, null, row));
				} else {
					HashDequeue.Node n = new HashDequeue.Node(null, null, col);
					answer.insert(n);
					if (answer.Contains(row))
						findBest(n, answer.getInt(row));
					else
						findBest(n, new HashDequeue.Node(null, null, row));
				}
				zero(row, col, matrix);
			}

			return answer.toList();
        }

        private static int countConnections(int rowNum, int[,] matrix)
        {
            int sum = 0;
            for (int c = 0; c < n; c++)
                sum += matrix[rowNum, c];
            return sum;
        }

		private static int getRowHigh(int rowNum, int[,] matrix) {
			int count = 0;
			int index = -1;
			for (int i = 0; i < n; ++i) {
				if (matrix[rowNum, i] > count) {
					count = Matrix[rowNum, i];
					index = i;
				}
			}
			return index;
		}

        private static int findMostConnections(int[,] matrix)
        {
            int con = 0;
			int index = -1;
            for (int module = 0; module < n; module++)
            {
                int temp = countConnections(module, matrix);
				if (temp > con) {
					con = temp;
					index = module;
				}
            }
            return index;
        }

		private static void zero(int spaceOne, int spaceTwo, int[,] matrix)
        {
            matrix[spaceOne, spaceTwo] = 0;
            matrix[spaceTwo, spaceOne] = 0;
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
            if (nums.Length != n) return false;

			bool[] check = new bool[n];
            foreach (string s in nums)
            {
				int temp = int.Parse(s);
				modOrder.Add(temp);
				if (check[temp-1])
					return false;
				check[temp-1] = true;
            }
            reader.Close();
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
                Console.Write("| sum: " + countConnections(i, Matrix));
                Console.WriteLine();
            }
        }
    }
}
