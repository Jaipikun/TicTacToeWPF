using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe;

namespace TicTacToeWPF
{
    class Game
    {
        public string[,] Board { get; set; }

        public string CurrentPlayer { get; set; }

        public int CurrentTurn { get; set; }

        public int SimulationMoveHelp { get; set; }

        public Game() //Constructor
        {
            this.Board = new string[3, 3];

            this.CurrentPlayer = "X";
            this.CurrentTurn = 0;

            this.Board = resetBoard(this.Board);

            
        }

        public Game(Game Original)
        {
            this.Board = new string[3,3];
            for(int i =0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    this.Board[i, j] = Original.Board[i, j];
                }
            }
            this.CurrentPlayer = Original.CurrentPlayer;
            this.CurrentTurn = Original.CurrentTurn;
            this.SimulationMoveHelp = Original.SimulationMoveHelp;
        }


        private void NextTurn() // Change CurrentPlayer and CurrentTurn
        {
            if (this.CurrentPlayer == "O")
            {
                this.CurrentPlayer = "X";
            }
            else
            {
                this.CurrentPlayer = "O";
            }
            this.CurrentTurn++;
        }

        /*
         * String move input -> [row,column] conversion method    - used in isMoveValid() and makeMove()
        
        {1  2  3}   -1  {0  1  2}       {[0,0]  [0,1]  [0,2]}
        {4  5  6}  ---> {3  4  5}  ---> {[1,0]  [1,1]  [1,2]}   [row,column]
        {7  8  9}       {6  7  8}       {[2,0]  [2,1]  [2,2]}

        row = floor((move - 1) / 3)       - quotient
        column = (move-1) % 3             - remainder

        */

        public bool isMoveValid(string move, string[,] TestBoard) //Check whether move's valid. Arguments are move to be checked and board on which its checked
        {
            if (int.TryParse(move, out int moveInt)) // check whether move's parsable, store value in moveInt
            {
                if (moveInt > 0 && moveInt < 10) //check whether move's on any board space
                {
                    moveInt += -1;
                    int row = (int)Math.Floor((decimal)moveInt / 3);
                    Math.DivRem(moveInt, 3, out int column);

                    if (TestBoard[row, column] == move) // if the space has the same value as the moves value, it's valid 
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public string[,] makeMove(string move, string[,] TestBoard) // Make move on the board given in the argument
        {
            if (isMoveValid(move, TestBoard))
            {
                int moveInt = int.Parse(move)-1;

                int Column = (int)Math.Floor((decimal)moveInt / 3);
                Math.DivRem(moveInt, 3, out int Row);

                TestBoard[Column, Row] = CurrentPlayer;
                NextTurn();
            }
            return TestBoard;
        }

        private string[,] resetBoard(string[,] TestBoard) // Clear the given board
        {
            for (int i = 0; i < TestBoard.GetLength(0); i++)
            {
                for (int j = 0; j < TestBoard.GetLength(1); j++)
                {
                    TestBoard[i, j] = (TestBoard.GetLength(0) * i + j + 1).ToString();
                }
            }
            return TestBoard;
        }

        public bool checkState(string[,] TestBoard) // Check whether there's a winner on the given board
        {
            bool state = false;
            int HorCnt;
            int VerCnt;
            for (int i = 0; i < TestBoard.GetLength(0); i++)
            {
                VerCnt = 0;
                HorCnt = 0;
                for (int j = 0; j < TestBoard.GetLength(1); j++)
                {
                    if (TestBoard[i, j] == "X") 
                    {
                        HorCnt++; // Count how many X's are in a row
                    }
                    else if (TestBoard[i, j] == "O")
                    {
                        HorCnt--; // Count how many O's are in a row
                    }
                    else
                    {
                        HorCnt = 0;
                    }

                    if (TestBoard[j, i] == "X")
                    {
                        VerCnt++; // Count how many X's are in a column
                    }
                    else if (TestBoard[j, i] == "O")
                    {
                        VerCnt--; // Count how many O's are in a column
                    }
                    else
                    {
                        VerCnt = 0;
                    }
                }
                if (Math.Abs(VerCnt) == 3) // if 3 in a column, someone won
                {

                    state = true;
                    break;
                }
                if (Math.Abs(HorCnt) == 3) // if 3 in a row, someone won
                {
                    state = true;
                    break;
                }
            }

            if (TestBoard[0, 0] == TestBoard[1, 1] && TestBoard[1, 1] == TestBoard[2, 2]) // Diagonal Left->Right check
            {
                state = true;
            }
            if (TestBoard[2, 0] == TestBoard[1, 1] && TestBoard[1, 1] == TestBoard[0, 2]) // Diagonal Right->Left check
            {
                state = true;
            }

            return state;
        }

        public bool IsDraw(string[,] TestBoard) // Check whether it's a draw
        {
            int cnt = 0;
            for (int i = 0; i < TestBoard.GetLength(0); i++)
            {
                for (int j = 0; j < TestBoard.GetLength(1); j++)
                {
                    if (isMoveValid(TestBoard[i, j], TestBoard))
                    {
                        cnt++; // Count how many valid moves are on the board
                    }
                }
            }
            if (cnt == 0 && !checkState(TestBoard))
            {
                return true; // if there's 0 valid moves and no one's won, it's a draw
            }
            return false; // if there are possible moves, it's not a draw
        }

        
    }
}

