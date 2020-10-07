using System;
using Board;
using Game;

namespace ChessConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            ChessGame game = new ChessGame();

            while (!game.finished)
            {
                try
                {
                    Console.Clear();
                    Screen.PrintGame(game);

                    Console.WriteLine();
                    Console.Write("Origin: ");
                    Position orig = Screen.ReadChessPos().ToPosition();
                    game.ValidOrigPos(orig);

                    bool[,] possiblePositons = game.board.GetPiece(orig).AvailableMovs();

                    Console.Clear();
                    Screen.PrintBoard(game.board, possiblePositons);

                    Console.Write("Destination: ");
                    Position dest = Screen.ReadChessPos().ToPosition();
                    game.ValidDestPos(orig, dest);

                    game.DoPlay(orig, dest);
                }
                catch (BoardException e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
                Console.Clear();
                Screen.PrintGame(game);
            }



        }
    }
}
