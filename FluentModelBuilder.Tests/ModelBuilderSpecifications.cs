using System;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Para.FluentModelBuilder.XAF;
using Ploeh.AutoFixture;

// ReSharper disable InconsistentNaming
namespace Para.FluentModelBuilder.Tests
{
    [TestClass]
    public class ModelBuilderSpecifications
    {
        private ITypeInfo _TypeInfo;

        private ITypesInfo _TypesInfo;

        private ModelBuilder<TargetClass> _Builder;

        [TestInitialize]
        public void Init()
        {
            _TypesInfo = A.Fake<ITypesInfo>();
            _TypeInfo = A.Fake<ITypeInfo>();
            A.CallTo(() => _TypesInfo.FindTypeInfo(A<Type>.Ignored)).Returns(_TypeInfo);

            _Builder = ModelBuilder.Create<TargetClass>(_TypesInfo);
        }

        [TestMethod]
        public void Create_Should_Call_FindTypeInfo()
        {
            var info = A.Fake<ITypesInfo>();

            ModelBuilder.Create<TargetClass>(info);

            A.CallTo(() => info.FindTypeInfo(A<Type>.That.Matches(t => t == typeof(TargetClass)))).MustHaveHappened();
        }

        [TestMethod]
        public void WithAttribute_Should_Return_Same_ModelBuilder()
        {
            var builder = ModelBuilder.Create<TargetClass>();

            var returnType = builder.WithAttribute(new ModelDefaultAttribute(String.Empty, String.Empty));

            returnType.Should().Be(builder);
        }

        [TestMethod]
        public void WithAttribute_Should_Add_Attribute()
        {
            var attribute = new ModelDefaultAttribute(String.Empty, String.Empty);

            _Builder.WithAttribute(attribute);

            A.CallTo(() => _TypeInfo.AddAttribute(A<Attribute>.That.Matches(t => t.Equals(attribute)))).MustHaveHappened();
        }

        [TestMethod]
        public void HasCaption_Should_Add_ModelDefaultAttribute_With_CaptionPropertyName_And_ValueMyCaption()
        {
            var caption = new Fixture().Create<string>();
            _Builder.HasCaption(caption);

            A.CallTo(() => _TypeInfo.AddAttribute(A<ModelDefaultAttribute>.That.Matches(t => t.PropertyName == ModelDefaultKeys.Caption && t.PropertyValue == caption))).MustHaveHappened();
        }

        [TestMethod]
        public void WithModelDefault_Should_Add_ModelDefaultAttribute()
        {
            var customPropertyName = new Fixture().Create<string>();
            var customValue = new Fixture().Create<string>();

            _Builder.WithModelDefault(customPropertyName, customValue);

            A.CallTo(() => _TypeInfo.AddAttribute(A<ModelDefaultAttribute>.That.Matches(t => t.PropertyName == customPropertyName && t.PropertyValue == customValue))).MustHaveHappened();
        }

        [TestMethod]
        public void HasImage_Should_Add_ImageNameAttribute()
        {
            var imageName = new Fixture().Create<string>();
            _Builder.HasImage(imageName);

            A.CallTo(() => _TypeInfo.AddAttribute(A<ImageNameAttribute>.That.Matches(t => t.ImageName == imageName))).MustHaveHappened();
        }

        [TestMethod]
        public void WithAttributeOfT_Should_Add_New_Attribute()
        {
            _Builder.WithAttribute<WithPublicCtorAttribute>();

            A.CallTo(() => _TypeInfo.AddAttribute(A<WithPublicCtorAttribute>.That.IsInstanceOf(typeof(WithPublicCtorAttribute)))).MustHaveHappened();
        }

        [TestMethod]
        public void For_Should_Return_PropertyBuilder_With_Correct_MemberInfo()
        {
            var memberinfo = A.Fake<IMemberInfo>();
            
            A.CallTo(() => _TypeInfo.FindMember(A<string>.Ignored)).Returns(memberinfo);
            
            var propertyBuilder = _Builder.For(p => p.StringProperty);

            propertyBuilder.MemberInfo.Should().Be(memberinfo);
        }

        [TestMethod]
        public void WithAttributeOfT_Should_Call_Callback_With_New_Attribute()
        {
            var wasCalled = false;
            Action<WithPublicCtorAttribute> myCallBack = item => wasCalled = true;

            _Builder.WithAttribute<WithPublicCtorAttribute>(myCallBack);

            wasCalled.Should().BeTrue();
        }

        [TestMethod]
        public void HasDefaultProperty_Should_Add_DefaultPropertyAttribute_With_Lambda()
        {
            _Builder.HasDefaultProperty(p => p.StringProperty);

            A.CallTo(() => _TypeInfo.AddAttribute(A<System.ComponentModel.DefaultPropertyAttribute>.That.Matches(t => t.Name == TargetClass.StringProperty_PropertyName))).MustHaveHappened();
        }

        [TestMethod]
        public void HasDefaultProperty_Should_Add_DefaultPropertyAttribute_With_String()
        {
            _Builder.HasDefaultProperty(TargetClass.StringProperty_PropertyName);

            A.CallTo(() => _TypeInfo.AddAttribute(A<System.ComponentModel.DefaultPropertyAttribute>.That.Matches(t => t.Name == TargetClass.StringProperty_PropertyName))).MustHaveHappened();
        }

        [TestMethod]
        public void HasObjectCaptionFormat_Should_Add_ObjectCaptionFormatAttribute_WithString()
        {
            
            _Builder.HasObjectCaptionFormat(TargetClass.StringProperty_Format);

            A.CallTo(() => _TypeInfo.AddAttribute(A<ObjectCaptionFormatAttribute>.That.Matches(t => t.FormatString == TargetClass.StringProperty_Format))).MustHaveHappened();
        }

        [TestMethod]
        public void HasObjectCaptionFormat_Should_Add_ObjectCaptionFormatAttribute_WithLambda()
        {
            _Builder.HasObjectCaptionFormat(p => p.StringProperty);

            A.CallTo(() => _TypeInfo.AddAttribute(A<ObjectCaptionFormatAttribute>.That.Matches(t => t.FormatString == TargetClass.StringProperty_Format))).MustHaveHappened();
        }
    }
}
