using System.Collections.Generic;

namespace Para.FluentModelBuilder.XAF
{
    public class BuilderManager : IBuilderManager
    {
        internal readonly List<IBuilder> _Builders = new List<IBuilder>();

        public void Build()
        {
            _Builders.ForEach(m => m.Build());
        }

        public void AddBuilder(IBuilder builder)
        {
            _Builders.Add(builder);
        }
    }
}