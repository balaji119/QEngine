//*****************************************************************************
// QueryEvaluator.cs - Base class for various qerry evaluator classes.
//*****************************************************************************
using Microsoft.CSharp.RuntimeBinder;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
namespace QEngine
{
    public abstract class QueryEvaluator
    {
        IQueryable<object> mObjects;
        string mCondition;
        IEnumerable<string> mProperties;
        ParameterExpression mParamType = Expression.Parameter(typeof(object));

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objects">Objects to be queried</param>
        /// <param name="condition">Query</param>
        /// <param name="props">Table properties</param>
        public QueryEvaluator(IQueryable<object> objects, string condition, IEnumerable<string> props)
        {
            mObjects = objects;
            mCondition = condition;
            mProperties = props;
        }

        public abstract IQueryable Evaluate();

        /// <summary>
        /// Represents a given string (database property) as a System.Linq.Expression
        /// </summary>
        /// <param name="token">database property</param>
        /// <returns>Expression representing the given property</returns>
        public Expression CreateExpression(string token)
        {
            if (mProperties.Contains(token))
            {
                CallSiteBinder cb = Binder.GetMember(CSharpBinderFlags.None, token, typeof(object), new CSharpArgumentInfo[] {
                   CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                });
                return Expression.Dynamic(cb, typeof(object), mParamType);
            }
            return Expression.Constant(token);
        }

        /// <summary>
        /// Objects to be queried.
        /// </summary>
        public IQueryable<object> Objects { get { return mObjects; } }

        /// <summary>
        /// Query conditon.
        /// </summary>
        public string Condition { get { return mCondition; } }

        /// <summary>
        /// Database properties.
        /// This is used to differentiate property from other string literals while querying.
        /// </summary>
        public IEnumerable<string> Properties { get { return mProperties; } }

        /// <summary>
        /// Parameter expression with type Object.
        /// </summary>
        public ParameterExpression ParameterType { get { return mParamType; } }
    }
}
