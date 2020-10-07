
namespace Board
{
    class GameBoard
    {
        public int lines { get; set; }
        public int columns { get; set; }
        private Piece[,] pieces;

        public GameBoard(int lines, int columns)
        {
            this.lines = lines;
            this.columns = columns;
            pieces = new Piece[lines, columns];
        }

        public Piece GetPiece(int line, int column)
        {
            return pieces[line, column];
        }
        public Piece GetPiece(Position pos)
        {
            return pieces[pos.line, pos.column];
        }

        public bool ExistingPiece(Position pos)
        {
            ToValidPos(pos);
            return GetPiece(pos) != null;
        }

        public void PlacePiece(Piece p, Position pos)
        {
            if (ExistingPiece(pos))
                throw new BoardException("There's already a piece!");
            pieces[pos.line, pos.column] = p;
            p.myPosition = pos;
        }

        public Piece RemovePiece(Position pos)
        {
            if (GetPiece(pos) == null)
                return null;
            Piece aux = GetPiece(pos);
            aux.myPosition = null;
            pieces[pos.line, pos.column] = null;
            return aux;
        }
        public bool ValidPos(Position pos)
        {
            if (pos.line < 0 || pos.line >= lines || pos.column < 0 || pos.column >= columns)
                return false;
            return true;
        }

        public void ToValidPos(Position pos)
        {
            if (!ValidPos(pos))
                throw new BoardException("Position not valid");
        }
    }
}
