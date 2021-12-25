using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIXiangQiGame
{
    public class Chess
    {
        public enum TypeChess
        {
            BING, XIANG, CHE, PAO, SHI, MA, JIANG,
        }

        public enum ChessColour
        {
            RED, BLACK,//两种颜色的棋子
        }
        TypeChess type;//identify different chess

        ChessColour colour;

        public Chess() { }
        public Chess(ChessColour colour)
        {
            this.colour = colour;
        }

        public TypeChess Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public ChessColour Colour
        {
            get
            {
                return colour;
            }
            set
            {
                colour = value;
            }
        }


        public override string ToString()
        {
            return "";
        }

        public virtual bool MovingRule(int XMov, int YMov, int XDes, int YDes, Chess[,] chessboard)
        {
            return false;
        }
    }
}
