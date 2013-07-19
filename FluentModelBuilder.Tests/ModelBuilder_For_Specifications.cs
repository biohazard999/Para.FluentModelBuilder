using System;
using DevExpress.ExpressApp.DC;
using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Para.FluentModelBuilder.XAF;

// ReSharper disable InconsistentNaming
namespace Para.FluentModelBuilder.Tests
{
    [TestClass]
    public class ModelBuilder_For_Specifications
    {
        private ITypeInfo _TypeInfo;

        private ITypesInfo _TypesInfo;

        private ModelBuilder<TargetClass> _Builder;

        private IMemberInfo _MemberInfo;

        private readonly Fields<TargetClass> _Fields = new Fields<TargetClass>();

        [TestInitialize]
        public void Init()
        {
            _TypesInfo = A.Fake<ITypesInfo>();
            _TypeInfo = A.Fake<ITypeInfo>();
            _MemberInfo = A.Fake<IMemberInfo>();

            A.CallTo(() => _TypesInfo.FindTypeInfo(A<Type>.Ignored)).Returns(_TypeInfo);

            _Builder = ModelBuilder.Create<TargetClass>(_TypesInfo);
        }

        [TestMethod]
        public void StringProperty_Should_Return_Instance_Of_StringPropertyBuilder()
        {
            A.CallTo(() => _TypeInfo.FindMember(_Fields.GetPropertyName(m => m.StringProperty))).Returns(_MemberInfo);
            
            A.CallTo(() => _MemberInfo.MemberType).Returns(typeof (string));

            var propertyBuilder = _Builder.For(m => m.StringProperty);

            propertyBuilder.Should().BeAssignableTo<PropertyBuilder<string, TargetClass>>();
        }

        [TestMethod]
        public void DateTimeProperty_Should_Return_Instance_Of_DateTimePropertyBuilder()
        {
            A.CallTo(() => _TypeInfo.FindMember(_Fields.GetPropertyName(m => m.DateTimeProperty))).Returns(_MemberInfo);

            A.CallTo(() => _MemberInfo.MemberType).Returns(typeof(DateTime));

            var propertyBuilder = _Builder.For(m => m.DateTimeProperty);

            propertyBuilder.Should().BeAssignableTo<PropertyBuilder<DateTime, TargetClass>>();
        }

        [TestMethod]
        public void NullableDateTimeProperty_Should_Return_Instance_Of_DateTimePropertyBuilder()
        {
            A.CallTo(() => _TypeInfo.FindMember(_Fields.GetPropertyName(m => m.NullableDateTimeProperty))).Returns(_MemberInfo);

            A.CallTo(() => _MemberInfo.MemberType).Returns(typeof(DateTime?));

            var propertyBuilder = _Builder.For(m => m.NullableDateTimeProperty);

            propertyBuilder.Should().BeAssignableTo<PropertyBuilder<DateTime?, TargetClass>>();
        }

        [TestMethod]
        public void ReferenceProperty_Should_Return_Instance_Of_OneToReferencePropertyBuilder()
        {
            A.CallTo(() => _TypeInfo.FindMember(_Fields.GetPropertyName(m => m.OneToReferencedTarget))).Returns(_MemberInfo);

            A.CallTo(() => _MemberInfo.MemberType).Returns(typeof(ReferencedTargetClass));

            A.CallTo(() => _MemberInfo.MemberTypeInfo).ReturnsLazily(() =>
                                                               {
                                                                   var info = A.Fake<ITypeInfo>();

                                                                   A.CallTo(() => info.IsValueType).Returns(false);

                                                                   return info;
                                                               });
            
            var propertyBuilder = _Builder.For(m => m.OneToReferencedTarget);

            propertyBuilder.Should().BeAssignableTo<PropertyBuilder<ReferencedTargetClass, TargetClass>>();
        }

        [TestMethod]
        public void Build_Should_Call_BuilderLists_Build()
        {
            var fakeBuilder = A.Fake<IBuilder>();

            (_Builder as IBuilderManager).AddBuilder(fakeBuilder);

            (_Builder as IBuilderManager).Build();

            A.CallTo(() => fakeBuilder.Build()).MustHaveHappened();
        }

        [TestMethod]
        public void Calling_For_Adds_PropertyBuilder_To_BuilderList()
        {
            var builderTarget = _Builder.For(m => m.OneToReferencedTarget);

            _Builder._Builders.Should().Contain(builderTarget);
        }
    }
}