using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIXiangQiGame
{
    internal class Ma : Chess
    {
        public Ma(ChessColour colour)
        {
            this.Type = TypeChess.MA;
            this.Colour = colour;
        }

        public override string ToString()
        {
            if (this.Colour == ChessColour.BLACK)
                return "馬";
            else
                return "马";

        }

        //马走日，马要走方向前不能有东西阻挡
        public override bool MovingRule(int XMov, int YMov, int XDes, int YDes, Chess[,] chessboard)
        {
            int XDis;
            int YDis;

            (XDis, YDis) = Table.DisCaculate(XMov, YMov, XDes, YDes, this.Colour);

            if (System.Math.Abs(XDis) == 1 && YDis == 2)
            {//向下两个点
                if (chessboard[YMov - 1, XMov - 1].Colour == ChessColour.BLACK && chessboard[YMov, XMov - 1] == null)
                    return true;//黑色子
                if (chessboard[YMov - 1, XMov - 1].Colour == ChessColour.RED && chessboard[YMov - 2, XMov - 1] == null)
                    return true;//红色子
            }

            if (System.Math.Abs(XDis) == 1 && YDis == -2)
            {//向上两个点
                if (chessboard[YMov - 1, XMov - 1].Colour == ChessColour.BLACK && chessboard[YMov - 2, XMov - 1] == null)
                    return true;//黑色子
                if (chessboard[YMov - 1, XMov - 1].Colour == ChessColour.RED && chessboard[YMov, XMov - 1] == null)
                    return true;//红色子
            }

            if (XDis == 2 && System.Math.Abs(YDis) == 1)
            {//向右两个点
                if (chessboard[YMov - 1, XMov - 2] == null)
                    return true;
            }

            if (XDis == -2 && System.Math.Abs(YDis) == 1)
            {//向左两个点
                if (chessboard[YMov - 1, XMov] == null)
                    return true;
            }


            return false;
        }
    }
}
