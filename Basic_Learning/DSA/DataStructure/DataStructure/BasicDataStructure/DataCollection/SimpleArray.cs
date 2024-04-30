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
                    array = OneDimArray(size);
                    OperationStarter(array);
                    break;

                case 2:
                    twoDimArray = TwoDimArray(size);
                    OperationStarter(twoDimArray);
                    break;

                case 3:
                    threeDimArray = ThreeDimArray(size);
                    OperationStarter(threeDimArray);
                    break;
            }
        }

        private int[] OneDimArray()
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

            return array;
        }

        private int[] OneDimArray(int size)
        {
            if (array == null)
            {
                array = new int[size];
            }

            for (int i = 0; i < size; i++)
            {
                Console.WriteLine("Enter the element: ");
                array[i] = Convert.ToInt32(Console.ReadLine());
            }

            Console.WriteLine("The elements in the array are: ");
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine(array[i]);
            }

            return array;
        }

        private int[,] TwoDimArray()
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

            return twoDimArray;
        }

        private int[,] TwoDimArray(int size)
        {
            if (twoDimArray == null)
            {
                twoDimArray = new int[size, size];
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.WriteLine($"Enter the element [{i}, {j}]: ");
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

            return twoDimArray;
        }

        private int[,,] ThreeDimArray()
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

            return threeDimArray;
        }

        private int[,,] ThreeDimArray(int size)
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
                        Console.WriteLine($"Enter the element [{i}, {j}, {k}]: ");
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

            return threeDimArray;
        }

        private void OperationStarter(dynamic arr)
        {
            int operation;
            do
            {
                bool functionCallLocation = false;
                Console.WriteLine("Select the operation you want to perform: ");
                Console.WriteLine("  1. Insert at End \n  2. Insert at Specific Location \n  3. Delete \n  4. Search \n  0. To End");
                operation = Convert.ToInt32(Console.ReadLine());

                switch (operation)
                {
                    case 1:
                        InsertAtEndOperation(arr);
                        break;

                    case 2:
                        InsertAtLocation(arr);
                        break;

                    case 3:
                        DeleteOperation(arr, functionCallLocation);
                        break;

                    case 4:
                        SearchOperation(arr);
                        break;

                    case 0:
                        break;

                    default:
                        Console.WriteLine("Invalid selection");
                        break;
                }
            } while (operation != 0);
        }

        private void SearchOperation(dynamic arr)
        {
            if (arr == null)
            {
                Console.WriteLine("Array is empty");
                return;
            }
            if (arr is int[])
            {
                int size = arr.GetLength(0);
                Console.WriteLine("Enter the element to search: ");
                int element = Convert.ToInt32(Console.ReadLine());

                for (int i = 0; i < size; i++)
                {
                    if (arr[i] == element)
                    {
                        Console.WriteLine("Element found at index: " + i);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Element not found");
                    }
                }
            }
            else if (arr is int[,])
            {
                int size = arr.GetLength(0);
                Console.WriteLine("Enter the element to search: ");
                int element = Convert.ToInt32(Console.ReadLine());

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (arr[i, j] == element)
                        {
                            Console.WriteLine("Element found at index: " + i + ", " + j);
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Element not found");
                        }
                    }
                }
            }
            else if (arr is int[,,])
            {
                int size = arr.GetLength(0);
                Console.WriteLine("Enter the element to search: ");
                int element = Convert.ToInt32(Console.ReadLine());

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        for (int k = 0; k < size; k++)
                        {
                            if (arr[i, j, k] == element)
                            {
                                Console.WriteLine("Element found at index: " + i + ", " + j + ", " + k);
                                return;
                            }
                            else
                            {
                                Console.WriteLine("Element not found");
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid array type");
            }

            OperationStarter(arr);
        }

        private void DeleteOperation(dynamic arr, bool loc)
        {
            if (arr == null)
            {
                Console.WriteLine("Array is empty");
                return;
            }
            if (arr is int[])
            {
                int size = arr.GetLength(0);
                Console.WriteLine("Enter the element to delete: ");
                int element = Convert.ToInt32(Console.ReadLine());

                for (int i = 0; i < size; i++)
                {
                    if (arr[i] == element)
                    {
                        for (int j = i; j < size - 1; j++)
                        {
                            arr[j] = arr[j + 1];
                        }
                        size--;
                        Console.WriteLine("Element deleted successfully");
                        if (loc)
                        {
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Element not found");
                    }
                }
            }
            else if (arr is int[,])
            {
                int size = arr.GetLength(0);
                Console.WriteLine("Enter the element to delete: ");
                int element = Convert.ToInt32(Console.ReadLine());

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (arr[i, j] == element)
                        {
                            for (int k = i; k < size - 1; k++)
                            {
                                for (int l = j; l < size - 1; l++)
                                {
                                    arr[k, l] = arr[k + 1, l + 1];
                                }
                            }
                            size--;
                            Console.WriteLine("Element deleted successfully");
                            if (loc)
                            {
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Element not found");
                        }
                    }
                }
            }
            else if (arr is int[,,])
            {
                int size = arr.GetLength(0);
                Console.WriteLine("Enter the element to delete: ");
                int element = Convert.ToInt32(Console.ReadLine());

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        for (int k = 0; k < size; k++)
                        {
                            if (arr[i, j, k] == element)
                            {
                                for (int l = i; l < size - 1; l++)
                                {
                                    for (int m = j; m < size - 1; m++)
                                    {
                                        for (int n = k; n < size - 1; n++)
                                        {
                                            arr[l, m, n] = arr[l + 1, m + 1, n + 1];
                                        }
                                    }
                                }
                                size--;
                                Console.WriteLine("Element deleted successfully");
                                if (loc)
                                {
                                    return;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Element not found");
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid array type");
            }

            OperationStarter(arr);
        }

        private void InsertAtEndOperation(dynamic arr)
        {
            if (arr == null)
            {
                Console.WriteLine("Array is empty");
                return;
            }
            if (arr is int[])
            {
                int size = arr.GetLength(0);
                Console.WriteLine("Enter the element to insert: ");
                int element = Convert.ToInt32(Console.ReadLine());

                if (size == arr.Length)
                {
                    Console.WriteLine("Array is full");
                    Console.WriteLine("Wanna delete any element? (Y/N)");
                    char choice = Convert.ToChar(Console.ReadLine()!);

                    switch (choice)
                    {
                        case 'Y':
                            DeleteOperation(arr, true);
                            arr[size] = element;
                            size++;
                            Console.WriteLine("Element inserted successfully");
                            break;

                        case 'N':
                            return;

                        default:
                            Console.WriteLine("Invalid selection");
                            break;
                    }
                }
                else
                {
                    arr[size] = element;
                    size++;
                    Console.WriteLine("Element inserted successfully");
                }
            }
            else if (arr is int[,])
            {
                int size = arr.GetLength(0);
                Console.WriteLine("Enter the element to insert: ");
                int element = Convert.ToInt32(Console.ReadLine());

                if (size == arr.Length)
                {
                    Console.WriteLine("Array is full");
                    Console.WriteLine("Wanna delete any element? (Y/N)");
                    char choice = Convert.ToChar(Console.ReadLine()!);

                    switch (choice)
                    {
                        case 'Y':
                            DeleteOperation(arr, true);
                            arr[arr.GetLength(0) - 1, arr.GetLength(1) - 1] = element;
                            size++;
                            Console.WriteLine("Element inserted successfully");
                            break;

                        case 'N':
                            return;

                        default:
                            Console.WriteLine("Invalid selection");
                            break;
                    }
                }
                else
                {
                    arr[size, size] = element;
                    size++;
                    Console.WriteLine("Element inserted successfully");
                }
            }
            else if (arr is int[,,])
            {
                int size = arr.GetLength(0);
                Console.WriteLine("Enter the element to insert: ");
                int element = Convert.ToInt32(Console.ReadLine());

                if (size == arr.Length)
                {
                    Console.WriteLine("Array is full");
                    Console.WriteLine("Wanna delete any element? (Y/N)");
                    char choice = Convert.ToChar(Console.ReadLine()!);

                    switch (choice)
                    {
                        case 'Y':
                            DeleteOperation(arr, true);
                            arr[arr.GetLength(0) - 1, arr.GetLength(1) - 1, arr.GetLength(2) - 1] = element;
                            size++;
                            Console.WriteLine("Element inserted successfully");
                            break;

                        case 'N':
                            return;

                        default:
                            Console.WriteLine("Invalid selection");
                            break;
                    }
                }
                else
                {
                    arr[size, size, size] = element;
                    size++;
                    Console.WriteLine("Element inserted successfully");
                }
            }
            else
            {
                Console.WriteLine("Invalid array type");
            }

            OperationStarter(arr);
        }

        private void InsertAtLocation(dynamic arr)
        {
            if (arr == null)
            {
                Console.WriteLine("Array is empty");
                return;
            }
            if (arr is int[])
            {
                bool repeat = false;
                do
                {
                    int size = arr.GetLength(0);
                    Console.WriteLine("Enter the element to insert: ");
                    int element = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Enter the location to insert: ");
                    int location = Convert.ToInt32(Console.ReadLine());

                    if (location > size)
                    {
                        Console.WriteLine("Invalid location");
                        repeat = true;
                    }
                    else if (size == arr.Length)
                    {
                        Console.WriteLine("Array is full");
                        Console.WriteLine("Wanna delete any element? (Y/N)");
                        char choice = Convert.ToChar(Console.ReadLine()!);

                        switch (choice)
                        {
                            case 'Y':
                                DeleteOperation(arr, true);
                                for (int i = size; i > location; i--)
                                {
                                    arr[i] = arr[i - 1];
                                }
                                arr[location] = element;
                                size++;
                                Console.WriteLine("Element inserted successfully");
                                break;

                            case 'N':
                                return;

                            default:
                                Console.WriteLine("Invalid selection");
                                break;
                        }
                        repeat = false;
                    }
                    else
                    {
                        for (int i = size; i > location; i--)
                        {
                            arr[i] = arr[i - 1];
                        }
                        arr[location] = element;
                        size++;
                        Console.WriteLine("Element inserted successfully");
                    }
                } while (repeat);
            }
            else if (arr is int[,])
            {
                bool repeat = false;
                do
                {
                    int size = arr.GetLength(0);
                    Console.WriteLine("Enter the element to insert: ");
                    int element = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Enter the location to insert: ");
                    int colLocation = Convert.ToInt32(Console.ReadLine());
                    int rowLocation = Convert.ToInt32(Console.ReadLine());

                    if (colLocation > size || rowLocation > size)
                    {
                        Console.WriteLine("Invalid location");
                        repeat = true;
                    }
                    else if (size == arr.Length)
                    {
                        Console.WriteLine("Array is full");
                        Console.WriteLine("Wanna delete any element? (Y/N)");
                        char choice = Convert.ToChar(Console.ReadLine()!);

                        switch (choice)
                        {
                            case 'Y':
                                DeleteOperation(arr, true);
                                for (int i = size; i > colLocation; i--)
                                {
                                    for (int j = size; j > rowLocation; j--)
                                    {
                                        arr[i, j] = arr[i - 1, j - 1];
                                    }
                                }
                                arr[colLocation, rowLocation] = element;
                                size++;
                                Console.WriteLine("Element inserted successfully");
                                break;

                            case 'N':
                                return;

                            default:
                                Console.WriteLine("Invalid selection");
                                break;
                        }
                        repeat = false;
                    }
                    else
                    {
                        for (int i = size; i > colLocation; i--)
                        {
                            for (int j = size; j > rowLocation; j--)
                            {
                                arr[i, j] = arr[i - 1, j - 1];
                            }
                        }
                        arr[colLocation, rowLocation] = element;
                        size++;
                        Console.WriteLine("Element inserted successfully");
                    }
                } while (repeat);
            }
            else if (arr is int[,,])
            {
                bool repeat = false;
                do
                {
                    int size = arr.GetLength(0);
                    Console.WriteLine("Enter the element to insert: ");
                    int element = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Enter the location to insert: ");
                    int matrix = Convert.ToInt32(Console.ReadLine());
                    int colLocation = Convert.ToInt32(Console.ReadLine());
                    int rowLocation = Convert.ToInt32(Console.ReadLine());

                    if (matrix > size || colLocation > size || rowLocation > size)
                    {
                        Console.WriteLine("Invalid location");
                        repeat = true;
                    }
                    else if (size == arr.Length)
                    {
                        Console.WriteLine("Array is full");
                        Console.WriteLine("Wanna delete any element? (Y/N)");
                        char choice = Convert.ToChar(Console.ReadLine()!);

                        switch (choice)
                        {
                            case 'Y':
                                DeleteOperation(arr, true);
                                for (int i = size; i > matrix; i--)
                                {
                                    for (int j = size; j > colLocation; j--)
                                    {
                                        for (int k = size; k > rowLocation; k--)
                                        {
                                            arr[i, j, k] = arr[i - 1, j - 1, k - 1];
                                        }
                                    }
                                }
                                arr[matrix, colLocation, rowLocation] = element;
                                size++;
                                Console.WriteLine("Element inserted successfully");
                                break;

                            case 'N':
                                return;

                            default:
                                Console.WriteLine("Invalid selection");
                                break;
                        }
                        repeat = false;
                    }
                    else
                    {
                        for (int i = size; i > matrix; i--)
                        {
                            for (int j = size; j > colLocation; j--)
                            {
                                for (int k = size; k > rowLocation; k--)
                                {
                                    arr[i, j, k] = arr[i - 1, j - 1, k - 1];
                                }
                            }
                        }
                        arr[matrix, colLocation, rowLocation] = element;
                        size++;
                        Console.WriteLine("Element inserted successfully");
                    }
                } while (repeat);
            }
            else
            {
                Console.WriteLine("Invalid array type");
            }

            OperationStarter(arr);
        }
    }
}