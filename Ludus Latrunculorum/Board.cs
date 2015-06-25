/*
    Copyright (C) 2015  Yan A. 

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludus_Latrunculorum
{
    class Board
    {
        private  Piece[,]  board;
        private  int       turn;
        private  int       playerColour;
        private  bool      ai;

        public Board()
        {
            this.board = new Piece[8, 8];
            PlacePieces();
        }

        public void Copy(Board board)
        {
            this.board = new Piece[8, 8];
            this.turn = board.GetTurn();
            this.playerColour = board.GetPlayerColour();
            this.ai = board.GetAIState();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++) {
                    if (board.GetPiece(i, j) != null) {
                        if (board.GetPiece(i, j).IsDux())
                            this.board[i, j] = new Dux(board.GetPiece(i, j).GetColour());
                        else
                            this.board[i, j] = new Pawn(board.GetPiece(i, j).GetColour());
                    }
                    else
                        this.board[i, j] = null;
                }
        }

        public void Restart() { PlacePieces(); }

        private void PlacePieces()
        {
            ai = MainMenu.ai;
            this.turn = MainMenu.playerColour;
            if (ai)
                playerColour = this.turn;
            else
                playerColour = 0;

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++) {
                    if (i == 0)
                        this.board[i, j] = new Pawn(-1);
                    else if (i == 7)
                        this.board[i, j] = new Pawn(1);
                    else
                        this.board[i, j] = null;
                }
            this.board[1, 3] = new Dux(-1);
            this.board[6, 4] = new Dux(1);
        }

        public int GetTurn() { return this.turn; }

        public void SetTurn(int turn) { this.turn = turn; }

        public Piece[,] GetBoard() { return this.board; }

        public Piece GetPiece(int i, int j) { return this.board[i, j]; }

        public void SetPiece(int x, int y, Piece p) { this.board[x, y] = p; } 

        public int GetPlayerColour() { return this.playerColour; }

        public bool GetAIState() { return this.ai; }

        // Tries to update the board with new info, and returns a bool if succesful or not.
        public bool UpdateBoard(int srcX, int srcY, int dstX, int dstY)
        {
            if (LegalMove(srcX, srcY, dstX, dstY)) {
                if (this.board[srcX, srcY].IsDux())
                    this.board[dstX, dstY] = new Dux(this.board[srcX, srcY].GetColour());
                else
                    this.board[dstX, dstY] = new Pawn(this.board[srcX, srcY].GetColour());

                this.board[srcX, srcY] = null;
                this.turn *= -1;

                // Checks for killed pieces.
                for (int i = -1; i <= 1; i += 2) {
                    if (dstX + i > -1 && dstX + i < 8)
                        CheckKill(dstX + i, dstY);
                    if (dstY + i > -1 && dstY + i < 8)
                        CheckKill(dstX, dstY + i);
                }

                if (CheckWin() != 0) {
                    this.turn = 0;
                    MainMenu.msg = ai ? CheckWin() == playerColour ? "You win!" : "You lose!" : CheckWin() == 1 ? "Player I" : "Player II";
                }
                    
                return true;
            }
            return false;
        }

        public bool CheckMove(int srcX, int srcY, int dstX, int dstY)
        {
            if (this.turn != 0 && (this.board[srcX, srcY] != null && (this.board[srcX, srcY].GetColour() == this.turn && this.board[dstX, dstY] == null)))
                if (srcX == dstX || srcY == dstY)
                    if (IsPathClear(srcX, srcY, dstX, dstY))
                        return true;
            return false;
        }

        // Checks whether the move is legal, and returns a bool accordingly.
        private bool LegalMove(int srcX, int srcY, int dstX, int dstY)
        {
            if (this.turn != 0 && (this.board[srcX, srcY] != null && (this.board[srcX, srcY].GetColour() == this.turn && this.board[dstX, dstY] == null)))
                if (srcX == dstX || srcY == dstY)
                    if (IsPathClear(srcX, srcY, dstX, dstY))
                        return true;
            return false;
        }

        // Checks whether the path for the piece to move is clear.
        private bool IsPathClear(int srcX, int srcY, int dstX, int dstY)
        {
            int change = 1;
            if(srcX != dstX)
                if(srcX > dstX) {
                    change = -1;
                    srcX += change;
                    while (srcX > dstX)
                        if (this.board[srcX, srcY] != null)
                            return false;
                        else
                            srcX += change;
                } else {
                    srcX += change;
                    while (srcX < dstX)
                        if (this.board[srcX, srcY] != null)
                            return false;
                        else
                            srcX += change;
                }
            else
                if(srcY > dstY) {
                    change = -1;
                    srcY += change;
                    while (srcY > dstY)
                        if (this.board[srcX, srcY] != null)
                            return false;
                        else
                            srcY += change;
                } else {
                    srcY += change;
                    while (srcY < dstY)
                        if (this.board[srcX, srcY] != null)
                            return false;
                        else
                            srcY += change;
                }


            return true;
        }

        // Checks if the player had won or lost, and returns an int accordingly. ( 1 = won. -1 = lost, 0 = nothing)
        private int CheckWin()
        {
            int white = 0, black = 0;

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++) {
                    if (this.board[i, j] != null && !this.board[i, j].IsDux()) {
                        white += this.board[i, j].GetColour() == 1 ? 1 : 0;
                        black += this.board[i, j].GetColour() == -1 ? 1 : 0;
                    }
                    else if (this.board[i, j] != null && IsDuxStuck(i, j))
                        return -1 * this.board[i, j].GetColour();
                }

            if (white == 0)
                return -1;
            else if (black == 0)
                return 1;

            return 0;
        }

        // Checks whether a piece would die, and returns a bool accordingly.
        private bool CheckKill(int i, int j)
        {
            if (this.board[i, j] != null && !this.board[i, j].IsDux()) {    
                for (int n = -1; n <= 1; n += 2) {
                    // Checks for vertical kill.
                    try {
                        if ((i != 7 && i != 0) && (this.board[i + n, j] != null && this.board[i - n, j] != null))
                            if (this.board[i + n, j].GetColour() != this.board[i, j].GetColour() && this.board[i - n, j].GetColour() != this.board[i, j].GetColour()) {
                                this.board[i, j] = null;
                                return true;
                            }
                    } catch (System.IndexOutOfRangeException) { }

                    // Checks for horizontal kill.
                    try {
                        if (this.board[i, j + n] != null && this.board[i, j - n] != null)
                            if (this.board[i, j + n].GetColour() != this.board[i, j].GetColour() && this.board[i, j - n].GetColour() != this.board[i, j].GetColour()) {
                                this.board[i, j] = null;
                                return true;
                            }
                    } catch (System.IndexOutOfRangeException) { }

                    // Checks for corner kills
                    if ((i == 0 || i == 7) && (j == 0 || j == 7))
                        for (int m = -1; m <= 1; m += 2)
                            try {
                                if ((this.board[i + n, j] != null && this.board[i, j + m] != null))
                                    if (this.board[i + n, j].GetColour() != this.board[i, j].GetColour() && this.board[i, j + m].GetColour() != this.board[i, j].GetColour()) {
                                        this.board[i, j] = null;
                                        return true;
                                    }
                            } catch (System.IndexOutOfRangeException) { }
                }
            }
            return false;
        }

        // Checks whether the Dux is stuck, and returns a bool accordingly.
        public bool IsDuxStuck(int i, int j)
        {
            int sides = 4, pieces = 0;

            for (int n = -1; n < 2; n += 2) {
                try {
                    if (this.board[i + n, j] != null) 
                        pieces++;
                } catch (System.IndexOutOfRangeException) { sides--; }

                try {
                    if (this.board[i, j + n] != null)
                        pieces++;
                } catch (System.IndexOutOfRangeException) { sides--; }
            }

            return sides == pieces;
        }
    }
}
