using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using System.Numerics;
using System.Runtime.CompilerServices;
using TicTacToeWPF;

namespace TicTacToe
{
    class Bot : Game
    {
        protected string PlayerName { get; set; }
        protected int Difficulty { get; set; }
        protected int WonScenarios;
        protected int SimulatedScenarios;
        private List<Game> PossibleScenarios = new List<Game>();
        public Game CurrentGame { get; set; }
        public int?[,] PossibleMoveScores;
            

        public Bot(string playerName, int difficulty)
        {
            this.PlayerName = playerName;
            this.Difficulty = difficulty;
            this.WonScenarios = 0;
            this.SimulatedScenarios = 0;
        }
        public int GetMove()
        {
            switch (Difficulty)
            {
                case 0:
                    return EasyDifficulty();
                case 1:
                    return MediumDifficulty();
                case 2:
                    return HardDifficulty();
                case 3:
                    return VeryHardDifficulty();
                case 4:
                    return ImpossibleDifficulty();
                default:
                    throw new Exception("Wrong difficulty");
            }
        }

        private int EasyDifficulty() // Random move , no logic behind it :)
        {
            Random rnd = new Random();
            return rnd.Next(1, 10);
        }

        private int MediumDifficulty() // Statistically mediocre move - assign values to the board, choose the value closest to the average unless - next turn loss / win possible 
        {
            int move = 0;
            


            return move;
        }
        private int HardDifficulty() //Statistically best move - assign values to the board, choose the highest value unless - next turn loss / win possible 
        {
            int move = 0;



            return move;
        }

        private int VeryHardDifficulty() //Analitically best move - simulate all possible scenarios - choose most defensive move
        {
            int move = 0;



            return move;
        }

        private int ImpossibleDifficulty() //Analitically best move - simulate all possible scenarios - best probability of winning
        {
            CheckMoveStats(CurrentGame);
            int? BestMoveScore = -1000000000;
            int? WorstMoveScore = 1000000000;
            int BestMove = 0 , WorstMove = 0;
            for(int i = 0; i < 9; i++)
            {
                if (PossibleMoveScores[i, 1] > BestMoveScore)
                {
                    BestMoveScore = PossibleMoveScores[i, 1];
                    BestMove = (int)PossibleMoveScores[i, 0];
                }
                if (PossibleMoveScores[i, 1] < WorstMoveScore)
                {
                    WorstMoveScore = PossibleMoveScores[i, 1];
                    WorstMove = (int)PossibleMoveScores[i, 0];
                }
            }
            
            return BestMove;
        }
        private void CheckMoveStats(Game PresentGame)
        {
            PossibleMoveScores = new int?[,]{
                { 1,null},
                { 2,null},
                { 3,null},
                { 4,null},
                { 5,null},
                { 6,null},
                { 7,null},
                { 8,null},
                { 9,null},
            };
            PossibleScenarios.Clear();

            GenerateAllPossibleScenarios(PresentGame);
            foreach(Game Scenario in PossibleScenarios)
            {
                if (Scenario.checkState(Scenario.Board) && Scenario.CurrentPlayer != this.PlayerName)
                {
                    if (PossibleMoveScores[Scenario.SimulationMoveHelp - 1, 1] == null)
                    {
                        PossibleMoveScores[Scenario.SimulationMoveHelp - 1, 1] = 0;
                    }
                    PossibleMoveScores[Scenario.SimulationMoveHelp - 1, 1] += Factorial(9)/Factorial(Scenario.CurrentTurn+1);
                }
                else
                {
                    if (Scenario.IsDraw(Scenario.Board))
                    {
                        if (PossibleMoveScores[Scenario.SimulationMoveHelp - 1, 1] == null)
                        {
                            PossibleMoveScores[Scenario.SimulationMoveHelp - 1, 1] = 0;
                        }
                        PossibleMoveScores[Scenario.SimulationMoveHelp - 1, 1] += 10;
                    }
                    else
                    {
                        if (PossibleMoveScores[Scenario.SimulationMoveHelp - 1, 1] == null)
                        {
                            PossibleMoveScores[Scenario.SimulationMoveHelp - 1, 1] = 0;
                        }
                        PossibleMoveScores[Scenario.SimulationMoveHelp - 1, 1] -= Factorial(9)/Factorial(Scenario.CurrentTurn+1);
                    }
                }

            }
        }
        private void GenerateAllPossibleScenarios(Game PresentGame)
        {
            int ScenarioGenerationCheck = 1;
            PresentGame.SimulationMoveHelp = 0;
            while (ScenarioGenerationCheck < 8)
            {
                if (PossibleScenarios.Count > 0)
                {
                    ScenarioGenerationCheck = PossibleScenarios.Last().CurrentTurn;

                    for(int i = 0; i<PossibleScenarios.Count;i++)
                    {
                        Game Scenario = PossibleScenarios[i];
                        if (Scenario.CurrentTurn == ScenarioGenerationCheck)
                        {
                            GeneratePossibleNextTurn(Scenario);
                        }
                    }
                }
                else
                {
                    
                    GeneratePossibleNextTurn(PresentGame);
                }
            }
            
            
            
        }

        private void GeneratePossibleNextTurn(Game PresentGame)
        {
            
            for (int i = 1; i < 10; i++)
            {
                Game SimGame = new Game(PresentGame);
                if (SimGame.isMoveValid(i.ToString(), PresentGame.Board))
                {
                    if(SimGame.SimulationMoveHelp == 0)
                    {
                        SimGame.SimulationMoveHelp = i;
                    }
                    
                    SimGame.Board = SimGame.makeMove(i.ToString(), SimGame.Board);
                    PossibleScenarios.Add(SimGame);
                }
                
            }
            
        }
        
        private int Factorial(int number)
        {
            int result = 1;
            if (number > 0) 
            {
                
                for (int i = 1; i <= number; i++)
                {
                    result *= i;
                }
            }
            return result;
                            
        }

        private void DefensiveMove(Game PresentGame)
        {
            //TODO - currently it counts enemySymbols in a row / column
            int HorizontalCheck,VerticalCheck;
            for(int i = 0; i < PresentGame.Board.GetLength(0); i++)
            {
                
                HorizontalCheck = 0;
                VerticalCheck = 0;
                string Enemy = this.PlayerName == "O" ? "X" : "O";
                int[] Defense = new int[9];
                
                for(int j = 0; j<PresentGame.Board.GetLength(1); j++)
                {
                    if (PresentGame.Board[i,j] == Enemy)
                    {
                        HorizontalCheck++;
                    }
                    
                    if(PresentGame.Board[j, i] == Enemy)
                    {
                        VerticalCheck++;
                    }
                    

                }
            }
            
        }
    }
}
