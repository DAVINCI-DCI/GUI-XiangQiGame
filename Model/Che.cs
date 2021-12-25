using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIXiangQiGame
{
    internal class Che : Chess
    {
        public Che(ChessColour colour)
        {
            this.Type = TypeChess.CHE;
            this.Colour = colour;
        }

        public override string ToString()
        {
            if (this.Colour == ChessColour.RED)
                return "车";
            else
                return "車";
        }

        //车能上下左右沿直线走，但是不能有阻碍
        public override bool MovingRule(int XMov, int YMov, int XDes, int YDes, Chess[,] chessboard)
        {
            int XDis;
            int YDis;


            (XDis, YDis) = Table.DisCaculate(XMov, YMov, XDes, YDes, this.Colour);

            //上下走
            if (XDis == 0)
            {
                if (Table.IsBlock(YMov, YDes, XMov, true, chessboard) == 0)//没有阻碍
                    return true;
            }

            //左右走
            if (YDis == 0)
            {
                if (Table.IsBlock(XMov, XDes, YMov, false, chessboard) == 0)
                    return true;
            }

            return false;
        }
    }
}
