
using System.Reflection.Metadata.Ecma335;

namespace Board
{
    abstract class Piece
    {
        public Position myPosition { get; set; }
        public Color color { get; protected set; }

        public int movCount { get; protected set; }
        public GameBoard board { get; protected set; }

        public Piece(Color color, GameBoard board)
        {
            this.myPosition = null;
            this.color = color;
            this.board = board;
        }

        public void IncreaseMovCount()
        {
            movCount++;
        }

        public void DecreaseMovCount()
        {
            movCount--;
        }


        public bool HasAvailableMoves()
        {
            bool[,] mat = AvailableMovs();
            for (int i = 0; i < board.lines; i++)
            {
                for (int j = 0; j < board.columns; j++)
                {
                    if (mat[i, j])
                        return true;
                }
            }
            return false;
        }

        public bool CanMoveTo(Position pos)
        {
            return AvailableMovs()[pos.line, pos.column];
        }

        public abstract bool[,] AvailableMovs();
    }
}
