using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;

namespace Para.FluentModelBuilder.XAF
{
    public static class ModelBuilder
    {
        public static ITypeInfo FindTypeInfo<T>(this ITypesInfo typesInfo)
        {
            return typesInfo.FindTypeInfo(typeof (T));
        }

        public static ModelBuilder<T> Create<T>(ITypesInfo typesInfo)
        {
            return new ModelBuilder<T>(typesInfo.FindTypeInfo<T>());
        }

        public static ModelBuilder<T> Create<T>()
        {
            return Create<T>(XafTypesInfo.Instance);
        }
    }

    public class ModelBuilder<T> : IBuilderManager, ITypeInfoProvider
    {
        public ITypeInfo TypeInfo { get; private set; }
    
        public readonly Fields<T> _Fields;

        internal readonly List<IBuilder> _Builders = new List<IBuilder>();

        public ModelBuilder(ITypesInfo typesInfo)
            : this(typesInfo.FindTypeInfo<T>())
        {
            
        }

        public ModelBuilder(ITypeInfo typeInfo)
        {
            TypeInfo = typeInfo;
            _Fields = new Fields<T>();

            BuildUp();
        }

        protected virtual void BuildUp()
        {
            
        }

        public ModelBuilder<T> WithAttribute(Attribute attribute)
        {
            TypeInfo.AddAttribute(attribute);
            return this;
        }

        public ModelBuilder<T> WithAttribute<TAttribute>() where TAttribute : Attribute, new()
        {
            return WithAttribute<TAttribute>(a => { });
        }

        public ModelBuilder<T> WithAttribute<TAttribute>(Action<TAttribute> configureAction) where TAttribute : Attribute, new()
        {
            var attribute = new TAttribute();

            configureAction(attribute);

            TypeInfo.AddAttribute(attribute);
            return this;
        }

        public ModelBuilder<T> HasCaption(string caption)
        {
            return WithModelDefault(ModelDefaultKeys.Caption, caption);
        }

        public ModelBuilder<T> WithModelDefault(string propertyName, string propertyValue)
        {
            return WithAttribute(new ModelDefaultAttribute(propertyName, propertyValue));
        }

        public ModelBuilder<T> HasImage(string imageName)
        {
            return WithAttribute(new ImageNameAttribute(imageName));
        }

        public PropertyBuilder<TProp, T> For<TProp>(Expression<Func<T, TProp>> propertyName)
        {
            var builder = PropertyBuilderFactory.PropertyBuilderFactoryMethod<TProp, T>(TypeInfo.FindMember(_Fields.GetPropertyName(propertyName)));

            _Builders.Add(builder);

            return builder;
        }

        public ModelBuilder<T> HasDefaultProperty<TProp>(Expression<Func<T, TProp>> propertyName)
        {
            return HasDefaultProperty(_Fields.GetPropertyName(propertyName));
        }

        public ModelBuilder<T> HasDefaultProperty(string propertyName)
        {
            return WithAttribute(new System.ComponentModel.DefaultPropertyAttribute(propertyName));
        }

        public ModelBuilder<T> HasObjectCaptionFormat(string formatString)
        {
            return WithAttribute(new ObjectCaptionFormatAttribute(formatString));
        }

        public ModelBuilder<T> HasObjectCaptionFormat<TProp>(Expression<Func<T, TProp>> formatString)
        {
            return WithAttribute(new ObjectCaptionFormatAttribute("{0:" + _Fields.GetPropertyName(formatString) + "}"));
        }

        public ModelBuilder<T> NotAllowingEdit()
        {
            return WithModelDefault("AllowEdit", "false");
        }

        public ModelBuilder<T> AllowingEdit()
        {
            return WithModelDefault("AllowEdit", "true");
        }

        public ModelBuilder<T> NotAllowingNew()
        {
            return WithModelDefault("AllowNew", "false");
        }

        public ModelBuilder<T> AllowingNew()
        {
            return WithModelDefault("AllowNew", "true");
        }

        public ModelBuilder<T> NotAllowingDelete()
        {
            return WithModelDefault("AllowDelete", "false");
        }

        public ModelBuilder<T> AllowingDelete()
        {
            return WithModelDefault("AllowDelete", "true");
        }

        public ModelBuilder<T> AllowingNothing()
        {
            return NotAllowingDelete().NotAllowingEdit().NotAllowingNew();
        }

        public ModelBuilder<T> AllowingEverything()
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