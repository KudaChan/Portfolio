using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure.CoreDataStructure.DataCollection.SimpleArray
{
    internal class SimpleArray
    {
        private int[]? array;
        private int[,]? twoDimArray;
        private int[,,]? threeDimArray;

        public void SimpleArrayStarter()
        {
            Console.WriteLine("Select whether you want pre-defined or user-defined array");
            Console.WriteLine("  1. Pre-defined Array \n  2. User-defined Array");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    PreDefinedArray();
                    break;

                case 2:
                    UserDefinedArray();
                    break;

                default:
                    Console.WriteLine("Invalid selection");
                    break;
            }
        }

        private void PreDefinedArray()
        {
            Console.WriteLine("Select the type of array to implement: ");
            Console.WriteLine("  1. 1-D Array \n  2. 2-D Array \n  3. 3-D Array");
            int type = Convert.ToInt32(Console.ReadLine());

            switch (type)
            {
                case 1:
                    OneDimArray();
                    break;

                case 2:
                    TwoDimArray();
                    break;

                case 3:
                    ThreeDimArray();
                    break;
            }
        }

        private void UserDefinedArray()
        {
            Console.WriteLine("Enter the size of the array: ");
            int size = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Select the type of array to implement: ");
            Console.WriteLine("  1. 1-D Array \n  2. 2-D Array \n  3. 3-D Array");
            int type = Convert.ToInt32(Console.ReadLine());

            switch (type)
            {
                case 1:
                    OneDimArray(size);
                    break;

                case 2:
                    TwoDimArray(size);
                    break;

                case 3:
                    ThreeDimArray(size);
                    break;
            }
        }

        private void OneDimArray()
        {
            int size = 5;

            if (array == null)
            {
                array = new int[size];
            }

            for (int i = 0; i < size; i++)
            {
                array[i] = i + 1;
            }

            Console.WriteLine("The elements in the array are: ");
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine(array[i]);
            }
        }

        private void OneDimArray(int size)
        {
            if (array == null)
            {
                array = new int[size];
            }

            for (int i = 0; i < size; i++)
            {
                array[i] = Convert.ToInt32(Console.ReadLine());
            }

            Console.WriteLine("The elements in the array are: ");
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine(array[i]);
            }
        }

        private void TwoDimArray()
        {
            int size = 5;

            if (twoDimArray == null)
            {
                twoDimArray = new int[size, size];
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    twoDimArray[i, j] = i + j;
                }
            }

            Console.WriteLine("The elements in the 2-D array are: ");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(twoDimArray[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        private void TwoDimArray(int size)
        {
            if (twoDimArray == null)
            {
                twoDimArray = new int[size, size];
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    twoDimArray[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }

            Console.WriteLine("The elements in the 2-D array are: ");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(twoDimArray[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        private void ThreeDimArray()
        {
            int size = 5;

            if (threeDimArray == null)
            {
                threeDimArray = new int[size, size, size];
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int k = 0; k < size; k++)
                    {
                        threeDimArray[i, j, k] = i + j + k;
                    }
                }
            }

            Console.WriteLine("The elements in the 3-D array are: ");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int k = 0; k < size; k++)
                    {
                        Console.Write(threeDimArray[i, j, k] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        private void ThreeDimArray(int size)
        {
            if (threeDimArray == null)
            {
                threeDimArray = new int[size, size, size];
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int k = 0; k < size; k++)
                    {
                        threeDimArray[i, j, k] = Convert.ToInt32(Console.ReadLine());
                    }
                }
            }

            Console.WriteLine("The elements in the 3-D array are: ");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int k = 0; k < size; k++)
                    {
                        Console.Write(threeDimArray[i, j, k] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        private void OperationStarter()
        {
            int operation;
            do
            {
                Console.WriteLine("Select the operation you want to perform: ");
                Console.WriteLine("  1. Insert \n  2. Delete \n  3. Update \n  4. Search \n  0. To End");
                operation = Convert.ToInt32(Console.ReadLine());

                switch (operation)
                {
                    case 1:
                        InsertOperation();
                        break;

                    case 2:
                        DeleteOperation();
                        break;

                    case 3:
                        UpdateOperation();
                        break;

                    case 4:
                        SearchOperation();
                        break;

                    case 0:

                        break;

                    default:
                        Console.WriteLine("Invalid selection");
                        break;
                }
            } while (operation != 0);
        }
    }
}