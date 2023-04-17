using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TicTacToe;

namespace TicTacToeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game = new Game();
        private List<Button> buttons = new List<Button>();
        private bool botState = false;
        private Bot bot;
        private bool isBot = false;

        public MainWindow()
        {
            InitializeComponent();
            buttons.Add(Button1);
            buttons.Add(Button2);
            buttons.Add(Button3);
            buttons.Add(Button4);
            buttons.Add(Button5);
            buttons.Add(Button6);
            buttons.Add(Button7);
            buttons.Add(Button8);
            buttons.Add(Button9);
            
        }

        private void MoveOnClick(int ButtonID)
        {
            //AI Initialize
            if (game.CurrentTurn == 0)
            {
                botState = (bool)CheckBoxBot.IsChecked;
                if (botState)
                {
                    bot = new Bot("O", 4);
                }
                else
                {
                    CheckBoxBot.IsEnabled = false;
                }
                
            }

            //Human move
            string move = ButtonID.ToString();
            if (game.isMoveValid(move, game.Board))
            {
                buttons[ButtonID-1].Content = game.CurrentPlayer;
                game.Board = game.makeMove(move, game.Board);
                isBot = true;
                GameEndCheck(ButtonID);
            }

            //AI move
            if (botState && isBot)
            {
                MoveByBot();
                isBot = false;
            }
            
        }

        private void MoveByBot()
        {
            bot.CurrentGame = game;
            int move = -1;
            while(!game.isMoveValid(move.ToString(), game.Board))
            {
                move = bot.GetMove();
            }
            if(buttons[move - 1].IsEnabled)
            {
                buttons[move - 1].Content = game.CurrentPlayer;
                game.Board = game.makeMove(move.ToString(), game.Board);
                GameEndCheck(move);
            }
                
            

        }

        private void GameEndCheck(int ButtonID)
        {
            if (game.checkState(game.Board) || game.IsDraw(game.Board))
            {
                foreach(Button button in buttons)
                {
                    button.IsEnabled = false;
                    
                }
                isBot = false;
                game.CurrentPlayer = game.CurrentPlayer == "X" ? "O" : "X";
                if (game.checkState(game.Board))
                {
                    MessageBox.Show("Payer " + game.CurrentPlayer + " won!");
                }
                else
                {
                    MessageBox.Show("It's a draw!");
                }
                
            }
            else
            {
                TurnTextBox.Text = "Turn " + (game.CurrentTurn + 1).ToString();
                PlayerTextBox.Text = "Current player: " + game.CurrentPlayer;
                buttons[ButtonID - 1].IsEnabled = false;
            }
        }


        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            MoveOnClick(1);
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            MoveOnClick(2);
        }
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            MoveOnClick(3);
        }
        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            MoveOnClick(4);
        }
        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            MoveOnClick(5);
        }
        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            MoveOnClick(6);
        }
        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            MoveOnClick(7);
        }
        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            MoveOnClick(8);
        }
        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            MoveOnClick(9);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            game = new Game();
            isBot = false;
            TurnTextBox.Text = "Turn " + (game.CurrentTurn + 1).ToString();
            PlayerTextBox.Text = "Current player: " + game.CurrentPlayer;
            foreach (Button button in buttons)
            {
                button.IsEnabled = true;
                button.Content = "";
                
            }
            CheckBoxBot.IsEnabled = true;
        }
    }
}
