namespace DataStructure.CoreDataStructure
{
    internal class ArrayImplementation
    {
        public int ArrayStarter()
        {
            int a = 0;

            Console.WriteLine("Select the array type to imeplement:");
            Console.WriteLine("1. Pre-Defined Array");
            Console.WriteLine("2. User Defined Array");

            a = Convert.ToInt32(Console.ReadLine());

            switch (a)
            {
                case 1:
                    PreDefinedArray();
                    break;

                case 2:
                    UserDefinedArray();
                    break;

                default:
                    Console.WriteLine("Invalid input");
                    break;
            }

            return 0;
        }

        private void PreDefinedArray()
        {
            int[] arr1 = new int[5] { 1, 2, 3, 4, 5 };
            int[] arr2 = new int[5] { 6, 7, 8, 9, 10 };
            int[] arr3 = new int[5] { 11, 12, 13, 14, 15 };
            int[] arr4 = new int[5] { 16, 17, 18, 19, 20 };

            int[] arr1D = new int[5];
            int[][] arr2D = new int[2][];
            int[][][] arr3D = new int[2][][];

            Console.WriteLine("Select the type of Array:");
            Console.WriteLine("1. One Dimensional Array");
            Console.WriteLine("2. Two Dimensional Array");
            Console.WriteLine("3. Three Dimensional Array");

            int a = Convert.ToInt32(Console.ReadLine());

            switch (a)
            {
                case 1:
                    arr1D = OneDimArray(arr1);
                    ArrayOperationPermission(arr1D);
                    break;

                case 2:
                    arr2D = TwoDimArray(arr1, arr2);
                    ArrayOperationPermission(arr2D);
                    break;

                case 3:
                    arr3D = ThreeDimArray(arr1, arr2, arr3, arr4);
                    ArrayOperationPermission(arr3D);
                    break;

                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }

        private void UserDefinedArray()
        {
            Console.WriteLine("Select the type of Array:");
            Console.WriteLine("1. One Dimensional Array");
            Console.WriteLine("2. Two Dimensional Array");
            Console.WriteLine("3. Three Dimensional Array");

            int a = Convert.ToInt32(Console.ReadLine());

            switch (a)
            {
                case 1:
                    Console.WriteLine("Enter the size of the array:");
                    int size = Convert.ToInt32(Console.ReadLine());
                    int[] arr1D = OneDimArray(size);
                    ArrayOperationPermission(arr1D);
                    break;

                case 2:
                    Console.WriteLine("Enter the size of the array[size1, size2]:");
                    Console.WriteLine("Size 1:");
                    int size1 = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Size 2:");
                    int size2 = Convert.ToInt32(Console.ReadLine());
                    int[][] arr2D = TwoDimArray(size1, size2);
                    ArrayOperationPermission(arr2D);
                    break;

                case 3:
                    Console.WriteLine("Enter the size of the array:");
                    Console.WriteLine("Size 1:");
                    int sizeA = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Size 2:");
                    int sizeB = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Size 3:");
                    int sizeC = Convert.ToInt32(Console.ReadLine());
                    int[][][] arr3D = ThreeDimArray(sizeA, sizeB, sizeC);
                    ArrayOperationPermission(arr3D);
                    break;

                default:
                    Console.WriteLine("Invalid input");
                    break;
            }

            Console.WriteLine("Array Operations:");
            Console.WriteLine("1. Insert an element at End.");
            Console.WriteLine("2. Insert an element at Particular Position.");
            Console.WriteLine("3. Delete an element.");
            Console.WriteLine("4. Search an element.");
        }

        private int[] OneDimArray(int[] arr)
        {
            Console.WriteLine("One Dimensional Array:");
            Console.WriteLine("Elements in the array are:");
            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine(arr[i]);
            }
            return arr;
        }

        private int[] OneDimArray(int size)
        {
            int[] arr = new int[size];
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine("Enter the element:");
                arr[i] = Convert.ToInt32(Console.ReadLine());
            }

            Console.WriteLine("Elements in the array are:");
            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine(arr[i]);
            }
            return arr;
        }

        private int[][] TwoDimArray(int[] arr1, int[] arr2)
        {
            int[][] arr = new int[2][];

            for (int i = 0; i < arr1.Length; i++)
            {
                arr[0][i] = arr1[i];
                arr[1][i] = arr2[i];
            }

            Console.WriteLine("Elements in the 2D-Array are:");
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr[i].Length; j++)
                {
                    Console.WriteLine(arr[i][j]);
                }
            }

            return arr;
        }

        private int[][] TwoDimArray(int size1, int size2)
        {
            int[][] arr = new int[2][];

            for (int i = 0; i < 2; i++)
            {
                arr[i] = new int[size1];
                for (int j = 0; j < size1; j++)
                {
                    Console.WriteLine("Enter the element:");
                    arr[i][j] = Convert.ToInt32(Console.ReadLine());
                }
            }

            Console.WriteLine("Elements in the 2D-Array are:");
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr[i].Length; j++)
                {
                    Console.WriteLine(arr[i][j]);
                }
            }

            return arr;
        }

        private int[][][] ThreeDimArray(int[] arr1, int[] arr2, int[] arr3, int[] arr4)
        {
            int[][][] arr = new int[2][][];

            for (int i = 0; i < arr1.Length; i++)
            {
                arr[0][0][i] = arr1[i];
                arr[0][1][i] = arr2[i];
                arr[1][0][i] = arr3[i];
                arr[1][1][i] = arr4[i];
            }

            Console.WriteLine("Elements in the 3D-Array are:");
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr[i].Length; j++)
                {
                    for (int k = 0; k < arr[i][j].Length; k++)
                    {
                        Console.WriteLine(arr[i][j][k]);
                    }
                }
            }

            return arr;
        }

        private int[][][] ThreeDimArray(int size1, int size2, int size3)
        {
            int[][][] arr = new int[2][][];

            for (int i = 0; i < 2; i++)
            {
                arr[i] = new int[size1][];
                for (int j = 0; j < size1; j++)
                {
                    arr[i][j] = new int[size2];
                    for (int k = 0; k < size2; k++)
                    {
                        Console.WriteLine("Enter the element:");
                        arr[i][j][k] = Convert.ToInt32(Console.ReadLine());
                    }
                }
            }

            Console.WriteLine("Elements in the 3D-Array are:");
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr[i].Length; j++)
                {
                    for (int k = 0; k < arr[i][j].Length; k++)
                    {
                        Console.WriteLine(arr[i][j][k]);
                    }
                }
            }

            return arr;
        }

        private void ArrayOperationPermission(dynamic arr = null!)
        {
            if (arr != null)
            {
                Console.WriteLine("Want to perform any operation on the array [y/n]:");
                string b = Console.ReadLine()!;

                switch (b)
                {
                    case "y":
                        ArrayOperations(arr);
                        break;

                    case "n":
                        break;

                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Passed array is empty");
            }
        }

        private void ArrayOperations(dynamic arr = null!)
        {
            if (arr != null)
            {
                Console.WriteLine("Array Operations:");
                Console.WriteLine("1. Insert an element at End.");
                Console.WriteLine("2. Insert an element at Particular Position.");
                Console.WriteLine("3. Delete an element.");
                Console.WriteLine("4. Search an element.");
                Console.WriteLine("0. Exit the operation.");

                int a = Convert.ToInt32(Console.ReadLine());

                switch (a)
                {
                    case 1:
                        InsertElementAtEnd(arr);
                        break;

                    case 2:
                        InsertElementAtPosition(arr);
                        break;

                    case 3:
                        DeleteElement(arr);
                        break;

                    case 4:
                        SearchElement(arr);
                        break;

                    case 0:
                        break;

                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Passed array is empty");
            }
        }

        private void InsertElementAtEnd(dynamic arr = null!)
        {
            if (arr != null)
            {
                if (arr is int[])
                {
                    Console.WriteLine("Enter the element to insert:");
                    int element = Convert.ToInt32(Console.ReadLine());
                    int[] temp = new int[arr.Length + 1];
                    for (int i = 0; i < arr.Length; i++)
                    {
                        temp[i] = arr[i];
                    }
                    temp[arr.Length] = element;
                    arr = temp;

                    Console.WriteLine("Elements in the array after insertion are:");
                    for (int i = 0; i < arr.Length; i++)
                    {
                        Console.WriteLine(arr[i]);
                    }
                }
                else if (arr is int[][])
                {
                    Console.WriteLine("Enter the element to insert:");
                    int element = Convert.ToInt32(Console.ReadLine());
                    int[][] temp = new int[arr.Length][];
                    for (int i = 0; i < arr.Length; i++)
                    {
                        temp[i] = arr[i];
                    }
                    temp[arr.Length] = new int[1] { element };
                    arr = temp;

                    Console.WriteLine("Elements in the 2D-Array after insertion are:");
                    for (int i = 0; i < arr.Length; i++)
                    {
                        for (int j = 0; j < arr[i].Length; j++)
                        {
                            Console.WriteLine(arr[i][j]);
                        }
                    }
                }
                else if (arr is int[][][])
                {
                    Console.WriteLine("Enter the element to insert:");
                    int element = Convert.ToInt32(Console.ReadLine());
                    int[][][] temp = new int[arr.Length][][];
                    for (int i = 0; i < arr.Length; i++)
                    {
                        temp[i] = arr[i];
                    }
                    temp[arr.Length] = new int[1][];
                    temp[arr.Length][0] = new int[1] { element };
                    arr = temp;

                    Console.WriteLine("Elements in the 3D-Array after insertion are:");
                    for (int i = 0; i < arr.Length; i++)
                    {
                        for (int j = 0; j < arr[i].Length; j++)
                        {
                            for (int k = 0; k < arr[i][j].Length; k++)
                            {
                                Console.WriteLine(arr[i][j][k]);
                            }
                        }
                    }
                }

                Console.WriteLine("Element inserted successfully");

                ArrayOperations(arr);
            }
            else
            {
                Console.WriteLine("Passed array is empty.");
            }
        }

        private void InsertElementAtPosition(dynamic arr = null!)
        {
            if (arr != null)
            {
                if (arr is int[])
                {
                    Console.WriteLine("Enter the element to insert:");
                    int element = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter the position to insert:");
                    int position = Convert.ToInt32(Console.ReadLine());

                    int[] temp = new int[arr.Length];

                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (i == (position - 1))
                        {
                            temp[i] = element;
                        }
                        if (i >= position)
                        {
                            temp[i] = arr[i - 1];
                        }
                        else
                        {
                            temp[i] = arr[i];
                        }
                    }

                    arr = temp;

                    Console.WriteLine("Elements in the array after insertion are:");
                    for (int i = 0; i < arr.Length; i++)
                    {
                        Console.WriteLine(arr[i]);
                    }
                }
                else if (arr is int[][])
                {
                    Console.WriteLine("Enter the element to insert:");
                    int element = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter the position to insert arr[position 1][position 2]:");
                    Console.WriteLine("Position 1:");
                    int position1 = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Position 2:");
                    int position2 = Convert.ToInt32(Console.ReadLine());

                    int[][] temp = new int[arr.Length][];

                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (i == (position1 - 1))
                        {
                            temp[i] = new int[arr[i].Length + 1];
                            for (int j = 0; j < arr[i].Length; j++)
                            {
                                if (j == (position2 - 1))
                                {
                                    temp[i][j] = element;
                                }
                                if (j >= position2)
                                {
                                    temp[i][j] = arr[i][j - 1];
                                }
                                else
                                {
                                    temp[i][j] = arr[i][j];
                                }
                            }
                        }
                        if (i >= position1)
                        {
                            temp[i] = arr[i - 1];
                        }
                        else
                        {
                            temp[i] = arr[i];
                        }
                    }

                    arr = temp;

                    Console.WriteLine("Elements in the 2D-Array after insertion are:");
                    for (int i = 0; i < arr.Length; i++)
                    {
                        for (int j = 0; j < arr[i].Length; j++)
                        {
                            Console.WriteLine(arr[i][j]);
                        }
                    }
                }
                else if (arr is int[][][])
                {
                    Console.WriteLine("Enter the element to insert:");
                    int element = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter the position to insert arr[position 1][position 2][position 3]:");
                    Console.WriteLine("Position 1:");
                    int position1 = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Position 2:");
                    int position2 = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Position 3:");
                    int position3 = Convert.ToInt32(Console.ReadLine());

                    int[][][] temp = new int[arr.Length][][];

                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (i == (position1 - 1))
                        {
                            temp[i] = new int[arr[i].Length + 1][];
                            for (int j = 0; j < arr[i].Length; j++)
                            {
                                if (j == (position2 - 1))
                                {
                                    temp[i][j] = new int[arr[i][j].Length + 1];
                                    for (int k = 0; k < arr[i][j].Length; k++)
                                    {
                                        if (k == (position3 - 1))
                                        {
                                            temp[i][j][k] = element;
                                        }
                                        if (k >= position3)
                                        {
                                            temp[i][j][k] = arr[i][j][k - 1];
                                        }
                                        else
                                        {
                                            temp[i][j][k] = arr[i][j][k];
                                        }
                                    }
                                }
                                if (j >= position2)
                                {
                                    temp[i][j] = arr[i][j - 1];
                                }
                                else
                                {
                                    temp[i][j] = arr[i][j];
                                }
                            }
                        }
                        if (i >= position1)
                        {
                            temp[i] = arr[i - 1];
                        }
                        else
                        {
                            temp[i] = arr[i];
                        }
                    }

                    arr = temp;

                    Console.WriteLine("Elements in the 3D-Array after insertion are:");
                    for (int i = 0; i < arr.Length; i++)
                    {
                        for (int j = 0; j < arr[i].Length; j++)
                        {
                            for (int k = 0; k < arr[i][j].Length; k++)
                            {
                                Console.WriteLine(arr[i][j][k]);
                            }
                        }
                    }
                }

                Console.WriteLine("Element inserted successfully");
                ArrayOperations(arr);
            }
            else
            {
                Console.WriteLine("Passed array is empty.");
            }
        }

        private void DeleteElement(dynamic arr = null!)
        {
            if (arr != null)
            {
                if (arr is int[])
                {
                    Console.WriteLine("Enter the element to delete:");
                    int element = Convert.ToInt32(Console.ReadLine());
                    int[] temp = new int[arr.Length];

                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i] != element)
                        {
                            temp[i] = arr[i];
                        }
                        else
                        {
                            for (int j = i; j < arr.Length - 1; j++)
                            {
                                temp[j] = arr[j + 1];
                            }
                            break;
                        }
                    }

                    arr = temp;

                    Console.WriteLine("Elements in the array after deletion are:");
                    for (int i = 0; i < arr.Length; i++)
                    {
                        Console.WriteLine(arr[i]);
                    }
                }
                else if (arr is int[][])
                {
                    Console.WriteLine("Enter the element to delete:");
                    int element = Convert.ToInt32(Console.ReadLine());
                    int[][] temp = new int[arr.Length][];

                    for (int i = 0; i < arr.Length; i++)
                    {
                        for (int j = 0; j < arr[i].Length; j++)
                        {
                            if (arr[i][j] != element)
                            {
                                temp[i][j] = arr[i][j];
                            }
                            else
                            {
                                for (int k = j; k < arr[i].Length - 1; k++)
                                {
                                    temp[i][k] = arr[i][k + 1];
                                }
                                break;
                            }
                        }
                    }

                    arr = temp;

                    Console.WriteLine("Elements in the 2D-Array after deletion are:");
                    for (int i = 0; i < arr.Length; i++)
                    {
                        for (int j = 0; j < arr[i].Length; j++)
                        {
                            Console.WriteLine(arr[i][j]);
                        }
                    }
                }
                else if (arr is int[][][])
                {
                    Console.WriteLine("Enter the element to delete:");
                    int element = Convert.ToInt32(Console.ReadLine());
                    int[][][] temp = new int[arr.Length][][];

                    for (int i = 0; i < arr.Length; i++)
                    {
                        for (int j = 0; j < arr[i].Length; j++)
                        {
                            for (int k = 0; k < arr[i][j].Length; k++)
                            {
                                if (arr[i][j][k] != element)
                                {
                                    temp[i][j][k] = arr[i][j][k];
                                }
                                else
                                {
                                    for (int l = k; l < arr[i][j].Length - 1; l++)
                                    {
                                        temp[i][j][l] = arr[i][j][l + 1];
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    arr = temp;

                    Console.WriteLine("Elements in the 3D-Array after deletion are:");
                    for (int i = 0; i < arr.Length; i++)
                    {
                        for (int j = 0; j < arr[i].Length; j++)
                        {
                            for (int k = 0; k < arr[i][j].Length; k++)
                            {
                                Console.WriteLine(arr[i][j][k]);
                            }
                        }
                    }
                }
                Console.WriteLine("Element deleted successfully");
                ArrayOperations(arr);
            }
            else
            {
                Console.WriteLine("Passed array is empty.");
            }
        }
    }
}