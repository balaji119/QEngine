using System;
//*****************************************************************************
// SelectQueryEvaluator.cs - Derived from QueryEvaluator which does "Select" 
// query evaluation.
// TO DO: This class does not handle selecting more than one property.
//        Need to be supported.
//*****************************************************************************
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace QEngine
{
    class SelectQueryEvaluator : QueryEvaluator
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="objects">Objects to be queried.</param>
        /// <param name="condition">Select condition.</param>
        /// <param name="props">Database properties</param>
        public SelectQueryEvaluator(IQueryable<object> objects, string condition, IEnumerable<string> props)
            :base(objects, condition, props)
        {
        }

        /// <summary>
        /// Executes the Select query.
        /// </summary>
        /// <returns>Queried collection</returns>
        public override IQueryable Evaluate()
        {
            var regex = new Regex(@"(.*)\((.*)\)");
            string prop, oper = String.Empty;
            if (regex.IsMatch(Condition))
            {
                var match = regex.Match(Condition);
                oper = match.Groups[1].Value; // Operator.
                prop = match.Groups[2].Value; // Property name.
                // if porp is null then the first match in the regular expression is property.
                if (String.IsNullOrWhiteSpace(prop)) prop = oper; 

            }
            else
            {
                prop = Condition;
            }

            // If property is * then there is nod need to do any query.
            if (prop.Equals("*")) return CreateRows(Objects, prop);

            var exp = CreateExpression(prop);

            // TO DO: Expression call is rpeated. Create a generic function.
            MethodCallExpression mcexp = Expression.Call(typeof(Queryable),
                "Select", new Type[] { Objects.ElementType, Objects.ElementType }, Objects.Expression,
                Expression.Lambda<Func<object, object>>(exp, new ParameterExpression[] { ParameterType }));

            if (oper.Equals("UNIQ"))
                mcexp = Expression.Call(typeof(Queryable),
                "Distinct", new Type[] { Objects.ElementType }, mcexp);

            var objs= Objects.Provider.CreateQuery<object>(mcexp);

            // TO DO: Make this an expression call usign generic function.
            if (oper.Equals("MAX")) {
                List<object> objList = new List<object>();
                objList.Add(objs.Max(x=> double.Parse(x.ToString())));
                objs  = objList.AsQueryable();
            }

            return CreateRows(objs, prop);
        }

        /// <summary>
        /// Converts IQueryable of object to list of Row for easy data handling at the client side.
        /// </summary>
        IQueryable CreateRows(IQueryable<object> objects,  string prop)
        {
            List<Row> rows = new List<Row>();
            if (prop.Equals("*"))
            {
                // TO DO: Adding headed to be handled at client side.
                rows.Add(new Row(Properties));
                foreach (var obj in objects)
                {
                    var row = new Row();
                    foreach (var p in Properties)
                    {
                        var type = obj.GetType();
                        // Adding value to row by getting property value dynamically using its type.
                        row.AddItem(type.GetProperty(p).GetValue(obj).ToString());
                    }
                    rows.Add(row);
                }
            }
            else
            {
                rows.Add(new Row(Condition));
                foreach (var obj in objects)
                {
                    rows.Add(new Row(obj.ToString()));
                }
            }

            return rows.AsQueryable();
        }
    }
}
