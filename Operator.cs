//*****************************************************************************
// Operator.cs - Represents a logical operator with a predefined set of 
// operators.
//*****************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;

namespace QEngine
{
    public class Operator
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="precedence">Operator precedence.</param>
        /// <param name="name">Name of the operator.</param>
        public Operator(int precedence, string name)
        {
            Name = name;
            Precedence = precedence;
        }

        /// <summary>
        /// Provides conversion functionality from string type to this(Operation) type.
        /// </summary>
        /// <param name="oper">Operator to be converted to Opration type.</param>
        /// <returns>Operation with given operator.</returns>
        public static explicit operator Operator(string oper)
        {
            Operator result;

            if (sOperators.TryGetValue(oper, out result))
            {
                return result;
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        /// <summary>
        /// Name of this operator.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Precedence of this operator.
        /// </summary>
        public int Precedence { get; private set; }

        /// <summary>
        /// Checks if the given string is an operator.
        /// </summary>
        public static bool IsOperator(string str)
        {
            return sOperators.Keys.Contains(str);
        }

        /// <summary>
        /// Gets the collection of defined operators.
        /// </summary>
        public static IEnumerable<string> Operators { get { return sOperators.Keys; } }

        static Operator sEqual = new Operator(1, "Equals");
        static Operator sGreaterThan = new Operator(1, "GreaterThan");
        static Operator sLessThan = new Operator(1, "LessThan");
        static Operator sAnd = new Operator(4, "AndAlso");
        static Operator sOr = new Operator(4, "OrElse");
        static Operator sLeftPar = new Operator(3, "LeftPar");
        static Operator sRightPar = new Operator(3, "RightPar");

        private static Dictionary<string, Operator> sOperators = new Dictionary<string, Operator>
        {
            { "=", sEqual },
            { ">", sGreaterThan },
            { "<", sLessThan},
            { "AND", sAnd },
            {"OR", sOr},
            {"(", sLeftPar},
            {")", sRightPar}
        };
    }
}
