//*****************************************************************************
// QEngine.cs - Entry point of the application.
//*****************************************************************************
using System;
using System.IO;

namespace QEngine
{
    class QEngine
    {
        static void Main(string[] args)
        {
            try
            {
                Run(args[0], args[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Loads the given file and executes commands continuously untill user quits.
        /// </summary>
        /// <param name="filePath">CSV file which is to be represented as database</param>
        /// <param name="tableName">Name of the database table</param>
        static void Run(string filePath, string tableName)
        {
            var lines = File.ReadLines(filePath);
            var db = Data.CreateDB(lines, tableName);

            while (true)
            {
                Console.Write(">> ");
                var cmd = Console.ReadLine();
                if (cmd == "exit" || cmd == "quit") return;
                
                var result = db.ProcessCommand(cmd);
                foreach (var item in result)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine();
            }
        }
    }
}
