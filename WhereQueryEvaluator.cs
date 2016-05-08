//*****************************************************************************
// WhereQueryEvaluator.cs - Derived from QueryEvaluator which does "Where" 
// query evaluation.
//*****************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
namespace QEngine
{
    public class WhereQueryEvaluator: QueryEvaluator
    {
        Stack<Operator> mOperators = new Stack<Operator>();
        Stack<Expression> mExpression = new Stack<Expression>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="objects">Objects to be queried.</param>
        /// <param name="condition">Where condition.</param>
        /// <param name="props">Database properties</param>
        public WhereQueryEvaluator(IQueryable<object> objects, string condition, IEnumerable<string> props): 
            base (objects, condition, props)
        {
        }

        /// <summary>
        /// Executes the Where query.
        /// </summary>
        /// <returns>Queried collection</returns>
        public override IQueryable Evaluate()
        {
            if (String.IsNullOrWhiteSpace(Condition)) return Objects;
            var pattern = Operator.Operators.Take(Operator.Operators.Count() - 2)
                .Aggregate((a, b) => String.Format("{0})|({1}", a, b));
            // Removed last two operators and added in the following line 
            // as it requires special treatment.
            //TO DO: Avoid such discrepancy.
            pattern = string.Format(@"({0})|(\()|(\))", pattern);
            foreach (var match in Regex.Split(Condition, pattern))
            {
                // Used Shunting-yard algorithm.
                // https://en.wikipedia.org/wiki/Shunting-yard_algorithm
                // Look at the above page for logic clarification.
                if (string.IsNullOrWhiteSpace(match)) continue;
                var token = match.Trim();
                if (token.IsOperator())
                {
                    if (token.Equals("("))
                    {
                        mOperators.Push((Operator)token);
                        continue;
                    }

                    if (token.Equals(")"))
                    {
                        while (mOperators.Peek() != (Operator)("("))
                            mExpression.Push(ApplyQuery());
                        mOperators.Pop(); // Popping out left parenthesis.
                        continue;
                    }

                    while (mOperators.Count > 0 && mOperators.Peek().Precedence < ((Operator)token).Precedence && mOperators.Peek() != (Operator)("("))
                    {
                        mExpression.Push(ApplyQuery());
                    }
                    mOperators.Push((Operator)token);
                }
                else
                {
                    mExpression.Push(CreateExpression(token));
                }
            }

            while (mOperators.Count > 0)
                mExpression.Push(ApplyQuery());

            if (mExpression.Count > 1) throw new Exception("Query not supported.");
            // Building Where condition.
            MethodCallExpression mcexp = Expression.Call(typeof(Queryable),
                "Where", new Type[] { Objects.ElementType }, Objects.Expression,
                Expression.Lambda<Func<object, bool>>(mExpression.Pop(), new ParameterExpression[] { ParameterType }));
            // Executing query.
            return Objects.Provider.CreateQuery<object>(mcexp);
        }

        Expression ApplyQuery()
        {
            var oper = mOperators.Pop(); // Operator.
            var right = mExpression.Pop(); // value.
            var left = mExpression.Pop(); // Databae property.
            Expression eval = null;
            System.Reflection.MethodInfo method = null;
            // TO DO: Have a factory for the following execution.
            if (oper.Name == "AndAlso")
            {
                eval = Expression.AndAlso(left, right);
            }
            else if (oper.Name == "OrElse")
            {
                eval = Expression.OrElse(left, right);
            }
            else
            {
                method = typeof(Logics).GetMethod(oper.Name, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                eval = Expression.Call(method, left, right);
            }

            return eval;
        }
    }
}
