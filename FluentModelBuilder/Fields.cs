using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DevExpress.Data.Filtering;

namespace Para.FluentModelBuilder.XAF
{
    public class Fields<T>
    {
        public string GetPropertyName<TProp>(Expression<Func<T, TProp>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
                return null;

            return memberExpression.Member.Name;
        }

        public TAttribute GetAttribute<TAttribute>()
        {
            return GetAttributes<TAttribute>().FirstOrDefault();
        }

        public IEnumerable<TAttribute> GetAttributes<TAttribute>()
        {
            return typeof(T).GetCustomAttributes(typeof(TAttribute), true).OfType<TAttribute>();
        }

        public bool HasAttributes<TAttribute>()
        {
            return GetAttributes<TAttribute>().Any();
        }

        public bool HasAttribute<TAttribute>()
        {
            return GetAttribute<TAttribute>() != null;
        }

        public PropertyInfo FromProperty<TProp>(Expression<Func<T, TProp>> expression)
        {
            return typeof(T).GetProperty(GetPropertyName(expression));
        }

        public OperandProperty GetOperand<TProp>(Expression<Func<T, TProp>> expression)
        {
            return new OperandProperty(GetPropertyName(expression));
        }
    }
}