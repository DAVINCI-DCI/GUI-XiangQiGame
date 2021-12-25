using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIXiangQiGame
{
    internal class Xiang : Chess
    {
        public Xiang(ChessColour colour)
        {
            this.Type = TypeChess.XIANG;
            this.Colour = colour;
        }

        public override string ToString()
        {
            if (this.Colour == ChessColour.BLACK)
                return "象";
            else
                return "相";

        }

        //相不能过河，沿着对角线两格走，且中间不能有阻挡
        public override bool MovingRule(int XMov, int YMov, int XDes, int YDes, Chess[,] chessboard)
        {
            int XDis;
            int YDis;

            if (this.Colour == ChessColour.BLACK && YDes > 5)//黑色方不能过河
                return false;
            if (this.Colour == ChessColour.RED && YDes < 6)//红色方不能过河
                return false;

            (XDis, YDis) = Table.DisCaculate(XMov, YMov, XDes, YDes, this.Colour);

            if (System.Math.Abs(XDis) == 2 && System.Math.Abs(YDis) == 2)
            {//斜着走两格

                if (chessboard[(YMov + YDes) / 2 - 1, (XMov + XDes) / 2 - 1] != null)
                    return false;//判断中间是否为空棋子

                return true;
            }

            return false;
        }
    }
}
