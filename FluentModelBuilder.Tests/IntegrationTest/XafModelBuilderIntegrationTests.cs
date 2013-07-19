using System;
using DevExpress.ExpressApp.DC;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Para.FluentModelBuilder.Tests.IntegrationTest
{
    [TestClass]
    public class XafModelBuilderIntegrationTests
    {
        [TestMethod]
        public void TestXafModelBuilder_Should_Buildup_Model()
        {
            var fakeTypesInfo = A.Fake<ITypesInfo>();
            
            var targetClassTypeInfo = A.Fake<ITypeInfo>();
            var referencedTargetClass = A.Fake<ITypeInfo>();

            var dateTimePropertyMemberInfo = A.Fake<IMemberInfo>();
            var stringPropertyMemberInfo = A.Fake<IMemberInfo>();
            var propMemberInfo = A.Fake<IMemberInfo>();

            A.CallTo(() => fakeTypesInfo.FindTypeInfo(typeof(TargetClass))).Returns(targetClassTypeInfo);
            A.CallTo(() => fakeTypesInfo.FindTypeInfo(typeof(ReferencedTargetClass))).Returns(referencedTargetClass);

            A.CallTo(() => targetClassTypeInfo.FindMember("DateTimeProperty")).Returns(dateTimePropertyMemberInfo);
            A.CallTo(() => targetClassTypeInfo.FindMember("StringProperty")).Returns(stringPropertyMemberInfo);
            A.CallTo(() => referencedTargetClass.FindMember("Prop")).Returns(propMemberInfo);
            
            new TestXafModelBuilder(fakeTypesInfo);

            A.CallTo(() => targetClassTypeInfo.AddAttribute(A<Attribute>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

            A.CallTo(() => dateTimePropertyMemberInfo.AddAttribute(A<Attribute>.Ignored)).MustHaveHappened(Repeated.Exactly.Times(6));
            A.CallTo(() => stringPropertyMemberInfo.AddAttribute(A<Attribute>.Ignored)).MustHaveHappened(Repeated.Exactly.Times(3));
            A.CallTo(() => propMemberInfo.AddAttribute(A<Attribute>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
            
            A.CallTo(() => fakeTypesInfo.RefreshInfo(A<ITypeInfo>.Ignored)).MustHaveHappened(Repeated.Exactly.Twice);
        }
    }
}
