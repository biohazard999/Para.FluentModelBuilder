using System.Drawing;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Para.FluentModelBuilder.ConditionalAppearance;
using Para.FluentModelBuilder.XAF;

// ReSharper disable InconsistentNaming
namespace Para.FluentModelBuilder.Tests.ConditionalAppearanceTests
{
    public static class ConditionalAppearancePropertyBuilderHelpers
    {
        public static void BuildAsBuilder(this IBuilder builder)
        {
            builder.Build();
        }
    }
    [TestClass]
    public class ConditionalAppearancePropertyBuilderTests
    {
        private IMemberInfo _MemberInfo;

        private PropertyBuilder<ReferencedTargetClass, TargetClass> _Builder;

        private ConditionalAppearancePropertyBuilder<ReferencedTargetClass, TargetClass> _AppearanceBuilder;

        [TestInitialize]
        public void Init()
        {
            _MemberInfo = A.Fake<IMemberInfo>();
            _Builder = new PropertyBuilder<ReferencedTargetClass, TargetClass>(_MemberInfo);

            A.CallTo(() => _MemberInfo.Name).Returns("OneToReferencedTarget");

            _AppearanceBuilder = _Builder.UsingAppearance();
        }

        [TestMethod]
        public void IdGeneration_Should_Be_Correct()
        {
            _AppearanceBuilder.BuildAsBuilder();

            _AppearanceBuilder._Attribute.Id.Should().Be("Para.FluentModelBuilder.Tests.TargetClass.OneToReferencedTarget.Visiblity");
        }

        [TestMethod]
        public void UsingForeColor_Should_Build_With_Value_Crimson()
        {
            _AppearanceBuilder.UsingForeColor(Color.Crimson).BuildAsBuilder();

            _AppearanceBuilder._Attribute.FontColor.Should().Be("Color [Crimson]");
        }

        [TestMethod]
        public void UsingBackColor_Should_Build_With_Value_DarkRed()
        {
            _AppearanceBuilder.UsingBackColor(Color.DarkRed).BuildAsBuilder();

            _AppearanceBuilder._Attribute.BackColor.Should().Be("Color [DarkRed]");
        }

        [TestMethod]
        public void UsingFontStyle_Should_Build_With_Value_Regular()
        {
            _AppearanceBuilder.UsingFontStyle(FontStyle.Regular).BuildAsBuilder();

            _AppearanceBuilder._Attribute.FontStyle.Should().Be(FontStyle.Regular);
        }

        [TestMethod]
        public void HavingPriority_Should_Build_With_Value_99()
        {
            _AppearanceBuilder.HavingPriority(99).BuildAsBuilder();

            _AppearanceBuilder._Attribute.Priority.Should().Be(99);
        }

        [TestMethod]
        public void IsVisible_Should_Build_With_Value_ViewItemVisibility_Show()
        {
            _AppearanceBuilder.IsVisible().BuildAsBuilder();

            _AppearanceBuilder._Attribute.Visibility.Should().Be(ViewItemVisibility.Show);
        }

        [TestMethod]
        public void IsNotVisible_Should_Build_With_Value_ViewItemVisibility_Hide()
        {
            _AppearanceBuilder.IsNotVisible().BuildAsBuilder();

            _AppearanceBuilder._Attribute.Visibility.Should().Be(ViewItemVisibility.Hide);
        }

        [TestMethod]
        public void IsVisibleAsEmptySpace_Should_Build_With_Value_ViewItemVisibility_ShowEmptySpace()
        {
            _AppearanceBuilder.IsVisibleAsEmptySpace().BuildAsBuilder();

            _AppearanceBuilder._Attribute.Visibility.Should().Be(ViewItemVisibility.ShowEmptySpace);
        }

        [TestMethod]
        public void IsEnabled_Should_Build_With_Value_Enabled_True()
        {
            _AppearanceBuilder.IsEnabled().BuildAsBuilder();

            _AppearanceBuilder._Attribute.Enabled.Should().BeTrue();
        }

        [TestMethod]
        public void IsNotEnabled_Should_Build_With_Value_Enabled_False()
        {
            _AppearanceBuilder.IsNotEnabled().BuildAsBuilder();

            _AppearanceBuilder._Attribute.Enabled.Should().BeFalse();
        }

        [TestMethod]
        public void When_Should_Build_With_CorrectCritieria_String()
        {
            _AppearanceBuilder.When("1 = 1").BuildAsBuilder();

            _AppearanceBuilder._Attribute.Criteria.Should().Be("1 = 1");
        }

        [TestMethod]
        public void When_Should_Build_With_CorrectCritieria_CriteriaOperator()
        {
            _AppearanceBuilder.When(CriteriaOperator.Parse("1 = 1")).BuildAsBuilder();

            _AppearanceBuilder._Attribute.Criteria.Should().Be("1 = 1");
        }

        [TestMethod]
        public void ForItemsOfType_Should_Build_With_Correct_ItemTypes_Given_String()
        {
            _AppearanceBuilder.ForItemsOfType(AppearanceItemType.Action).BuildAsBuilder();

            _AppearanceBuilder._Attribute.AppearanceItemType.Should().Be("Action");
        }

        [TestMethod]
        public void ForItemsOfType_Should_Build_With_DefaultValue_ViewItem()
        {
            _AppearanceBuilder.BuildAsBuilder();
            _AppearanceBuilder._Attribute.AppearanceItemType.Should().Be("ViewItem");
        }

        [TestMethod]
        public void ForLayoutItems_Should_Build_With_Correct_ItemTypes_Given_String()
        {
            _AppearanceBuilder.ForLayoutItems().BuildAsBuilder();

            _AppearanceBuilder._Attribute.AppearanceItemType.Should().Be("LayoutItem");
        }

        [TestMethod]
        public void ForViewItems_Should_Build_With_Correct_ItemTypes_Given_String()
        {
            _AppearanceBuilder.ForViewItems().BuildAsBuilder();

            _AppearanceBuilder._Attribute.AppearanceItemType.Should().Be("ViewItem");
        }

        [TestMethod]
        public void ForActions_Should_Build_With_Correct_ItemTypes_Given_String()
        {
            _AppearanceBuilder.ForActions().BuildAsBuilder();

            _AppearanceBuilder._Attribute.AppearanceItemType.Should().Be("Action");
        }

        [TestMethod]
        public void Combined_TargetItemType_Should_Build_With_Correct_ItemTypes()
        {
            _AppearanceBuilder.ForActions().ForLayoutItems().ForViewItems().BuildAsBuilder();

            _AppearanceBuilder._Attribute.AppearanceItemType.Should().Be("Action;LayoutItem;ViewItem");
        }

        [TestMethod]
        public void InTheContextOf_Should_Build_With_Correct_ItemTypes_Given_String()
        {
            _AppearanceBuilder.InTheContextOf("TestContext").BuildAsBuilder();

            _AppearanceBuilder._Attribute.Context.Should().Be("TestContext");
        }

        [TestMethod]
        public void InTheContextOf_Should_Build_With_Default_Value()
        {
            _AppearanceBuilder.BuildAsBuilder();

            _AppearanceBuilder._Attribute.Context.Should().Be("Any");
        }

        [TestMethod]
        public void InAnyContext_Should_Build_With_Correct_ItemTypes_Given_String()
        {
            _AppearanceBuilder.InAnyContext().BuildAsBuilder();

            _AppearanceBuilder._Attribute.Context.Should().Be("Any");
        }

        [TestMethod]
        public void InDetailViewContext_Should_Build_With_Correct_ItemTypes_Given_String()
        {
            _AppearanceBuilder.InDetailViewContext().BuildAsBuilder();

            _AppearanceBuilder._Attribute.Context.Should().Be("DetailView");
        }

        [TestMethod]
        public void InListViewContext_Should_Build_With_Correct_ItemTypes_Given_String()
        {
            _AppearanceBuilder.InListViewContext().BuildAsBuilder();

            _AppearanceBuilder._Attribute.Context.Should().Be("ListView");
        }

        [TestMethod]
        public void Combined_InContextOf_Should_Build_With_Correct_ItemTypes()
        {
            _AppearanceBuilder.InTheContextOf("Test").InAnyContext().InDetailViewContext().InListViewContext().BuildAsBuilder();

            _AppearanceBuilder._Attribute.Context.Should().Be("Test;Any;DetailView;ListView");
        }

        [TestMethod]
        public void Targeting_Should_Build_With_Correct_Targets_Given_String()
        {
            _AppearanceBuilder.Targeting("TestProperty").BuildAsBuilder();
            
            _AppearanceBuilder._Attribute.TargetItems.Should().Be("TestProperty");
        }

        [TestMethod]
        public void Targeting_Should_Build_With_Correct_Targets_Given_Lambda()
        {
            _AppearanceBuilder.Targeting(p => p.DateTimeProperty).BuildAsBuilder();

            _AppearanceBuilder._Attribute.TargetItems.Should().Be("DateTimeProperty");
        }

        [TestMethod]
        public void TargetingAll_Should_Build_With_Correct_Targets()
        {
            _AppearanceBuilder.TargetingAll().BuildAsBuilder();

            _AppearanceBuilder._Attribute.TargetItems.Should().Be("*");
        }

        [TestMethod]
        public void ExceptingTarget_Should_Build_With_Correct_Targets_Given_String()
        {
            _AppearanceBuilder.ExceptingTarget("ExceptedProperty").BuildAsBuilder();

            _AppearanceBuilder._Attribute.TargetItems.Should().Be("ExceptedProperty");
        }

        [TestMethod]
        public void ExceptingTarget_Should_Build_With_Correct_Targets_Given_Lambda()
        {
            _AppearanceBuilder.ExceptingTarget(p => p.NullableDateTimeProperty).BuildAsBuilder();

            _AppearanceBuilder._Attribute.TargetItems.Should().Be("NullableDateTimeProperty");
        }

        [TestMethod]
        public void CombindedTarget_Should_Build_With_Correct_Targets_Given_Lambda()
        {
            _AppearanceBuilder.TargetingAll().ExceptingTarget(p => p.NullableDateTimeProperty).ExceptingTarget(p => p.OneToReferencedTarget).BuildAsBuilder();

            _AppearanceBuilder._Attribute.TargetItems.Should().Be("*;NullableDateTimeProperty;OneToReferencedTarget");
        }

        [TestMethod]
        public void Calling_UsingAppearance_Adds_PropertyBuilder_To_BuilderList()
        {
            var builderTarget = _Builder.UsingAppearance();

            _Builder._Builders.Should().Contain(builderTarget);
        }
    }
}