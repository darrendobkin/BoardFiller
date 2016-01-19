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
        private int boxIndex;

        public int? curValue { get; set; }

        // Row, column, and square for this box.
        List<Group> groups = new List<Group>(3);

        // available possible values
        List<int> availableVals = new List<int>();

        // old picks
        List<int> oldPicks = new List<int>();

        // c'tor
        public Box(int idx)
        {
            boxIndex = idx;

            ResetValue();

            for (int i = 1; i <= 9; i++)
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

            // Console.WriteLine("ResetValue(" + boxIndex.ToString() + "), avail.Count=" + availableVals.Count.ToString());

            // return true if out of options
            return availableVals.Count == 0;
        }

        // Redo the available collection based on my groups
        public void RecalcAvailable()
        {
            if (boxIndex == 20) { int i = 0; }

            availableVals.Clear();

            for (int i = 1; i <= 9; i++)
            {
                bool fOK = true;

                foreach (Group g in groups)
                {
                    if (g.IsValueInUse(i))
                    {
                        fOK = false;
                        break;
                    }
                }

                if (fOK)
                {
                    availableVals.Add(i);
                }
            }

            // Console.WriteLine("RecalcAvailable(" + boxIndex.ToString() + "), avail.Count=" + availableVals.Count.ToString());
        }

        /// <summary>
        /// A group is notifying this box that a value previously in use is now available to the group.  Consider it
        /// to be available to this box if it does not conflict with one of this box's other groups.
        /// </summary>
        /// <param name="fromGroup"></param>
        /// <param name="val"></param>
        public void AddAvailableVal(Group fromGroup, Box fromBox, int val)
        {
            bool fOK = true;

            // Don't add values released by later boxes that I've already picked before.
            if ((fromBox.boxIndex > boxIndex) && oldPicks.Contains(val))
                return;

            foreach (Group g in groups)
            {
                //if (g != fromGroup && g.IsValueInUse(val, fromBox))
                if (g.IsValueInUse(val, fromBox))
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
            if (boxIndex == 20) { int i = 0; }

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

            Random random = new Random();
            int iVal = availableVals[random.Next(0, availableVals.Count)];

            //int iVal = availableVals.First<int>();

            // if I already have a value, let my groups know I'm giving it up
            if (curValue != null)
            {
                if (boxIndex == 11) { int i = 0; }

                foreach (Group g in groups)
                {
                    g.ValueIsAvailable(this, curValue.GetValueOrDefault());
                }
            }

            curValue = iVal;

            if (boxIndex == 20) { int i = 0; }

            availableVals.Remove(iVal);
            oldPicks.Add(iVal);

            foreach (Group g in groups)
            {
                g.ValueIsUnavailable(this, iVal);
            }

            return true;
        }
    }
}
