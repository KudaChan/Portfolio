using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private int[] PreDefinedArray()
        {
            int[] arr = new int[5];
            arr[0] = 10;
            arr[1] = 20;
            arr[2] = 30;
            arr[3] = 40;
            arr[4] = 50;

            Console.WriteLine("Pre-Defined Array:");
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i]);
            }

            return arr;
        }

        private int[] UserDefinedArray()
        {
            int n = 0;
            Console.WriteLine("Enter the size of the array:");
            n = Convert.ToInt32(Console.ReadLine());

            int[] arr = new int[n];

            Console.WriteLine("Enter the elements of the array:");
            for (int i = 0; i < n; i++)
            {
                arr[i] = Convert.ToInt32(Console.ReadLine());
            }

            Console.WriteLine("User Defined Array:");
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i]);
            }

            return arr;
        }

        private void ArrayOperations(int arr[])
        {
            Console.WriteLine("Select the operation to perform:");
            Console.WriteLine("1. Insert an element");
            Console.WriteLine("2. Delete an element");
            Console.WriteLine("3. Search an element");
            Console.WriteLine("4. Sort the array");

            int a = Convert.ToInt32(Console.ReadLine());

            switch (a)
            {
                case 1:
                    InsertElement(arr);
                    break;

                case 2:
                    DeleteElement(arr);
                    break;

                case 3:
                    SearchElement(arr);
                    break;

                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }
}