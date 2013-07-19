namespace Para.FluentModelBuilder.XAF
{
    public interface IBuilderManager : IBuilder
    {
        void AddBuilder(IBuilder builder);
    }
}