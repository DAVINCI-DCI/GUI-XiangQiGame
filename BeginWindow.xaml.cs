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

        static GAMESTATE gameState = GAMESTATE.SelectMove;//游戏状态
        Table table = new Table();

        public BeginWindow()
        {
            InitializeComponent();
            ShowGrid(true);//一开始展示棋盘  Show the chessboard at the beginning
            ChangeState(GAMESTATE.SelectPieces);//转换选子
        }
        //三种游戏状态
        public enum GAMESTATE
        {
            SelectMove, SelectPieces, Win
        }
        //返回对应棋子图片的文件名以供Uri添加背景图片  Returns the file name of the corresponding chess piece picture for URI to add a background picture
        private static String SetChessUri(Button target)
        {   //The DataContext of the target chess piece is bound to the corresponding chess piece
            Chess targetChess = (Chess)target.DataContext;
            //三目运算符以获取当前棋子的颜色 与Uri的Black或者Red对应
            String color = (targetChess.Colour == Chess.ChessColour.RED ? "Red" : "Black");

            switch (targetChess.Type)
            {   //according to the picture file name
                case Chess.TypeChess.BING:
                    return color + "Bing";
                case Chess.TypeChess.MA:
                    return color + "Ma";
                case Chess.TypeChess.SHI:
                    return color + "Shi";
                case Chess.TypeChess.CHE:
                    return color + "Che";
                case Chess.TypeChess.PAO:
                    return color + "Pao";
                case Chess.TypeChess.XIANG:
                    return color + "Xiang";
                case Chess.TypeChess.JIANG:
                    return color + "Jiang";
                default: return null;
            }
        }

        //形参CanClick 表示可不可以点（游戏结束将不能在点击棋盘上的任意棋子）
        private void ShowGrid(bool CanClick)
        {
            Button mybutton;
            int row = XiangQiGrid.RowDefinitions.Count;//xaml中定义的Grid行数  row number
            int col = XiangQiGrid.ColumnDefinitions.Count;//xaml中定义的Grid列数  column number

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    mybutton = new Button();
                    
                    //设置透明的按钮以展示Grid背景
                    mybutton.Background = Brushes.Transparent;
                    mybutton.BorderBrush = Brushes.Transparent;
                    XiangQiGrid.Children.Add(mybutton);//button与在xaml中定义的 Grid 的对应位置绑定  binding button
                    mybutton.DataContext = table.chessboard[i, j];//与当前i，j对应的chessboard上棋子绑定
                    if (mybutton.DataContext != null)//chessboard上没有的棋子则不考虑添加图片
                    {   
                        String target = SetChessUri(mybutton);//获取对应图片文件名
                        mybutton.Background = new ImageBrush(//添加背景 @"pack://application:,,,/file name.png"为Uri格式
                            new BitmapImage(new Uri(@"pack://application:,,,/JPG/"+target+".png"))
                        );
                    }
                    if (CanClick)
                        mybutton.Click += Play_Button_Click;//添加点击事件 click event
                   
                    mybutton.SetValue(Grid.RowProperty, i);//设置button所在的行
                    mybutton.SetValue(Grid.ColumnProperty, j);//设置button所在的列
                }
            }
        }
        //重载方法，用于选完子后展示可以走的路径  show the feasible path
        //return bool to avoid some chess pieces which can't move successfully after select 
        private bool ShowGrid(int NowRow,int NowCol)
        {
            Button mybutton;
            int row = XiangQiGrid.RowDefinitions.Count;
            int col = XiangQiGrid.ColumnDefinitions.Count;
            bool isCanMove = false;//use to avoid some chess pieces which can't move successfully after select 
           
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    mybutton = new Button();
                    //设置通明的按钮以展示Grid背景
                    mybutton.Background = Brushes.Transparent;
                    mybutton.BorderBrush = Brushes.Transparent;
                    //Analog path,Here only use args canSuccessMove
                    (bool CanSuccessMove,bool isWin) = table.MovingChess(false,NowCol + 1, NowRow + 1, j + 1, i + 1);
                    
                    XiangQiGrid.Children.Add(mybutton);
                    mybutton.DataContext = table.chessboard[i, j];//与当前i，j对应的chessboard上棋子绑定
                    if (mybutton.DataContext != null)
                    {
                        String target = SetChessUri(mybutton);
                        mybutton.Background = new ImageBrush(//添加背景 @"pack://application:,,,/file name.png"为Uri格式
                            new BitmapImage(new Uri(@"pack://application:,,,/JPG/" + target + ".png"))
                        );
                    }
                    mybutton.Click += Play_Button_Click;

                    if (CanSuccessMove) { //Mark the walkable path
                        mybutton.BorderBrush = Brushes.Green;//Green border
                        mybutton.BorderThickness = new Thickness(7,7,7,7);//Highlight button margin thickness
                        isCanMove = true;
                    }
                    
                    if(i == NowRow && j == NowCol)//Right click the event to bind the currently selected piece
                        mybutton.MouseRightButtonDown += RightButtonDown;
                    //每一个按钮都与Grid中对应的一个网格绑定
                    mybutton.SetValue(Grid.RowProperty, i);//设置button所在的行
                    mybutton.SetValue(Grid.ColumnProperty, j);//设置button所在的列
                }
            }
            return isCanMove;//The return value is used to judge whether the current chess piece has a walkable path
        }
        //当点击右键时，视为取消当前棋子的下棋状态，重新回到SelectPieces
        private void RightButtonDown(object sender, MouseButtonEventArgs e)
        {
            XiangQiGrid.Children.Clear();//清除原先的记录
            ChangeState(GAMESTATE.SelectPieces);//重新选棋子
            ShowGrid(true);
        }

        static int NowRow;  //Current row
        static int NowCol;  //Current column
        static int DesRow;  //Destination row
        static int DesCol;  //Destination column
        static Boolean isRed = true;  //Judge whether the current is red
        static int ScoreBlack = 0;  //score of black
        static int ScoreRed = 0;  //score of red
        //Each time you click the button in the grid, judge whether the current status is sub or sub
        public void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;

                switch (gameState)
                {
                    case GAMESTATE.SelectMove:  //下子
                        DesRow = (int)button.GetValue(Grid.RowProperty);  //通过在Grid的行获取在chessboard上对应的行
                        DesCol = (int)button.GetValue(Grid.ColumnProperty);  //通过在Grid的列获取在chessboard上对应的列

                        bool isSuccessMove;
                        bool isWin;
                        (isSuccessMove, isWin) = table.MovingChess(true,NowCol + 1, NowRow + 1, DesCol + 1, DesRow + 1);//真正走子

                        if (!isWin && isSuccessMove)//move succeeded but didn't win
                        {
                            XiangQiGrid.Children.Clear();//Clear original record
                            ShowGrid(true);
                            ChangeState(GAMESTATE.SelectPieces);
                            isRed = !isRed;//Convert opponent
                        }
                        else if (isWin)//赢了
                        {
                            ChangeState(GAMESTATE.Win);
                            XiangQiGrid.Children.Clear();//Clear original record
                            ShowGrid(false);  //赢了之后则不可以再响应点击

                            Chess.ChessColour colour = ((Chess)button.DataContext).Colour;//获取棋子颜色
                            if (colour == Chess.ChessColour.RED) {
                                MessageBox.Show("Black side is Win !", "Congratulate");//后面为标题
                                ScoreBlack++;//Black score plus one
                                BlackScore.Text = "Black Side  :  " + ScoreBlack;  //Synchronized display to black score box
                            } else {
                                MessageBox.Show("Red side is Win !", "Congratulate");
                                ScoreRed++;//Red score plus one
                                RedScore.Text = "Red Side  :  " + ScoreRed;//Synchronously displayed in the red score box
                            }
                            return;
                        }
                        else{//If the selected point cannot be played, the path will be displayed again and the place will be selected again to play chess
                            XiangQiGrid.Children.Clear();//清除原先的记录
                            ShowGrid(NowRow, NowCol);//展示可行路径
                        }
                        break;

                    case GAMESTATE.SelectPieces://选子
                        NowRow = (int)button.GetValue(Grid.RowProperty);
                        NowCol = (int)button.GetValue(Grid.ColumnProperty);
                       
                        //If the clicked coordinates are legal, start the sub phase
                        if (table.SelectingChess(NowCol + 1, NowRow + 1, isRed))
                        {
                            XiangQiGrid.Children.Clear();//清除原先的记录
                            if (ShowGrid(NowRow, NowCol))//return isCanMove to avoid blocked chess piece happen
                                ShowGrid(NowRow, NowCol);
                            else
                                throw new Exception("The feasible path of the chess piece does not exist and has been blocked " +
                                                    "\nPlease choose another place");//throw error when this piece can't move
                            ChangeState(GAMESTATE.SelectMove);
                            return;
                        }
                        break;
                }
            }
            catch (Exception ex)//catch exception if program have
            {
                MessageBox.Show(ex.Message,"Error");//show error message
                ChangeState(GAMESTATE.SelectPieces);//If any abnormality occurs, re select chess
            }
        }
        
        public void Game_Button_Click(object sender, RoutedEventArgs e)
        {   
            Button button = (Button)sender;
            //重置
            if (button.Name == "Reset") {   //A prompt window. If you select OK it will reset or select no it will continue
                if (MessageBox.Show("Reset NOW ?", "Confirm Message",MessageBoxButton.OKCancel) == MessageBoxResult.OK)   {
                    isRed = true;
                    table = new Table();//A new table corresponds to a new piece
                    ChangeState(GAMESTATE.SelectPieces);
                    XiangQiGrid.Children.Clear();//清除原先的记录
                    ShowGrid(true);
                    return;
                }
            }
            if(button.Name == "ClickRule")  {//展示点击规则  
                MessageBox.Show("Left - Click to select pieces" + 
                "\n\nAfter the first click, the walkable path of the chess piece will be displayed.You can only go to the position mentioned above" +
                "\n\nIf you want to reselect chess pieces, Right - Click to cancel the current selection " +
                "and reselect chess pieces(note that it must be within your turn)", "Click Rule");
            }

        }

        private void ChangeState(GAMESTATE state) {
            gameState = state;
            //Synchronize the current status to textpolck to facilitate players to play chess
            switch (gameState)
            {
                case GAMESTATE.SelectPieces:
                    GameState.Text = "Select Pieces";//GameState is the element define in .xaml
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

