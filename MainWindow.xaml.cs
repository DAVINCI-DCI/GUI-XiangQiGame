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
using System.Windows.Shapes;

namespace GUIXiangQiGame
{
    /// <summary>
    /// beginWindowxaml.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void play_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.Name == "play_button")//Play game button 跳转到下棋窗口
            {
                BeginWindow begin = new BeginWindow();//new 一个下棋窗口
                begin.Show();
                this.Close();//关闭当前页面
            }
        }

        public void menu_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button.Name == "play_rule")
                //点击时打开菜单 open menu
                menuPop.IsOpen = true;

            //点击时对应各自的规则  show each pieces rule
            if (button.Name == "ButtonBing")
            {
                MessageBox.Show("Soldiers can only move and capture 1 point forward (not diagonally) until they cross the river. " +
                                "Once they cross the river, soldiers can move 1 point in any direction except backward. ", "Soldier");
                menuPop.IsOpen = false;
            }
            if (button.Name == "ButtonMa")
            {
                MessageBox.Show("Horse can move 1 point any direction and 1 point diagonally." +
                                "However, the horse cannot jump another piece ." +
                                "e.g. If there is a piece in front of the horse blocking its path of 2 points forward.", "Horse");
                menuPop.IsOpen = false;
            }
            if (button.Name == "ButtonPao")
            {
                MessageBox.Show("Cannon move rules are the same as the chariot/rook with one difference. In order to " +
                                "capture, the cannon has to jump over only 1 piece but it can be of either color.", "Pao");
                menuPop.IsOpen = false;
            }
            if (button.Name == "ButtonShi")
            {
                MessageBox.Show("Guards can move only 1 point diagonally in either direction but, they cannot leave the Imperial Palace.", "Shi");
                menuPop.IsOpen = false;
            }
            if (button.Name == "ButtonXiang")
            {
                MessageBox.Show("The elephant can move 2 points diagonally,as a bishop can in international " +
                                "chess.However an elephant cannot cross the river on the board.If there " +
                                "is a piece on the point which the elephant has to jump over in order" +
                                "to get to the second point then the elephant can't get to the second point.", "Xiang");
                menuPop.IsOpen = false;
            }
            if (button.Name == "ButtonChe")
            {
                MessageBox.Show("A che or rook piece can move any number of spaces in a straight" +
                                "line horizontally or vertically through the rows of the board.", "Chariot");
                menuPop.IsOpen = false;
            }
            if (button.Name == "ButtonJiang")
            {
                MessageBox.Show("The general can move 1 space back or forward, right or left but cannot make a " +
                                "move diagonally. This piece is also not allowed to leave the Imperial Palace" +
                                "area. The general piece may capture any enemy piece which strays into the " +
                                "Imperial Palace area unless that piece is protected by another piece.Generals " +
                                "on the opposite sides of the board cannot be across from each other without" +
                                "another piece between them.", "General");
                menuPop.IsOpen = false;
            }
        }
    }
}
