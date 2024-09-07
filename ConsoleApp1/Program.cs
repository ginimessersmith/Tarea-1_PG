// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;
using ConsoleApp1;
using OpenTK;

namespace Tutorial 
{
    class Program
    {
        static void Main(string[] args) {
            Console.WriteLine("hola mundo");

            using (Game game = new(800, 800)) 
            {
                game.Run();
            }
        }
    }
}
