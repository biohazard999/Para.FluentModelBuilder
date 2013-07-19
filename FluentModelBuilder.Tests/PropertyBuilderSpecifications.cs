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
    public class PropertyBuilderSpecifications
    {
        private IMemberInfo _MemberInfo;

        private PropertyBuilder<object, object> _Builder;

        [TestInitialize]
        public void Init()
        {
            _MemberInfo = A.Fake<IMemberInfo>();

            _Builder = new PropertyBuilder<object, object>(_MemberInfo);
        }

        [TestMethod]
        public void WithAttribute_Should_Return_Same_PropertyBuilder()
        {
            var result = _Builder.WithAttribute(new WithPublicCtorAttribute());
            result.Should().Be(_Builder);
        }

        [TestMethod]
        public void WithAttribute_Should_Add_Attribute()
        {
            var attribute = new WithPublicCtorAttribute();
            _Builder.WithAttribute(attribute);

            A.CallTo(() => _MemberInfo.AddAttribute(A<WithPublicCtorAttribute>.That.Matches(a => Equals(a, attribute)))).MustHaveHappened();
        }

        [TestMethod]
        public void WithAttributeOfT_Should_Add_NewAttribute()
        {
            _Builder.WithAttribute<WithPublicCtorAttribute>();

            A.CallTo(() => _MemberInfo.AddAttribute(A<WithPublicCtorAttribute>.That.IsInstanceOf(typeof(WithPublicCtorAttribute)))).MustHaveHappened();
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
        public void WithModelDefault_Should_Add_ModelDefaultAttribute()
        {
            var customValue = new Fixture().Create<string>();
            var customPropertyName = new Fixture().Create<string>();

            _Builder.WithModelDefault(customPropertyName, customValue);

            A.CallTo(() => _MemberInfo.AddAttribute(A<ModelDefaultAttribute>.That.Matches(t => t.PropertyName == customPropertyName && t.PropertyValue == customValue))).MustHaveHappened();
        }

        [TestMethod]
        public void HasCaption_Should_Add_ModelDefaultAttribute_With_CaptionPropertyName_And_ValueMyCaption()
        {
            var caption = new Fixture().Create<string>();
            _Builder.HasCaption(caption);

            A.CallTo(() => _MemberInfo.AddAttribute(A<ModelDefaultAttribute>.That.Matches(t => t.PropertyName == ModelDefaultKeys.Caption && t.PropertyValue == caption))).MustHaveHappened();
        }

        [TestMethod]
        public void HasDisplayFormat_Should_Add_ModelDefaultAttribute_With_DisplayFormatPropertyName_And_MyDisplayFormat()
        {
            const string DisplayFormat = "{0:yy-MM-dd}";
            _Builder.HasDisplayFormat(DisplayFormat);

            A.CallTo(() => _MemberInfo.AddAttribute(A<ModelDefaultAttribute>.That.Matches(t => t.PropertyName == ModelDefaultKeys.DisplayFormat && t.PropertyValue == DisplayFormat))).MustHaveHappened();
        }

        [TestMethod]
        public void IsVisibleInDetailView_Should_Add_VisibleInDetailView_With_Value_True()
        {
            _Builder.IsVisibleInDetailView();

            A.CallTo(() => _MemberInfo.AddAttribute(A<VisibleInDetailViewAttribute>.That.Matches(t => ((bool)t.Value) == true))).MustHaveHappened();
        }

        [TestMethod]
        public void IsVisibleInDetailView_Should_Add_VisibleInDetailView_With_Value_False()
        {
            _Builder.IsVisibleInDetailView(false);

            A.CallTo(() => _MemberInfo.AddAttribute(A<VisibleInDetailViewAttribute>.That.Matches(t => ((bool)t.Value) == false))).MustHaveHappened();
        }

        [TestMethod]
        public void IsVisibleInListView_Should_Add_VisibleInListView_With_Value_True()
        {
            _Builder.IsVisibleInListView();

            A.CallTo(() => _MemberInfo.AddAttribute(A<VisibleInListViewAttribute>.That.Matches(t => ((bool)t.Value) == true))).MustHaveHappened();
        }

        [TestMethod]
        public void IsVisibleInListView_Should_Add_VisibleInListView_With_Value_False()
        {
            _Builder.IsVisibleInListView(false);

            A.CallTo(() => _MemberInfo.AddAttribute(A<VisibleInListViewAttribute>.That.Matches(t => ((bool)t.Value) == false))).MustHaveHappened();
        }

        [TestMethod]
        public void IsVisibleInLookupListView_Should_Add_VisibleInLookupListView_With_Value_True()
        {
            _Builder.IsVisibleInLookupListView();

            A.CallTo(() => _MemberInfo.AddAttribute(A<VisibleInLookupListViewAttribute>.That.Matches(t => ((bool)t.Value) == true))).MustHaveHappened();
        }

        [TestMethod]
        public void IsVisibleInLookupListView_Should_Add_VisibleInLookupListView_With_Value_False()
        {
            _Builder.IsVisibleInLookupListView(false);

            A.CallTo(() => _MemberInfo.AddAttribute(A<VisibleInLookupListViewAttribute>.That.Matches(t => ((bool)t.Value) == false))).MustHaveHappened();
        }

        [TestMethod]
        public void IsVisibleInAnyView_Should_Add_AllVisibleAttributes_With_Value_True()
        {
            _Builder.IsVisibleInAnyView();

            A.CallTo(() => _MemberInfo.AddAttribute(A<Attribute>.Ignored)).MustHaveHappened(Repeated.Exactly.Times(3));
        }

        [TestMethod]
        public void IsVisibleInAnyView_Should_Add_AllVisibleAttributes_With_Value_False()
        {
            _Builder.IsVisibleInAnyView(false);

            A.CallTo(() => _MemberInfo.AddAttribute(A<Attribute>.Ignored)).MustHaveHappened(Repeated.Exactly.Times(3));
        }

        [TestMethod]
        public void IsNotVisibleInAnyView_Should_Add_AllVisibleAttributes_With_Value_False()
        {
            _Builder.IsNotVisibleInAnyView();

            A.CallTo(() => _MemberInfo.AddAttribute(A<Attribute>.Ignored)).MustHaveHappened(Repeated.Exactly.Times(3));
        }


        [TestMethod]
        public void UsingPropertyEditor_Should_Add_ModelDefaultAttribute_With_PropertyEditorTypePropertyName_And_MyPropertyEditorTypePropertyName_WithString()
        {
            var propertyEditorTypeName = new Fixture().Create<string>();

            _Builder.UsingPropertyEditor(propertyEditorTypeName);

            A.CallTo(() => _MemberInfo.AddAttribute(A<ModelDefaultAttribute>.That.Matches(t => t.PropertyName == ModelDefaultKeys.PropertyEditorType && t.PropertyValue == propertyEditorTypeName))).MustHaveHappened();
        }

        [TestMethod]
        public void UsingPropertyEditorOfT_Should_Add_ModelDefaultAttribute_With_PropertyEditorTypePropertyName_And_MyPropertyEditorTypePropertyName()
        {
            _Builder.UsingPropertyEditor<TargetClass>();

            A.CallTo(() => _MemberInfo.AddAttribute(A<ModelDefaultAttribute>.That.Matches(t => t.PropertyName == ModelDefaultKeys.PropertyEditorType && t.PropertyValue == typeof(TargetClass).FullName))).MustHaveHappened();
        }

        [TestMethod]
        public void UsingPropertyEditor_Should_Add_ModelDefaultAttribute_With_PropertyEditorTypePropertyName_And_MyPropertyEditorTypePropertyName_WithType()
        {
            Type PropertyEditorType = typeof(TargetClass);

            _Builder.UsingPropertyEditor(PropertyEditorType);

            A.CallTo(() => _MemberInfo.AddAttribute(A<ModelDefaultAttribute>.That.Matches(t => t.PropertyName == ModelDefaultKeys.PropertyEditorType && t.PropertyValue == PropertyEditorType.FullName))).MustHaveHappened();
        }

        [TestMethod]
        public void IsImmediatePostData_Should_Add_ImmediatePostDataAttribute_()
        {
            _Builder.IsImmediatePostData();

            A.CallTo(() => _MemberInfo.AddAttribute(A<ImmediatePostDataAttribute>.That.Matches(t => (bool)t.Value))).MustHaveHappened();
        }

        [TestMethod]
        public void Build_Should_Call_BuilderLists_Build()
        {
            var fakeBuilder = A.Fake<IBuilder>();

            (_Builder as IBuilderManager).AddBuilder(fakeBuilder);

            (_Builder as IBuilderManager).Build();

            A.CallTo(() => fakeBuilder.Build()).MustHaveHappened();
        }
    }
}