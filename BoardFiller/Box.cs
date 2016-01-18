using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardFiller
{
    /// <summary>
    /// Represents one box in the board, to hold one number from 1 to 9.
    /// </summary>
    public class Box
    {
        public int? curValue { get; set; }

        // Row, column, and square for this box.
        List<Group> groups = new List<Group>(3);

        // reference to The Board
        Board board;

        // available possible values
        List<int> availableVals = new List<int>();

        // c'tor
        public Box(Board theBoard)
        {
            board = theBoard;

            ResetValue();

            for (int i=1; i<=9; i++)
            {
                availableVals.Add(i);
            }
        }

        /// <summary>
        /// Let this box know it is in the group.
        /// </summary>
        /// <param name="g"></param>
        public void AssignToGroup(Group g)
        {
            groups.Add(g);
        }

        /// <summary>
        /// Discard current value.  Tell all my groups old value is now available.
        /// </summary>
        public bool ResetValue()
        {
            if (curValue != null)
            {
                int oldVal = curValue.GetValueOrDefault();
                curValue = null;

                foreach (Group g in groups)
                {
                    g.ValueIsAvailable(this, oldVal);
                }
            }

            // return true if out of options
            return availableVals.Count == 0;
        }

        // Redo the available collection based on 
        public void RecalcAvailable()
        {

        }

        /// <summary>
        /// A group is notifying this box that a value previously in use is now available to the group.  Consider it
        /// to be available this box if it does not conflict with one of this box's other groups.
        /// </summary>
        /// <param name="fromGroup"></param>
        /// <param name="val"></param>
        public void AddAvailableVal(Group fromGroup, int val)
        {
            bool fOK = true;

            foreach (Group g in groups)
            {
                if (g != fromGroup && g.IsValueInUse(val))
                {
                    fOK = false;
                    break;
                }
            }

            if (fOK && !availableVals.Contains(val))
            {
                availableVals.Add(val);
            }
        }

        /// <summary>
        /// Some other box in one of my groups has picked a value.  Remove the value from my set
        /// of available values.
        /// </summary>
        /// <param name="val"></param>
        public void RemoveAvailableVal(int iVal)
        {
            Debug.Assert(curValue != iVal);

            availableVals.Remove(iVal);
        }

        /// <summary>
        /// Pick a new value for this box from the available values that does not break the board.
        /// Update all my groups to inform them of my pick.  If no good value available, reset.
        /// </summary>
        /// <returns>Return true if was able to choose an available value without breaking board.</returns>
        public bool MakePick()
        {
            if (availableVals.Count == 0)
            {
                return false;
            }

            //Random random = new Random();
            //int iVal = availableVals[random.Next(0, availableVals.Count)];

            int iVal = availableVals.First<int>();

            // if I already have a value, let my groups know I'm giving it up
            if (curValue != null)
            {
                foreach (Group g in groups)
                {
                    g.ValueIsAvailable(this, curValue.GetValueOrDefault());
                }
            }

            curValue = iVal;
            availableVals.Remove(iVal);
            foreach (Group g in groups)
            {
                g.ValueIsUnavailable(this, iVal);
            }

            return true;
        }
    }
}
