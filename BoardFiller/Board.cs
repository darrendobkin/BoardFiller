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

        // c'tor
        public Board()
        {
            // must make 81 valid picks to fill board
            int iPick = 0;

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
                Box box = GetBox(iPick);
                if (box.MakePick())
                {
                    iPick++;
                }
                else
                {
                    bool fContinueBacktracking;

                    do
                    {
                        if (fContinueBacktracking = GetBox(iPick).ResetValue())
                        {
                            iPick--;
                        }
                    } while (fContinueBacktracking);
                }
                Dump();
            } while (iPick < 81);
        }

        Box GetBox(int iPick)
        {
            int iRow = iPick / 9;
            int iCol = iPick - (iRow * 9);

            return boxes[iCol, iRow];
        }

        public void Dump()
        {
            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < 9; i++)
                {
                    int? iVal = boxes[i, j].curValue;

                    if (iVal == null)
                        Console.Write(". ");
                    else
                        Console.Write(boxes[i, j].curValue.ToString() + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("==================");
        }
    }
}
