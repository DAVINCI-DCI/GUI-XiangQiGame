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
            ShowGrid(true);//一开始展示棋盘
            ChangeState(GAMESTATE.SelectPieces);//转换选子
        }
        //三种游戏状态
        public enum GAMESTATE
        {
            SelectMove, SelectPieces, Win
        }
        //返回对应棋子图片的文件名以供Uri添加背景图片
        private static String SetChessUri(Button target)
        {   //目标棋子的DataContext绑定了对应的棋子
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
            int row = XiangQiGrid.RowDefinitions.Count;//xaml中定义的Grid行数
            int col = XiangQiGrid.ColumnDefinitions.Count;//xaml中定义的Grid列数

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    mybutton = new Button();
                    
                    //设置透明的按钮以展示Grid背景
                    mybutton.Background = Brushes.Transparent;
                    mybutton.BorderBrush = Brushes.Transparent;
                    XiangQiGrid.Children.Add(mybutton);//button与在xaml中定义的 Grid 的对应位置绑定
                    mybutton.DataContext = table.chessboard[i, j];//与当前i，j对应的chessboard上棋子绑定
                    if (mybutton.DataContext != null)//chessboard上没有的棋子则不考虑添加图片
                    {   
                        String target = SetChessUri(mybutton);//获取对应图片文件名
                        mybutton.Background = new ImageBrush(//添加背景 @"pack://application:,,,/file name.png"为Uri格式
                            new BitmapImage(new Uri(@"pack://application:,,,/JPG/"+target+".png"))
                        );
                    }
                    if (CanClick)
                        mybutton.Click += Play_Button_Click;//添加点击事件
                   
                    mybutton.SetValue(Grid.RowProperty, i);//设置button所在的行
                    mybutton.SetValue(Grid.ColumnProperty, j);//设置button所在的列
                }
            }
        }
        //重载方法，用于选完子后展示可以走的路径 return isCanMove to avoid some chess pieces which can't move successfully after select 
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
                    //模拟路径,这里只用到canSuccessMove
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

                    if (CanSuccessMove) { //标出可走路径
                        mybutton.BorderBrush = Brushes.Green;//绿色边框
                        mybutton.BorderThickness = new Thickness(7,7,7,7);//突出button边距厚度
                        isCanMove = true;
                    }
                    
                    if(i == NowRow && j == NowCol)//用鼠标右击事件绑定当前选中的棋子
                        mybutton.MouseRightButtonDown += RightButtonDown;
                    //每一个按钮都与Grid中对应的一个网格绑定
                    mybutton.SetValue(Grid.RowProperty, i);//设置button所在的行
                    mybutton.SetValue(Grid.ColumnProperty, j);//设置button所在的列
                }
            }
            return isCanMove;//返回值用于判断当前棋子是否有可走的路径
        }
        //当点击右键时，视为取消当前棋子的下棋状态，重新回到SelectPieces
        private void RightButtonDown(object sender, MouseButtonEventArgs e)
        {
            XiangQiGrid.Children.Clear();//清除原先的记录
            ChangeState(GAMESTATE.SelectPieces);//重新选棋子
            ShowGrid(true);
        }

        static int NowRow;//当前行
        static int NowCol;//当前列
        static int DesRow;//目的地行
        static int DesCol;//目的地列
        static Boolean isRed = true;//判断当前是不是红色走
        static int ScoreBlack = 0;//黑色的分数
        static int ScoreRed = 0;//红色的分数
        //每次点击Grid 中的Button 时判断当前状态是下子还是选子
        public void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;

                switch (gameState)
                {
                    case GAMESTATE.SelectMove://下子
                        DesRow = (int)button.GetValue(Grid.RowProperty);//通过在Grid的行获取在chessboard上对应的行
                        DesCol = (int)button.GetValue(Grid.ColumnProperty);//通过在Grid的列获取在chessboard上对应的列

                        bool isSuccessMove;
                        bool isWin;
                        (isSuccessMove, isWin) = table.MovingChess(true,NowCol + 1, NowRow + 1, DesCol + 1, DesRow + 1);//真正走子

                        if (!isWin && isSuccessMove)//下子成功但是没赢
                        {
                            XiangQiGrid.Children.Clear();//清除原先的记录
                            ShowGrid(true);
                            ChangeState(GAMESTATE.SelectPieces);
                            isRed = !isRed;//转换对手
                        }
                        else if (isWin)//赢了
                        {
                            ChangeState(GAMESTATE.Win);
                            XiangQiGrid.Children.Clear();//清除原先的记录
                            ShowGrid(false);//赢了之后则不可以再响应点击

                            Chess.ChessColour colour = ((Chess)button.DataContext).Colour;//获取棋子颜色
                            if (colour == Chess.ChessColour.RED) {
                                MessageBox.Show("Black side is Win !", "Congratulate");//后面为标题
                                ScoreBlack++;//黑色分数加一
                                BlackScore.Text = "Black Side  :  " + ScoreBlack;//同步展示到黑色的分数框
                            } else {
                                MessageBox.Show("Red side is Win !", "Congratulate");
                                ScoreRed++;//红色分数加一
                                RedScore.Text = "Red Side  :  " + ScoreRed;//同步展示到红色的分数框
                            }
                            return;
                        }
                        else{//选择的点不能下，则重新展示路径，重新选地方下棋
                            XiangQiGrid.Children.Clear();//清除原先的记录
                            ShowGrid(NowRow, NowCol);//展示可行路径
                        }
                        break;

                    case GAMESTATE.SelectPieces://选子
                        NowRow = (int)button.GetValue(Grid.RowProperty);
                        NowCol = (int)button.GetValue(Grid.ColumnProperty);
                       
                        //如果点击的坐标合法则开始下子阶段
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
            catch (Exception ex)//catch 异常
            {
                MessageBox.Show(ex.Message,"Error");//show error message
                ChangeState(GAMESTATE.SelectPieces);//有异常出现则重新选棋
            }
        }
        
        public void Game_Button_Click(object sender, RoutedEventArgs e)
        {   
            Button button = (Button)sender;
            //重置
            if (button.Name == "Reset") {   //一个提示窗口，如果选择OK 则重置，No 则取消
                if (MessageBox.Show("Reset NOW ?", "Confirm Message",MessageBoxButton.OKCancel) == MessageBoxResult.OK)   {
                    isRed = true;
                    table = new Table();//新的table,对应新的棋子
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
            //将当前状态同步到textbolck,方便玩家下棋
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

