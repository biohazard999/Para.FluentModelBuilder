using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;

namespace Para.FluentModelBuilder.XAF
{
    public class PropertyBuilder<T, TType> : IBuilderManager
    {
        public readonly Fields<TType> _Fields = new Fields<TType>();
 
        public readonly IMemberInfo MemberInfo;

        internal readonly List<IBuilder> _Builders = new List<IBuilder>();

        public PropertyBuilder(IMemberInfo memberInfo)
        {
            MemberInfo = memberInfo;
        }

        public PropertyBuilder<T, TType> WithAttribute(Attribute attribute)
        {
            MemberInfo.AddAttribute(attribute);
            return this;
        }

        public PropertyBuilder<T, TType> WithAttribute<TAttribute>() where TAttribute : Attribute, new()
        {
            return WithAttribute<TAttribute>(a => { });
        }

        public PropertyBuilder<T, TType> WithAttribute<TAttribute>(Action<TAttribute> configureAction) where TAttribute : Attribute, new()
        {
            var attribute = new TAttribute();

            configureAction(attribute);

            return WithAttribute(attribute);
        }

        public PropertyBuilder<T, TType> WithModelDefault(string propertyName, string propertyValue)
        {
            return WithAttribute(new ModelDefaultAttribute(propertyName, propertyValue));
        }

        public PropertyBuilder<T, TType> HasCaption(string caption)
        {
            return WithModelDefault(ModelDefaultKeys.Caption, caption);
        }

        public PropertyBuilder<T, TType> HasDisplayFormat(string displayFormat)
        {
            return WithModelDefault(ModelDefaultKeys.DisplayFormat, displayFormat);
        }

        public PropertyBuilder<T, TType> IsVisibleInDetailView(bool isVisible = true)
        {
            return WithAttribute(new VisibleInDetailViewAttribute(isVisible));
        }

        public PropertyBuilder<T, TType> IsVisibleInListView(bool isVisible = true)
        {
            return WithAttribute(new VisibleInListViewAttribute(isVisible));
        }

        public PropertyBuilder<T, TType> IsVisibleInLookupListView(bool isVisible = true)
        {
            return WithAttribute(new VisibleInLookupListViewAttribute(isVisible));
        }

        public PropertyBuilder<T, TType> IsNotVisibleInDetailView()
        {
            return IsVisibleInDetailView(false);
        }

        public PropertyBuilder<T, TType> IsNotVisibleInListView()
        {
            return IsVisibleInListView(false);
        }

        public PropertyBuilder<T, TType> IsNotVisibleInLookupListView()
        {
            return IsVisibleInLookupListView(false);
        }

        public PropertyBuilder<T, TType> IsVisibleInAnyView(bool isVisible = true)
        {
            return
                IsVisibleInDetailView(isVisible)
                .IsVisibleInListView(isVisible)
                .IsVisibleInLookupListView();
        }

        public PropertyBuilder<T, TType> IsNotVisibleInAnyView()
        {
            return IsVisibleInAnyView(false);
        }

        public PropertyBuilder<T, TType> UsingPropertyEditor(string propertyEditorTypeName)
        {
            return WithModelDefault(ModelDefaultKeys.PropertyEditorType, propertyEditorTypeName);
        }

        public PropertyBuilder<T, TType> UsingPropertyEditor<TEditor>()
        {
            return UsingPropertyEditor(typeof(TEditor));
        }

        public PropertyBuilder<T, TType> UsingPropertyEditor(Type propertyEditorTypeName)
        {
            return UsingPropertyEditor(propertyEditorTypeName.FullName);
        }

        public PropertyBuilder<T, TType> IsImmediatePostData()
        {
            return WithAttribute<ImmediatePostDataAttribute>();
        }

        public PropertyBuilder<T, TType> NotAllowingEdit()
        {
            return WithModelDefault("AllowEdit", "false");
        }

        public PropertyBuilder<T, TType> AllowingEdit()
        {
            return WithModelDefault("AllowEdit", "true");
        }

        public PropertyBuilder<T, TType> NotAllowingNew()
        {
            return WithModelDefault("AllowNew", "false");
        }

        public PropertyBuilder<T, TType> AllowingNew()
        {
            return WithModelDefault("AllowNew", "true");
        }

        public PropertyBuilder<T, TType> NotAllowingDelete()
        {
            return WithModelDefault("AllowDelete", "false");
        }

        public PropertyBuilder<T, TType> AllowingDelete()
        {
            return WithModelDefault("AllowDelete", "true");
        }

        public PropertyBuilder<T, TType> AllowingNothing()
        {
            return NotAllowingDelete().NotAllowingEdit().NotAllowingNew();
        }

        public PropertyBuilder<T, TType> AllowingEverything()
        {
            return AllowingDelete().AllowingEdit().AllowingNew();
        }

        void IBuilderManager.AddBuilder(IBuilder builder)
        {
            _Builders.Add(builder);
        }

        void IBuilder.Build()
        {
            _Builders.ForEach(b => b.Build());
        }
    }
}