using Board;
using Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessConsole.Game
{
    class Pawn : Piece
    {
        private ChessGame game;

        public Pawn(Color color, GameBoard board, ChessGame game) : base(color, board)
        {
            this.game = game;
        }

        public override string ToString()
        {
            return "p";
        }

        private bool EnemyBlocks(Position pos)
        {
            Piece p = board.GetPiece(pos);
            return p != null && p.color != color;
        }

        private bool Empty(Position pos)
        {
            return board.GetPiece(pos) == null;
        }

        public override bool[,] AvailableMovs()
        {
            bool[,] mat = new bool[board.lines, board.columns];

            Position pos = new Position(0, 0);

            if (color == Color.White)
            {
                pos.DefineValues(myPosition.line - 1, myPosition.column);
                if (board.ValidPos(pos) && Empty(pos))
                    mat[pos.line, pos.column] = true;

                pos.DefineValues(myPosition.line - 2, myPosition.column);
                if (board.ValidPos(pos) && Empty(pos) && movCount == 0)
                    mat[pos.line, pos.column] = true;

                pos.DefineValues(myPosition.line - 1, myPosition.column - 1);
                if (board.ValidPos(pos) && EnemyBlocks(pos))
                    mat[pos.line, pos.column] = true;

                pos.DefineValues(myPosition.line - 1, myPosition.column + 1);
                if (board.ValidPos(pos) && EnemyBlocks(pos))
                    mat[pos.line, pos.column] = true;

                //# EN PASSANT
                if (myPosition.line == 3)
                {
                    Position left = new Position(myPosition.line, myPosition.column - 1);
                    if(board.ValidPos(left) && EnemyBlocks(left) && board.GetPiece(left) == game.EnPassantVunerable)
                        mat[left.line - 1, left.column] = true;

                    Position right = new Position(myPosition.line, myPosition.column + 1);
                    if (board.ValidPos(right) && EnemyBlocks(right) && board.GetPiece(right) == game.EnPassantVunerable)
                        mat[right.line - 1, right.column] = true;
                }
            } else
            {
                pos.DefineValues(myPosition.line + 1, myPosition.column);
                if (board.ValidPos(pos) && Empty(pos))
                    mat[pos.line, pos.column] = true;

                pos.DefineValues(myPosition.line + 2, myPosition.column);
                if (board.ValidPos(pos) && Empty(pos) && movCount == 0)
                    mat[pos.line, pos.column] = true;

                pos.DefineValues(myPosition.line + 1, myPosition.column - 1);
                if (board.ValidPos(pos) && EnemyBlocks(pos))
                    mat[pos.line, pos.column] = true;

                pos.DefineValues(myPosition.line + 1, myPosition.column + 1);
                if (board.ValidPos(pos) && EnemyBlocks(pos))
                    mat[pos.line, pos.column] = true;

                //# EN PASSANT
                if (myPosition.line == 4)
                {
                    Position left = new Position(myPosition.line, myPosition.column - 1);
                    if (board.ValidPos(left) && EnemyBlocks(left) && board.GetPiece(left) == game.EnPassantVunerable)
                        mat[left.line + 1, left.column] = true;

                    Position right = new Position(myPosition.line, myPosition.column + 1);
                    if (board.ValidPos(right) && EnemyBlocks(right) && board.GetPiece(right) == game.EnPassantVunerable)
                        mat[right.line + 1, right.column] = true;
                }
            }

            return mat;
        }
    }
}
