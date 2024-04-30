using DataStructure.CoreDataStructure.DataCollection.SimpleArray;

namespace Program
{
    internal class Program
    {
        private static void DataCollection()
        {
            Console.WriteLine("In C#, We have 4 types of collections methods:");
            Console.WriteLine("  1. Array \n  2. Jagged Array \n  3. List \n  4. Dictionary");
            Console.WriteLine("Select the collection method you want to perform: ");
            int cm = Convert.ToInt32(Console.ReadLine());

            switch (cm)
            {
                case 1:
                    SimpleArray simpleArray = new SimpleArray();
                    simpleArray.SimpleArrayStarter();
                    break;

                case 2:
                    Console.WriteLine("Jagged Array");
                    break;

                case 3:
                    Console.WriteLine("List");
                    break;

                case 4:
                    Console.WriteLine("Dictionary");
                    break;

                default:
                    Console.WriteLine("Invalid selection");
                    break;
            }
        }

        public static void Main(string[] args)
        {
            DataCollection();
        }
    }
}