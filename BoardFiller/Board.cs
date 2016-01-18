using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardFiller
{
    public class Board
    {
        // 9x9 array of Boxes
        Box[,] boxes = new Box[9, 9];

        // 27 groups
        Group[] rows = new Group[9];
        Group[] cols = new Group[9];
        Group[] squares = new Group[9];

        // must make 81 valid picks to fill board
        int iPick = 0;

        // c'tor
        public Board()
        {
            // create the groups
            for (int i = 0; i < 9; i++)
            {
                rows[i] = new Group();
                cols[i] = new Group();
                squares[i] = new Group();
            }

            // create the boxes
            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < 9; i++)
                {
                    Box newBox = new Box(this);
                    boxes[i, j] = newBox;
                    cols[i].AddBox(newBox);
                    newBox.AssignToGroup(cols[i]);
                    rows[j].AddBox(newBox);
                    newBox.AssignToGroup(rows[j]);

                    int iSquare = ((j / 3) * 3) + (i / 3);

                    squares[iSquare].AddBox(newBox);
                    newBox.AssignToGroup(squares[iSquare]);
                }
            }

            // Keep making picks until board is filled
            do
            {
                int iRow = iPick / 9;
                int iCol = iPick - (iRow * 9);

                if (boxes[iRow, iCol].MakePick())
                {
                    iPick++;
                }
                else
                {
                    iPick--;
                }
            } while (iPick < 81);
        }
    }
}
