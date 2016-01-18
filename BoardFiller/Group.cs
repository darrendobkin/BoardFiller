using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardFiller
{
    /// <summary>
    /// Represents a row, column, or square of nine boxes on the board.
    /// </summary>
    public class Group
    {
        List<Box> boxes = new List<Box>(9);

        public void AddBox(Box box)
        {
            boxes.Add(box);
        }

        /// <summary>
        /// A box in this group is notifying the group that it is relinquishing its value.  All other members
        /// of this group may now use the value provided it does not conflict with their groups.
        /// </summary>
        /// <param name="val"></param>
        public void ValueIsAvailable(Box fromBox, int val)
        {
            foreach (Box b in boxes)
            {
                if (b != fromBox)
                {
                    b.AddAvailableVal(this, val);
                }
            }
        }

        /// <summary>
        /// A box in this group is notifying the group that it is using a new value.  All other members
        /// of this group must remove it from their set of available groups.
        /// </summary>
        /// <param name="fromBox"></param>
        /// <param name="val"></param>
        public void ValueIsUnavailable( Box fromBox, int val)
        {
            foreach (Box b in boxes)
            {
                if (b != fromBox)
                {
                    b.RemoveAvailableVal(val);
                }
            }
        }

        /// <summary>
        /// Determine if a value is in use in a group.
        /// </summary>
        /// <param name="val">The int value to look for</param>
        /// <returns>True if value is in use.</returns>
        public bool IsValueInUse(int val)
        {
            foreach (Box b in boxes)
            {
                if (b.curValue == val)
                    return true;
            }
            return false;
        }
    }
}
