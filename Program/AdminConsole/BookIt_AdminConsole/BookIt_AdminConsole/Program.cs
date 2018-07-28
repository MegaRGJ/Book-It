using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookIt_AdminConsole
{
    class Program
    {
        /// <summary>
        /// Main routine, just calls main class (converts from static to non-static)
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Program p = new Program();
            p.InitaliseMain();
        }

        private void InitaliseMain()
        {
            Main main = new Main();
            main.InitiateAdminConsole();
        }

        /// <summary>
        /// Waits for user to enter input and returns value
        /// </summary>
        /// <returns></returns>
        public static string Read()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// Writes a string to the console
        /// </summary>
        /// <param name="str"></param>
        public static void Write(string str, bool newLine = true)
        {
            if (newLine)
                Console.WriteLine(">" + str);
            else
                Console.Write(str);
        }

        public static void WriteLines(List<string> str) { foreach (string s in str) { Write(s); System.Threading.Thread.Sleep(100); } }

        /// <summary>
        /// Waits for a user to enter a password, password is hidden (hashed out), and returns the value
        /// </summary>
        /// <returns></returns>
        public static string ReadPassword()
        {
            string pass = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return pass;
        }

        /// <summary>
        /// Clears the console
        /// </summary>
        public static void Clear()
        { Console.Clear(); }
    }
}
