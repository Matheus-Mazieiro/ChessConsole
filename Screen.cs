using Board;
using Game;
using System;
using System.Collections.Generic;

namespace ChessConsole
{
    class Screen
    {
        public static void PrintGame(ChessGame game)
        {
            PrintBoard(game.board);
            PrintCapturedPieces(game);
            Console.WriteLine();
            Console.WriteLine("Turn: " + game.turn);
            if (!game.finished)
            {
                Console.WriteLine("Waiting move: " + game.actualPlayer);
                if (game.Check)
                {
                    Console.WriteLine("CHECK!!");
                }
            }
            else
            {
                Console.WriteLine("CHECKMATE!!");
                Console.WriteLine(game.actualPlayer + " WINS!");
            }
        }

        public static void PrintCapturedPieces(ChessGame game)
        {
            Console.WriteLine("Captured pieces: ");

            Console.Write("White: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintSet(game.CapturedPieces(Color.White));
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("Black: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintSet(game.CapturedPieces(Color.Black));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PrintSet(HashSet<Piece> set)
        {
            Console.Write("[");
            foreach (Piece p in set)
            {
                Console.Write(p + " ");
            }
            Console.WriteLine("]");
        }

        public static void PrintBoard(GameBoard board)
        {
            for (int i = 0; i < board.lines; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < board.columns; j++)
                {
                    PressPiece(board.GetPiece(i, j));
                }
                Console.WriteLine();
            }
            char c = 'a';
            Console.Write("-");
            for (int i = 0; i < board.columns; i++)
            {
                Console.Write(" " + c++);
            }
            Console.WriteLine();
        }

        public static void PrintBoard(GameBoard board, bool[,] availablePos)
        {
            ConsoleColor origBg = Console.BackgroundColor;
            ConsoleColor availableBg = ConsoleColor.DarkGray;

            for (int i = 0; i < board.lines; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < board.columns; j++)
                {
                    if (availablePos[i, j])
                        Console.BackgroundColor = availableBg;
                    else Console.BackgroundColor = origBg;
                    PressPiece(board.GetPiece(i, j));
                    Console.BackgroundColor = origBg;
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = origBg;
            char c = 'a';
            Console.Write("-");
            for (int i = 0; i < board.columns; i++)
            {
                Console.Write(" " + c++);
            }            
            Console.WriteLine();
        }

        public static void PressPiece(Piece p)
        {
            if (p == null)
            {
                Console.Write("- ");
            }
            else
            {
                ConsoleColor aux = Console.ForegroundColor;
                if (p.color == Color.White)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(p);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(p);
                }
                Console.ForegroundColor = aux;
                Console.Write(" ");
            }
        }

        public static ChessPosition ReadChessPos()
        {
            string s = Console.ReadLine();
            char column = s[0];
            int line = int.Parse(s[1] + "");
            return new ChessPosition(column, line);
        }

    }
}
