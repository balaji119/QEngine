//*****************************************************************************
// Extension.cs - Helper class having various extension methods used in this 
// project
//*****************************************************************************
using System.Collections.Generic;
using System.Linq;

namespace QEngine
{
    public static class Extensions
    {
        /// <summary>
        /// Executes where query.
        /// </summary>
        /// <param name="objects">Objectsto be queried.</param>
        /// <param name="condition">where condition.</param>
        /// <param name="props">Properties of database (Headder values).</param>
        /// <returns>Queried collection.</returns>
        public static IQueryable<object> Where(this IQueryable<object> objects, string condition, IEnumerable<string> props)
        {
            QueryEvaluator qe = new WhereQueryEvaluator(objects, condition, props);
            return (IQueryable<object>)qe.Evaluate();
        }

        /// <summary>
        /// Executes select query.
        /// </summary>
        /// <param name="objects">Objects to be queried.</param>
        /// <param name="condition">Select condition.</param>
        /// <param name="props">Database properties.</param>
        /// <returns>Queried collection.</returns>
        public static IQueryable<object> Select(this IQueryable<object> objects, string condition, IEnumerable<string> props)
        {
            QueryEvaluator qe = new SelectQueryEvaluator(objects, condition, props);
            return (IQueryable<object>)qe.Evaluate();
        }

        /// <summary>
        /// Checks if the given string is an operator.
        /// </summary>
        /// <param name="token">string to be checked.</param>
        /// <returns>True if the token is operator otherwise false.</returns>
        public static bool IsOperator(this string token)
        {
            return Operator.IsOperator(token);
        }

        /// <summary>
        /// Checks if the given string is a parenthesis.
        /// </summary>
        public static bool IsParenthesis(this string token)
        {
            return sParenthesis.Contains(token);
        }

        static string[] sParenthesis = new string[] { "(", ")" };
    }
}
