//*****************************************************************************
// Row.cs - Represents a row in database tabe.
//*****************************************************************************
using System;
using System.Collections.Generic;

namespace QEngine
{
    public class Row
    {
        /// <summary>
        /// Default constuctor.
        /// </summary>
        public Row()
        {
            mValues = new List<string>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="items">items to be represented as Row</param>
        public Row(IEnumerable<string> items)
        {
            mValues = new List<string>();
            mValues.AddRange(items);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row">comma separated string to be represented as Row</param>
        public Row(string row)
        {
            mValues = new List<string>();
            mValues.AddRange(row.Split(','));
        }

        /// <summary>
        /// Addes item to this Row.
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(string item)
        {
            mValues.Add(item);
        }

        /// <summary>
        /// Overrided default ToString to represent Row
        /// </summary>
        public override string ToString()
        {
            return String.Join(",", mValues);
        }

        /// <summary>
        /// Vaules in this Row
        /// </summary>
        public IEnumerable<string> Values { get { return mValues; } }
        List<string> mValues;
    }
}
