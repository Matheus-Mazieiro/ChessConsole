using Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessConsole.Game
{
    class Bishop : Piece
    {
        public Bishop(Color color, GameBoard board) : base(color, board)
        {

        }

        public override string ToString()
        {
            return "B";
        }

        private bool CanMove(Position pos)
        {
            Piece p = board.GetPiece(pos);
            return p == null || p.color != color;
        }

        public override bool[,] AvailableMovs()
        {
            bool[,] mat = new bool[board.lines, board.columns];

            Position pos = new Position(0, 0);

            pos.DefineValues(myPosition.line - 1, myPosition.column - 1);
            while (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
                if (board.GetPiece(pos) != null && board.GetPiece(pos).color != color)
                    break;
                pos.DefineValues(pos.line - 1, pos.column - 1);
            }

            pos.DefineValues(myPosition.line - 1, myPosition.column + 1);
            while (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
                if (board.GetPiece(pos) != null && board.GetPiece(pos).color != color)
                    break;
                pos.DefineValues(pos.line - 1, pos.column + 1);
            }

            pos.DefineValues(myPosition.line + 1, myPosition.column - 1);
            while (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
                if (board.GetPiece(pos) != null && board.GetPiece(pos).color != color)
                    break;
                pos.DefineValues(pos.line + 1, pos.column - 1);
            }


            pos.DefineValues(myPosition.line + 1, myPosition.column + 1);
            while (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
                if (board.GetPiece(pos) != null && board.GetPiece(pos).color != color)
                    break;
                pos.DefineValues(pos.line + 1, pos.column + 1);
            }

            return mat;
        }
    }
}