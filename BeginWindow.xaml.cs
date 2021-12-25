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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUIXiangQiGame
{
    public partial class BeginWindow : Window
    {

        static GAMESTATE gameState = GAMESTATE.SelectMove;
        Table table = new Table();

        public BeginWindow()
        {
            InitializeComponent();
            ShowGrid(true);

            /* TableService tableService = new TableService();
             tableService.play();*/

            ChangeState(GAMESTATE.SelectPieces);

        }

        public enum GAMESTATE
        {
            SelectMove, SelectPieces, Win
        }

        private void ShowGrid(bool CanClick)
        {
            Button mybutton;
            int row = XiangQiGrid.RowDefinitions.Count;
            int col = XiangQiGrid.ColumnDefinitions.Count;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    mybutton = new Button();
                    XiangQiGrid.Children.Add(mybutton);
                    mybutton.DataContext = table.chessboard[i, j];
                    if (mybutton.DataContext != null)
                    {
                        mybutton.Content = (String)mybutton.DataContext.ToString();
                        mybutton.FontSize = 25;

                        //调节颜色
                        Chess.ChessColour colour = ((Chess)mybutton.DataContext).Colour;
                        if (colour == Chess.ChessColour.BLACK) mybutton.Foreground = Brushes.Black;
                        else mybutton.Foreground = Brushes.Red;
                    }
                    if (CanClick)
                        mybutton.Click += Play_Button_Click;
                    mybutton.SetValue(Grid.RowProperty, i);
                    mybutton.SetValue(Grid.ColumnProperty, j);
                }
            }
        }

        private void ShowGrid(int NowRow,int NowCol)
        {
            Button mybutton;
            int row = XiangQiGrid.RowDefinitions.Count;
            int col = XiangQiGrid.ColumnDefinitions.Count;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    mybutton = new Button();

                    (bool CanSuccessMove,bool isWin) = table.MovingChess(false,NowCol + 1, NowRow + 1, j + 1, i + 1);

                    if(CanSuccessMove)
                        mybutton.Background = Brushes.Gray;

                    XiangQiGrid.Children.Add(mybutton);
                    mybutton.DataContext = table.chessboard[i, j];
                    if (mybutton.DataContext != null)
                    {
                        mybutton.Content = (String)mybutton.DataContext.ToString();
                        mybutton.FontSize = 25;

                        //调节颜色
                        Chess.ChessColour colour = ((Chess)mybutton.DataContext).Colour;
                        if (colour == Chess.ChessColour.BLACK) mybutton.Foreground = Brushes.Black;
                        else mybutton.Foreground = Brushes.Red;
                    }
                    mybutton.Click += Play_Button_Click;
                    mybutton.SetValue(Grid.RowProperty, i);
                    mybutton.SetValue(Grid.ColumnProperty, j);
                }
            }
        }

        static int NowRow;
        static int NowCol;
        static int DesRow;
        static int DesCol;
        static Boolean isRed = true;

        public void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;

                switch (gameState)
                {
                    case GAMESTATE.SelectMove:
                        DesRow = (int)button.GetValue(Grid.RowProperty);
                        DesCol = (int)button.GetValue(Grid.ColumnProperty);
                        /* MessageBox.Show(row + " " + col + button.DataContext.ToString() + gameState);*/
                        
                        bool isSuccessMove;
                        bool isWin;
                        (isSuccessMove, isWin) = table.MovingChess(true,NowCol + 1, NowRow + 1, DesCol + 1, DesRow + 1);
                        
                        if (!isWin && isSuccessMove)//下子成功但是没赢
                        {
                            ShowGrid(true);
                            ChangeState(GAMESTATE.SelectPieces);
                            isRed = !isRed;
                        }
                        else if (isWin)//赢了
                        {
                            ChangeState(GAMESTATE.Win);
                            ShowGrid(false);
                            Chess.ChessColour colour = ((Chess)button.DataContext).Colour;//获取棋子颜色
                            if (colour == Chess.ChessColour.RED)
                                MessageBox.Show("Congratulate black side is Win !");
                            else
                                MessageBox.Show("Congratulate red side is Win !");
                            return;
                        }else
                            ShowGrid(NowRow, NowCol);

                        break;

                    case GAMESTATE.SelectPieces:
                        NowRow = (int)button.GetValue(Grid.RowProperty);
                        NowCol = (int)button.GetValue(Grid.ColumnProperty);
                       
                        /* MessageBox.Show(row + " " + col + button.DataContext.ToString() + gameState);*/
                        if (table.SelectingChess(NowCol + 1, NowRow + 1, isRed))
                        {
                            ShowGrid(NowRow,NowCol);
                            ChangeState(GAMESTATE.SelectMove);
                            return;
                        }
                            
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ChangeState(GAMESTATE.SelectPieces);
            }
        }

        public void Game_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.Name == "Reset")
            {
                isRed = true;
                table = new Table();
                ChangeState(GAMESTATE.SelectPieces);
                ShowGrid(true);
                return;
            }

        }

        public void ChangeState(GAMESTATE state)
        {
            gameState = state;

            switch (gameState)
            {
                case GAMESTATE.SelectPieces:
                    GameState.Text = "Select Pieces";
                    break;
                case GAMESTATE.SelectMove:
                    GameState.Text = "Select Move";
                    break;
                case GAMESTATE.Win:
                    GameState.Text = "Win";
                    break;
            }
        }

    }
}

