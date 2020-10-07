using Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessConsole.Game
{
    class Knight : Piece
    {
        public Knight(Color color, GameBoard board) : base(color, board)
        {

        }

        public override string ToString()
        {
            return "N";
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

            pos.DefineValues(myPosition.line - 1, myPosition.column - 2);
            if (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
            }
            pos.DefineValues(myPosition.line - 2, myPosition.column - 1);
            if (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
            }

            pos.DefineValues(myPosition.line - 2, myPosition.column + 1 );
            if (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
            }
            pos.DefineValues(myPosition.line - 1, myPosition.column + 2);
            if (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
            }

            pos.DefineValues(myPosition.line + 1, myPosition.column + 2);
            if (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
            }
            pos.DefineValues(myPosition.line + 2, myPosition.column + 1);
            if (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
            }

            pos.DefineValues(myPosition.line + 2, myPosition.column - 1);
            if (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
            }
            pos.DefineValues(myPosition.line + 1, myPosition.column - 2);
            if (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
            }

            return mat;
        }
    }
}
