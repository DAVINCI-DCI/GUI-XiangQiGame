using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIXiangQiGame
{
    internal class Bing : Chess
    {
        public Bing(ChessColour colour)
        {
            this.Type = TypeChess.BING;
            this.Colour = colour;
        }

        public override string ToString()
        {
            if (this.Colour == ChessColour.BLACK)
                return "卒";
            else
                return "兵";

        }

        //兵过河前只能直走一格，过河后能左右和向前走一格
        public override bool MovingRule(int XNow, int YNow, int XDes, int YDes, Chess[,] chessboard)
        {

            int XDis;
            int YDis;
            (XDis, YDis) = Table.DisCaculate(XNow, YNow, XDes, YDes, this.Colour);

            //黑色红色棋子没过河时
            if ((this.Colour == ChessColour.RED && YNow > 5) || (this.Colour == ChessColour.BLACK && YNow <= 5))
            {
                if (XDis != 0)//不能左右
                    return false;
                if (YDis != 1)//只能向前
                    return false;
            }

            //过河后
            if ((this.Colour == ChessColour.RED && YNow <= 5) || (this.Colour == ChessColour.BLACK && YNow > 5))
            {
                if (YDis == 0 && System.Math.Abs(XDis) == 1)
                    return true;//左右走
                else if (XDis == 0 && YDis == 1)
                    return true;//向前走
                else
                    return false;
            }

            //没过河前    
            return true;

        }
    }
}
