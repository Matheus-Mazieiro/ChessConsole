using Board;

namespace Game
{
    class King : Piece
    {
        private ChessGame game;
        public King(Color color, GameBoard board, ChessGame game) : base(color, board)
        {
            this.game = game;
        }

        public override string ToString()
        {
            return "K";
        }

        private bool CanMove(Position pos)
        {
            Piece p = board.GetPiece(pos);
            return p == null || p.color != color;
        }

        private bool CheckRookForRock(Position pos)
        {
            Piece p = board.GetPiece(pos);
            return p != null && p is Rook && p.color == color && p.movCount == 0;
        }

        public override bool[,] AvailableMovs()
        {
            bool[,] mat = new bool[board.lines, board.columns];

            Position pos = new Position(0, 0);

            pos.DefineValues(myPosition.line - 1, myPosition.column);
            if (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
            }

            pos.DefineValues(myPosition.line - 1, myPosition.column - 1);
            if (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
            }

            pos.DefineValues(myPosition.line, myPosition.column - 1);
            if (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
            }

            pos.DefineValues(myPosition.line + 1, myPosition.column - 1);
            if (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
            }

            pos.DefineValues(myPosition.line + 1, myPosition.column);
            if (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
            }

            pos.DefineValues(myPosition.line + 1, myPosition.column + 1);
            if (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
            }

            pos.DefineValues(myPosition.line, myPosition.column + 1);
            if (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
            }

            pos.DefineValues(myPosition.line - 1, myPosition.column + 1);
            if (board.ValidPos(pos) && CanMove(pos))
            {
                mat[pos.line, pos.column] = true;
            }


            //#CASTLES
            if(movCount == 0 && !game.Check)
            {
                //Small
                Position posR = new Position(myPosition.line, myPosition.column + 3);
                if (CheckRookForRock(posR))
                {
                    Position p1 = new Position(myPosition.line, myPosition.column + 1);
                    Position p2 = new Position(myPosition.line, myPosition.column + 2);
                    if (board.GetPiece(p1) == null && board.GetPiece(p2) == null)
                        mat[myPosition.line, myPosition.column + 2] = true;
                }

                //Big
                posR = new Position(myPosition.line, myPosition.column - 4);
                if (CheckRookForRock(posR))
                {
                    Position p1 = new Position(myPosition.line, myPosition.column - 1);
                    Position p2 = new Position(myPosition.line, myPosition.column - 2);
                    Position p3 = new Position(myPosition.line, myPosition.column - 3);
                    if (board.GetPiece(p1) == null && board.GetPiece(p2) == null && board.GetPiece(p3) == null)
                        mat[myPosition.line, myPosition.column - 2] = true;
                }
            }

            return mat;
        }
    }
}
