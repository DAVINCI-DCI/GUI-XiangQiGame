using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIXiangQiGame
{
    internal class Jiang : Chess
    {
        public Jiang(ChessColour colour)
        {
            this.Type = TypeChess.JIANG;
            this.Colour = colour;
        }


        public override string ToString()
        {
            if (this.Colour == ChessColour.BLACK)
                return "将";
            else
                return "帥";

        }

        //将只能在米子里面走，只能上下左右走一格            
        public override bool MovingRule(int XMov, int YMov, int XDes, int YDes, Chess[,] chessboard)
        {
            int XDis;
            int YDis;

            //只能在’米‘里面走
            if (XDes > 6 || XDes < 4)
                return false;

            //判断有没有下到对面米子
            if (this.Colour == ChessColour.BLACK && YDes > 3)
                return false;
            if (this.Colour == ChessColour.RED && YDes < 8)
                return false;

            (XDis, YDis) = Table.DisCaculate(XMov, YMov, XDes, YDes, this.Colour);

            if (XDis == 2 || YDis == 2 || (System.Math.Abs(XDis) == 1 && System.Math.Abs(YDis) == 1))//走两格或者斜着走
                return false;


            return true;
        }
    }
}
