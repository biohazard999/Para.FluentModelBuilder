using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Para.FluentModelBuilder.XAF;
using Ploeh.AutoFixture;

namespace Para.FluentModelBuilder.Tests
{
    [TestClass]
    public class StringPropertyBuilderSpecifications
    {
        private IMemberInfo _MemberInfo;

        private PropertyBuilder<string, object> _Builder;

        [TestInitialize]
        public void Init()
        {
            _MemberInfo = A.Fake<IMemberInfo>();

            _Builder = new PropertyBuilder<string, object>(_MemberInfo);
        }


        [TestMethod]
        public void HasRegExEditMask_Should_Add_ModelDefaultAttribute_With_EditMaskTypePropertyName_And_RegExPropertyName()
        {
            _Builder.HasRegExEditMask(string.Empty);

            A.CallTo(() => _MemberInfo.AddAttribute(A<ModelDefaultAttribute>.That.Matches(t => t.PropertyName == ModelDefaultKeys.EditMask.EditMaskType && t.PropertyValue == "RegEx"))).MustHaveHappened();
        }

        [TestMethod]
        public void HasRegExEditMask_Should_Add_ModelDefaultAttribute_With_EditMaskPropertyName_And_MyRegEx()
        {
            var editMask = new Fixture().Create<string>();
            _Builder.HasRegExEditMask(editMask);

            A.CallTo(() => _MemberInfo.AddAttribute(A<ModelDefaultAttribute>.That.Matches(t => t.PropertyName == ModelDefaultKeys.EditMask.EditMaskKey && t.PropertyValue == editMask))).MustHaveHappened();
        }
    }
}