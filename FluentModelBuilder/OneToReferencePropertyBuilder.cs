using System;
using System.Linq.Expressions;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;

namespace Para.FluentModelBuilder.XAF
{
    public static class OneToReferencePropertyBuilder
    {
        public static PropertyBuilder<TType, T> UsingDataSourceProperty<TType, T>(this PropertyBuilder<TType, T> builder, string datasourceProperty, DataSourcePropertyIsNullMode nullMode = DataSourcePropertyIsNullMode.SelectNothing, CriteriaOperator isNullCriteria = null) where TType : class
        {
            var crit = ReferenceEquals(isNullCriteria, null) ? string.Empty : isNullCriteria.ToString();

            return builder.WithAttribute(new DataSourcePropertyAttribute(datasourceProperty, nullMode, crit));
        }

        public static PropertyBuilder<TType, T> UsingDataSourceProperty<TType, T, TProp>(this PropertyBuilder<TType, T> builder, Expression<Func<T, TProp>> datasourceProperty, DataSourcePropertyIsNullMode nullMode = DataSourcePropertyIsNullMode.SelectNothing, CriteriaOperator isNullCriteria = null)
            where TType : class
            where T : class
        {
            return builder.UsingDataSourceProperty(builder._Fields.GetPropertyName(datasourceProperty), nullMode, isNullCriteria);
        }

        public static PropertyBuilder<TType, T> UsingDataSourceCriteriaProperty<TType, T>(this PropertyBuilder<TType, T> builder, string datasourceProperty) where TType : class
        {
            return builder.WithAttribute(new DataSourceCriteriaPropertyAttribute(datasourceProperty));
        }

        public static PropertyBuilder<TType, T> UsingDataSourceCriteriaProperty<TType, T, TProp>(this PropertyBuilder<TType, T> builder, Expression<Func<T, TProp>> datasourceProperty)
            where TType : class
            where T : class
        {
            return builder.UsingDataSourceCriteriaProperty(builder._Fields.GetPropertyName(datasourceProperty));
        }

        public static PropertyBuilder<TType, T> UsingDataSourceCriteriaAttribute<TType, T>(this PropertyBuilder<TType, T> builder, string datasourceCriteria) where TType : class
        {
            return builder.WithAttribute(new DataSourceCriteriaAttribute(datasourceCriteria));
        }

        public static PropertyBuilder<TType, T> UsingDataSourceCriteriaAttribute<TType, T>(this PropertyBuilder<TType, T> builder, CriteriaOperator datasourceCriteria) where TType : class
        {
            return builder.UsingDataSourceCriteriaAttribute(datasourceCriteria.ToString());
        }
    }
}