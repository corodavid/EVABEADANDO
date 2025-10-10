using System;
using Connect4.Model;
using Connect4.Persistance;

namespace Connect4.Model
{
    class Program
    {
        static void Main(string[] args)
        {
            int n;
            int.TryParse(Console.ReadLine(), out n);   
            GameTable gameTable = new GameTable(n);
            for(int i = 0; i < n; i++) 
            {
                for(int j = 0;j < n; j++)
                {
                    Console.Write($"({i}, {j}){gameTable[i, j]} ");
                }
                Console.WriteLine();
            }

            GameModel model = new GameModel(gameTable);

            while (model.State == GameState.RUNNING)
            {
                int col;
                int.TryParse(Console.ReadLine(), out col);
                model.Round(col);
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        string temp = (gameTable[i, j] == FieldStatus.NONE) ? (FieldStatus.NONE.ToString()) : " " + gameTable[i, j].ToString() + "  ";
                        Console.Write($"{temp} ");
                    }
                    Console.WriteLine();
                }
            }

            /*
            int x, y;
            int.TryParse(Console.ReadLine(), out x);
            int.TryParse(Console.ReadLine(), out y);


            for (int i = 0; i < x; i++)
            {
                model.TryInsert(y);
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write($"({i}, {j}){gameTable[i, j]} ");
                }
                Console.WriteLine();
            }

            model.checkDiagonals(y);

            Console.WriteLine($"Up to Down: {model.startingPointUpToDown(x, y)}");
            Console.WriteLine($"Down to Up: {model.startingPointDownToUp(x, y)}"); 
            */

        }
    }
}