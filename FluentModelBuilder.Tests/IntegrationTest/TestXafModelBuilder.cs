using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using Para.FluentModelBuilder.XAF;

namespace Para.FluentModelBuilder.Tests.IntegrationTest
{
    class TestXafModelBuilder : XafBuilderManager
    {
        public TestXafModelBuilder(ITypesInfo typesInfo) : base(typesInfo)
        {
        }
        
        public override IEnumerable<IBuilder> BuildUpModel(ITypesInfo typesInfo)
        {
            yield return new TargetClassBuilder(typesInfo);

            var builder2 = ModelBuilder.Create<ReferencedTargetClass>(typesInfo);

            builder2.For(m => m.Prop)
                .HasCaption("Test");

            yield return builder2;
        }
    }
}