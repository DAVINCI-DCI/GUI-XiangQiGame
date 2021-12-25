using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIXiangQiGame
{
    internal class Shi : Chess
    {
        public Shi(ChessColour colour)
        {
            this.Type = TypeChess.SHI;
            this.Colour = colour;
        }

        public override string ToString()
        {
            if (this.Colour == ChessColour.BLACK)
                return "士";
            else
                return "仕";

        }

        //士只能在米子里面走，斜着走一格
        public override bool MovingRule(int XMov, int YMov, int XDes, int YDes, Chess[,] chessboard)
        {
            int XDis;
            int YDis;

            //只能在’米‘里面走
            if (XDes > 6 || XDes < 4)
                return false;

            if (this.Colour == ChessColour.BLACK && YDes > 3)
                return false;
            if (this.Colour == ChessColour.RED && YDes < 8)
                return false;

            (XDis, YDis) = Table.DisCaculate(XMov, YMov, XDes, YDes, this.Colour);

            if (System.Math.Abs(XDis) == 1 && System.Math.Abs(YDis) == 1)//斜着走
                return true;

            return false;
        }
    }
}
