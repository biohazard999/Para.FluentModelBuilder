using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using Para.FluentModelBuilder.ConditionalAppearance;
using Para.FluentModelBuilder.XAF;

namespace Para.FluentModelBuilder.Tests.IntegrationTest
{
    class TargetClassBuilder : ModelBuilder<TargetClass>
    {
        public TargetClassBuilder(ITypesInfo typesInfo) : base(typesInfo)
        {
        }

        public TargetClassBuilder(ITypeInfo typeInfo) : base(typeInfo)
        {
        }

        protected override void BuildUp()
        {
            HasCaption("Test");

            For(m => m.DateTimeProperty)
                .HasCaption("Datum")
                .HasDisplayFormat("{0:dd.mm.yyyy")
                .IsImmediatePostData()
                .IsVisibleInDetailView()
                .IsNotVisibleInLookupListView()

                .UsingAppearance()
                    .Targeting(m => m.StringProperty)
                    .When(CriteriaOperator.Parse("1 = 2"))
                    .HavingPriority(99);


            For(m => m.StringProperty)
                .AllowingDelete()
                .HasCaption("Bla");

            For(m => m.StringProperty)
                .UsingAppearance()
                .When("1 = 2")
                .TargetingAll()
                .ExceptingTarget(m => m.NullableDateTimeProperty);
        }
    }
}