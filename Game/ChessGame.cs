using System;
using System.Collections.Generic;
using System.Text;
using Board;
using ChessConsole.Game;
using Microsoft.VisualBasic;

namespace Game
{
    class ChessGame
    {
        public GameBoard board { get; private set; }
        public int turn { get; private set; }
        public Color actualPlayer { get; private set; }
        public bool finished { get; private set; }
        private HashSet<Piece> pieces;
        private HashSet<Piece> captured;
        public bool Check { get; set; }
        public Piece EnPassantVunerable { get; private set; }

        public ChessGame()
        {
            board = new GameBoard(8, 8);
            turn = 1;
            actualPlayer = Color.White;
            finished = false;
            pieces = new HashSet<Piece>();
            captured = new HashSet<Piece>();
            PlacePieces();
            EnPassantVunerable = null;
        }

        public Piece Moves(Position orig, Position destination)
        {
            Piece p = board.RemovePiece(orig);
            p.IncreaseMovCount();
            Piece captured = board.RemovePiece(destination);
            board.PlacePiece(p, destination);
            if (captured != null)
            {
                this.captured.Add(captured);
            }

            //#CASTLES
            if (p is King && destination.column == orig.column + 2)
            {
                Position rookOrig = new Position(orig.line, orig.column + 3);
                Position rookDest = new Position(orig.line, orig.column + 1);
                Piece r = board.RemovePiece(rookOrig);
                r.IncreaseMovCount();
                board.PlacePiece(r, rookDest);
            }
            if (p is King && destination.column == orig.column - 2)
            {
                Position rookOrig = new Position(orig.line, orig.column - 4);
                Position rookDest = new Position(orig.line, orig.column - 1);
                Piece r = board.RemovePiece(rookOrig);
                r.IncreaseMovCount();
                board.PlacePiece(r, rookDest);
            }

            //# EN PASSANT
            if (p is Pawn && orig.column != destination.column && captured == null)
            {
                Position posP = new Position(orig.line, destination.column);
                captured = board.RemovePiece(posP);
                this.captured.Add(captured);
            }


            return captured;
        }

        public void UndoMove(Position orig, Position dest, Piece captured)
        {
            Piece p = board.RemovePiece(dest);
            p.DecreaseMovCount();
            board.PlacePiece(p, orig);
            if (captured != null)
            {
                board.PlacePiece(captured, dest);
                this.captured.Remove(captured);
            }

            //#CASTLES
            if (p is King && dest.column == orig.column + 2)
            {
                Position rookOrig = new Position(orig.line, orig.column + 3);
                Position rookDest = new Position(orig.line, orig.column + 1);
                Piece r = board.RemovePiece(rookDest);
                r.DecreaseMovCount();
                board.PlacePiece(r, rookOrig);
            }
            if (p is King && dest.column == orig.column - 2)
            {
                Position rookOrig = new Position(orig.line, orig.column - 4);
                Position rookDest = new Position(orig.line, orig.column - 1);
                Piece r = board.RemovePiece(rookDest);
                r.DecreaseMovCount();
                board.PlacePiece(r, rookOrig);
            }

            //# EN PASSANT
            if (p is Pawn && orig.column != dest.column && captured == EnPassantVunerable)
            {
                Piece pawn = board.RemovePiece(dest);
                Position posP = new Position(orig.line, dest.column);
                board.PlacePiece(pawn, posP);
                this.captured.Remove(captured);
            }
        }

        public void DoPlay(Position orig, Position dest)
        {
            Piece captured = Moves(orig, dest);
            if (IsInCheck(actualPlayer))
            {
                UndoMove(orig, dest, captured);
                throw new BoardException("You cannot check yourself!");
            }

            Piece p = board.GetPiece(dest);

            //#PROMOTION
            if(p is Pawn)
            {
                if((p.color == Color.White && dest.line == 0) || (p.color == Color.Black && dest.line == 7))
                {
                    p = board.RemovePiece(dest);
                    pieces.Remove(p);

                    Console.Write("Chose promotion piece (R/N/B/Q): ");
                    Piece upgradeTo;
                    char c = Console.ReadLine()[0];
                    switch (c)
                    {
                        case 'R':
                            upgradeTo = new Rook(p.color, board);
                            break;
                        case 'N':
                            upgradeTo = new Knight(p.color, board);
                            break;
                        case 'B':
                            upgradeTo = new Bishop(p.color, board);
                            break;
                        case 'Q':
                            upgradeTo = new Queen(p.color, board);
                            break;
                        default:
                            upgradeTo = new Queen(p.color, board);
                            break;
                    }
                    board.PlacePiece(upgradeTo, dest);
                    pieces.Add(upgradeTo);
                }
            }

            if (IsInCheck(Enemy(actualPlayer)))
            {
                Check = true;
            }
            else Check = false;

            if (CheckMate(Enemy(actualPlayer)))
                finished = true;

            turn++;
            ChangePlayer();

            //#EN PASSANT
            if (p is Pawn && dest.line == orig.line - 2 || dest.line == orig.line + 2)
                EnPassantVunerable = p;
            else EnPassantVunerable = null;
        }

        public void ValidOrigPos(Position pos)
        {
            if (board.GetPiece(pos) == null)
                throw new BoardException("There is no piece in this position!");
            if (actualPlayer != board.GetPiece(pos).color)
                throw new BoardException("The chosen piece is not yours!");
            if (board.GetPiece(pos).HasAvailableMoves() == false)
                throw new BoardException("There is no available moves for the piece!");
        }

        public void ValidDestPos(Position origPos, Position destPos)
        {
            if (!board.GetPiece(origPos).CanMoveTo(destPos))
                throw new BoardException("Invalid destiny position");
        }

        private void ChangePlayer()
        {
            if (actualPlayer == Color.White)
                actualPlayer = Color.Black;
            else actualPlayer = Color.White;
        }

        public HashSet<Piece> CapturedPieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece p in captured)
            {
                if (p.color == color)
                {
                    aux.Add(p);
                }
            }
            return aux;
        }

        public HashSet<Piece> InGamePieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece p in pieces)
            {
                if (p.color == color)
                {
                    aux.Add(p);
                }
            }
            aux.ExceptWith(CapturedPieces(color));
            return aux;
        }

        private Color Enemy(Color color)
        {
            if (color == Color.White)
                return Color.Black;
            if (color == Color.Black)
                return Color.White;
            else return Color.White;
        }

        private Piece King(Color color)
        {
            foreach (Piece p in InGamePieces(color))
            {
                if (p is King)
                    return p;
            }
            return null;
        }

        public bool IsInCheck(Color color)
        {
            Piece k = King(color);
            if(k == null)
            {
                throw new BoardException("Ther is no " + color + " King!");
            }

            foreach (Piece p in InGamePieces(Enemy(color)))
            {
                bool[,] mat = p.AvailableMovs();
                if (mat[k.myPosition.line, k.myPosition.column])
                    return true;
            }
            return false;
        }

        public bool CheckMate(Color color)
        {
            if (!IsInCheck(color))
            {
                return false;
            }

            foreach (Piece p in InGamePieces(color))
            {
                bool[,] mat = p.AvailableMovs();
                for (int i = 0; i < board.lines; i++)
                {
                    for (int j = 0; j < board.columns; j++)
                    {
                        if(mat[i,j])
                        {
                            Position orig = p.myPosition;
                            Position dest = new Position(i, j);
                            Piece capturedPiece = Moves(orig, dest);
                            bool checktest = IsInCheck(color);
                            UndoMove(orig, dest, capturedPiece);
                            if (!checktest)
                                return false;
                        }
                    }
                }
            }
            return true;
        } 

        public void PlaceNewPiece(char column, int line, Piece p)
        {
            board.PlacePiece(p, new ChessPosition(column, line).ToPosition());
            pieces.Add(p);
        }

        private void PlacePieces()
        {
            PlaceNewPiece('a', 8, new Rook(Color.Black, board));
            PlaceNewPiece('b', 8, new Knight(Color.Black, board));
            PlaceNewPiece('c', 8, new Bishop(Color.Black, board));
            PlaceNewPiece('d', 8, new Queen(Color.Black, board));
            PlaceNewPiece('e', 8, new King(Color.Black, board, this));
            PlaceNewPiece('f', 8, new Bishop(Color.Black, board));
            PlaceNewPiece('g', 8, new Knight(Color.Black, board));
            PlaceNewPiece('h', 8, new Rook(Color.Black, board));
            PlaceNewPiece('a', 7, new Pawn(Color.Black, board, this));
            PlaceNewPiece('b', 7, new Pawn(Color.Black, board, this));
            PlaceNewPiece('c', 7, new Pawn(Color.Black, board, this));
            PlaceNewPiece('d', 7, new Pawn(Color.Black, board, this));
            PlaceNewPiece('e', 7, new Pawn(Color.Black, board, this));
            PlaceNewPiece('f', 7, new Pawn(Color.Black, board, this));
            PlaceNewPiece('g', 7, new Pawn(Color.Black, board, this));
            PlaceNewPiece('h', 7, new Pawn(Color.Black, board, this));

            PlaceNewPiece('a', 1, new Rook(Color.White, board));
            PlaceNewPiece('b', 1, new Knight(Color.White, board));
            PlaceNewPiece('c', 1, new Bishop(Color.White, board));
            PlaceNewPiece('d', 1, new Queen(Color.White, board));
            PlaceNewPiece('e', 1, new King(Color.White, board, this));
            PlaceNewPiece('f', 1, new Bishop(Color.White, board));
            PlaceNewPiece('g', 1, new Knight(Color.White, board));
            PlaceNewPiece('h', 1, new Rook(Color.White, board));
            PlaceNewPiece('a', 2, new Pawn(Color.White, board, this));
            PlaceNewPiece('b', 2, new Pawn(Color.White, board, this));
            PlaceNewPiece('c', 2, new Pawn(Color.White, board, this));
            PlaceNewPiece('d', 2, new Pawn(Color.White, board, this));
            PlaceNewPiece('e', 2, new Pawn(Color.White, board, this));
            PlaceNewPiece('f', 2, new Pawn(Color.White, board, this));
            PlaceNewPiece('g', 2, new Pawn(Color.White, board, this));
            PlaceNewPiece('h', 2, new Pawn(Color.White, board, this));
        }
    }
}
