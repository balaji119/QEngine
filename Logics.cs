//*****************************************************************************
// Logics.cs - Helper class having various comparision logics used for querying.
//*****************************************************************************
using System.Linq.Expressions;

namespace QEngine
{
    public class Logics
    {
        /// <summary>
        /// Checks if object a is equal to ojbect b
        /// </summary>
        public static bool Equals(object a, object b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Checks if object a is greater than objetct b
        /// </summary>
        public static bool GreaterThan(object a, object b)
        {
            return double.Parse(a.ToString()) > double.Parse(b.ToString());
        }

        /// <summary>
        /// Checks if object a is less than object b
        /// </summary>
        public static bool LessThan(object a, object b)
        {
            return double.Parse(a.ToString()) < double.Parse(b.ToString());
        }
    }
}
