using DataStructure.CoreDataStructure;

namespace Program
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            int a = 0;

            Console.WriteLine("Select the task to perform:");
            Console.WriteLine("1. Array Implementation");

            a = Convert.ToInt32(Console.ReadLine());

            switch (a)
            {
                case 1:
                    ArrayImplementation arrayImplementation = new();
                    arrayImplementation.ArrayStarter();
                    break;

                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }
}