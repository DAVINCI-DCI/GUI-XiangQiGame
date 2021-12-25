using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIXiangQiGame
{
    internal class Pao : Chess
    {
        public Pao(ChessColour colour)
        {
            this.Type = TypeChess.PAO;
            this.Colour = colour;
        }

        public override string ToString()
        {
            if (this.Colour == ChessColour.BLACK)
                return "炮";
            else
                return "砲";

        }

        //炮跟车一样，但是吃子的时候必须中间隔着一个棋子
        public override bool MovingRule(int XMov, int YMov, int XDes, int YDes, Chess[,] chessboard)
        {
            int XDis;
            int YDis;
            int count = 0;

            (XDis, YDis) = Table.DisCaculate(XMov, YMov, XDes, YDes, this.Colour);

            //竖着走
            if (XDis == 0)
            {
                count = Table.IsBlock(YMov, YDes, XMov, true, chessboard);

                if (count == 0 && chessboard[YDes - 1, XDes - 1] == null)
                    return true;//没有东西，跟车一样直走
                if (count == 1 && chessboard[YDes - 1, XDes - 1] != null)
                    return true;//架炮吃
            }

            //横着走
            if (YDis == 0)
            {
                count = Table.IsBlock(XMov, XDes, YMov, false, chessboard);
                //直走
                if (count == 0 && chessboard[YDes - 1, XDes - 1] == null)
                    return true;
                //架炮吃
                if (count == 1 && chessboard[YDes - 1, XDes - 1] != null)
                    return true;
            }

            return false;
        }
    }
}
