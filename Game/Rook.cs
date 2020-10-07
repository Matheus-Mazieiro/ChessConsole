using Board;
using Microsoft.VisualBasic;
using System;

namespace Game
{
    class Rook : Piece
    {

        public Rook(Color color, GameBoard board) : base(color, board)
        {

        }

        public override string ToString()
        {
            return "R";
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

            pos.DefineValues(myPosition.line - 1, myPosition.column);
            while(board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
                if (board.GetPiece(pos) != null && board.GetPiece(pos).color != color)
                    break;
                pos.line--;
            }

            pos.DefineValues(myPosition.line + 1, myPosition.column);
            while (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
                if (board.GetPiece(pos) != null && board.GetPiece(pos).color != color)
                    break;
                pos.line++;
            }

            pos.DefineValues(myPosition.line, myPosition.column - 1);
            while (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
                if (board.GetPiece(pos) != null && board.GetPiece(pos).color != color)
                    break;
                pos.column--;
            }

            pos.DefineValues(myPosition.line, myPosition.column + 1);
            while (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
                if (board.GetPiece(pos) != null && board.GetPiece(pos).color != color)
                    break;
                pos.column++;
            }

            return mat;
        }

    }
}
