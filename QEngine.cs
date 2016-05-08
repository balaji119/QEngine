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
            Run(args[0], args[1]);
        }

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
            }
        }
    }
}
