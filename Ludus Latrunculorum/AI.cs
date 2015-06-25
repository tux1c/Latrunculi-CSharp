/*
    Copyright (C) <year>  <name of author>

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
    class AI
    {
        static int DONE = -987654321;
        static int MAXLEVEL = 3;

        public static int[] MakeTurn(Board game, int level)
        {
            int[] bestMove = new int[4];
            int mostValuable = -999999999;
            int tempVal = 0;

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    for (int k = 0; k < 8; k++)
                        if (game.GetPiece(i, j) != null && game.GetPiece(k, j) == null) {
                            if (level < MAXLEVEL - 1)
                                tempVal = GoDeeper(game, i, j, k, j, level, mostValuable);
                            else
                                tempVal = EvaluateMove(game, i, j, k, j, level);

                            if (tempVal != -DONE && tempVal >= mostValuable) {
                                mostValuable = tempVal;
                                bestMove[0] = i;
                                bestMove[1] = j;
                                bestMove[2] = k;
                                bestMove[3] = j;
                            }
                        }
                    for (int l = 0; l < 8; l++)
                        if (game.GetPiece(i, j) != null && game.GetPiece(i, l) == null) {
                            if (level < MAXLEVEL - 1)
                                tempVal = GoDeeper(game, i, j, i, l, level, mostValuable);
                            else
                                tempVal = EvaluateMove(game, i, j, i, l, level);

                            if (tempVal != -DONE && tempVal >= mostValuable) {
                                mostValuable = tempVal;
                                bestMove[0] = i;
                                bestMove[1] = j;
                                bestMove[2] = i;
                                bestMove[3] = l;
                            }
                        }
                }
                        /*
                        for (int l = 0; l < 8; l++) {
                            if ((i == k || j == l) && (game.GetPiece(i, j) != null && game.GetPiece(k, l) ==  null))
                            {
                                if (level < MAXLEVEL - 1)
                                    tempVal = GoDeeper(game, i, j, k, l, level, mostValuable);
                                else
                                    tempVal = EvaluateMove(game, i, j, k, l, level);

                                if (tempVal != -DONE && tempVal >= mostValuable)
                                {
                                    mostValuable = tempVal;
                                    bestMove[0] = i;
                                    bestMove[1] = j;
                                    bestMove[2] = k;
                                    bestMove[3] = l;
                                }
                            }
                        }*/


            // Debug
            //if(level == 0)
            //    System.Windows.Forms.MessageBox.Show(mostValuable + "");

            return bestMove;
        }

        private static int GoDeeper(Board currentBoard, int srcX, int srcY, int dstX, int dstY, int level, int highestValue)
        {
            int[] move = new int[]{srcX, srcY, dstX, dstY};
            int[] arr;
            Board tempBoard = new Board();
            tempBoard.Copy(currentBoard);

            if (tempBoard.UpdateBoard(srcX, srcY, dstX, dstY)) {
                if (EvaluateBoard(tempBoard) < highestValue)
                    return DONE;
                else
                    if (EvaluateBoard(tempBoard) < 9000 || EvaluateBoard(tempBoard) > -1000) {
                        arr = MakeTurn(tempBoard, level + 1);
                        tempBoard.UpdateBoard(arr[0], arr[1], arr[2], arr[3]);
                        return EvaluateBoard(tempBoard);
                    }
                    else
                        return EvaluateBoard(tempBoard);
            }

            return DONE;
        }

        private static int EvaluateMove(Board currentBoard, int srcX, int srcY, int dstX, int dstY, int level)
        {
            Board tempBoard = new Board();
            tempBoard.Copy(currentBoard);
            if (tempBoard.UpdateBoard(srcX, srcY, dstX, dstY))
                return EvaluateBoard(tempBoard);
            return DONE;
        }

        // Gets a board an evaluates it.
        private static int EvaluateBoard(Board turn)
        {
            int value = 0;
            int playerColour = turn.GetPlayerColour();

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (turn.GetPiece(i, j) != null) {
                        if (turn.GetPiece(i, j).GetColour() == playerColour)
                            value--;
                        else
                            value++;
                        if (turn.GetPiece(i, j).IsDux()) {
                            if (turn.GetPiece(i, j).GetColour() == playerColour)
                                if (turn.IsDuxStuck(i, j))
                                    value += 9001;
                            if (turn.GetPiece(i, j).GetColour() == playerColour * -1)
                                if (turn.IsDuxStuck(i, j))
                                    value -= 1337;
                        }
                    }

            return value;
        }
    }
}
