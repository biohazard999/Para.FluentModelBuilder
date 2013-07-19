using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Para.FluentModelBuilder.XAF;

// ReSharper disable InconsistentNaming
namespace Para.FluentModelBuilder.Tests
{
    [TestClass]
    public class OneToReferencePropertySpecifications
    {
        private IMemberInfo _MemberInfo;

        private PropertyBuilder<ReferencedTargetClass, TargetClass> _Builder;

        [TestInitialize]
        public void Init()
        {
            _MemberInfo = A.Fake<IMemberInfo>();

            _Builder = new PropertyBuilder<ReferencedTargetClass, TargetClass>(_MemberInfo);
        }

        [TestMethod]
        public void UsingDataSourceProperty_Should_Add_DataSourcePropertyAttribute_With_Lambda()
        {
            _Builder.UsingDataSourceProperty(m => m.StringProperty, DataSourcePropertyIsNullMode.SelectAll, CriteriaOperator.Parse("1=1"));

            A.CallTo(() => _MemberInfo.AddAttribute(A<DataSourcePropertyAttribute>.That.Matches(t =>
                                                                                                     t.DataSourceProperty == "StringProperty" &&
                                                                                                     t.DataSourcePropertyIsNullMode == DataSourcePropertyIsNullMode.SelectAll &&
                                                                                                     t.DataSourcePropertyIsNullCriteria == "1 = 1"
                                                                                                ))).MustHaveHappened();
        }

        [TestMethod]
        public void UsingDataSourceProperty_Should_Add_DataSourcePropertyAttribute_With_String()
        {
            _Builder.UsingDataSourceProperty("StringProperty", DataSourcePropertyIsNullMode.SelectAll, CriteriaOperator.Parse("1=1"));

            A.CallTo(() => _MemberInfo.AddAttribute(A<DataSourcePropertyAttribute>.That.Matches(t =>
                                                                                                     t.DataSourceProperty == "StringProperty" &&
                                                                                                     t.DataSourcePropertyIsNullMode == DataSourcePropertyIsNullMode.SelectAll &&
                                                                                                     t.DataSourcePropertyIsNullCriteria == "1 = 1"
                                                                                                ))).MustHaveHappened();
        }
    }
}
