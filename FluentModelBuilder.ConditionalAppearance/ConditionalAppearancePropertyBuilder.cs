using System;
using System.Drawing;
using System.Linq.Expressions;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using Para.FluentModelBuilder.XAF;

namespace Para.FluentModelBuilder.ConditionalAppearance
{
    public static class ConditionalAppearancePropertyBuilder
    {
        public static ConditionalAppearancePropertyBuilder<TProp, T> UsingAppearance<TProp, T>(this PropertyBuilder<TProp, T> builder, string shortId = "Visiblity")
        {
            var appearanceBuilder =  new ConditionalAppearancePropertyBuilder<TProp, T>(builder, shortId);

            (builder as IBuilderManager).AddBuilder(appearanceBuilder);

            return appearanceBuilder;
        }

        internal static string AppendString(this string str, string value)
        {
            var appender = ";";

            if (string.IsNullOrEmpty(str))
                appender = string.Empty;

            return str + appender + value;
        }
    }

    public class ConditionalAppearancePropertyBuilder<TProp, T> : IBuilder
    {
        private readonly PropertyBuilder<TProp, T> _Builder;

        private string _AppearanceItemTypeValue;

        private string _Context;

        internal AppearanceAttribute _Attribute;

        public ConditionalAppearancePropertyBuilder(PropertyBuilder<TProp, T> builder, string shortId)
        {
            _Builder = builder;
            _Attribute = new AppearanceAttribute(typeof(T).FullName + "." + builder.MemberInfo.Name + "." + shortId);
            builder.WithAttribute(_Attribute);
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> UsingForeColor(Color color)
        {
            var converter = System.ComponentModel.TypeDescriptor.GetConverter(color);
            _Attribute.FontColor = converter.ConvertToString(color);
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> UsingBackColor(Color color)
        {
            var converter = System.ComponentModel.TypeDescriptor.GetConverter(color);
            _Attribute.BackColor = converter.ConvertToString(color);
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> UsingFontStyle(FontStyle style)
        {
            _Attribute.FontStyle = style;
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> HavingPriority(int priority)
        {
            _Attribute.Priority = priority;
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> IsVisible()
        {
            _Attribute.Visibility = ViewItemVisibility.Show;
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> IsVisibleAsEmptySpace()
        {
            _Attribute.Visibility = ViewItemVisibility.ShowEmptySpace;
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> IsNotVisible()
        {
            _Attribute.Visibility = ViewItemVisibility.Hide;
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> IsEnabled()
        {
            _Attribute.Enabled = true;
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> IsNotEnabled()
        {
            _Attribute.Enabled = false;
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> When(string criteria)
        {
            _Attribute.Criteria = criteria;
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> When(CriteriaOperator criteria)
        {
            _Attribute.Criteria = criteria.ToString();
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> ForItemsOfType(AppearanceItemType appearanceItemType)
        {
            return ForItemsOfType(appearanceItemType.ToString());
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> ForItemsOfType(string appearanceItemType)
        {
            _AppearanceItemTypeValue = _AppearanceItemTypeValue.AppendString(appearanceItemType);
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> ForLayoutItems()
        {
            return ForItemsOfType(AppearanceItemType.LayoutItem);
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> ForViewItems()
        {
            return ForItemsOfType(AppearanceItemType.ViewItem);
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> ForActions()
        {
            return ForItemsOfType(AppearanceItemType.Action);
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> InTheContextOf(string context)
        {
            _Context = _Context.AppendString(context);
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> InDetailViewContext()
        {
            return InTheContextOf(ViewType.DetailView.ToString());
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> InAnyContext()
        {
            return InTheContextOf(ViewType.Any.ToString());
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> InListViewContext()
        {
            return InTheContextOf(ViewType.ListView.ToString());
        }

        public PropertyBuilder<TProp, T> Build()
        {
            return _Builder;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> Targeting(string property)
        {
            _Attribute.TargetItems = _Attribute.TargetItems.AppendString(property);
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> TargetingAll()
        {
            return Targeting("*");
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> ExceptingTarget(string property)
        {
            return Targeting(property);
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> Targeting<TProp2>(Expression<Func<T, TProp2>> property)
        {
            return Targeting(_Builder._Fields.GetPropertyName(property));
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> ExceptingTarget<TProp2>(Expression<Func<T, TProp2>> property)
        {
            return ExceptingTarget(_Builder._Fields.GetPropertyName(property));
        }

        void IBuilder.Build()
        {
            if (!string.IsNullOrEmpty(_AppearanceItemTypeValue))
                _Attribute.AppearanceItemType = _AppearanceItemTypeValue;

            if (!string.IsNullOrEmpty(_Context))
                _Attribute.Context = _Context;
        }
    }
}
