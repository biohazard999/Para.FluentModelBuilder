using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp.DC;

namespace Para.FluentModelBuilder.XAF
{
    public abstract class XafBuilderManager : BuilderManager
    {
        protected XafBuilderManager(ITypesInfo typesInfo)
        {
            var builders = BuildUpModel(typesInfo);

            _Builders.AddRange(builders);
            
            Build();

            foreach(var builder in _Builders.OfType<ITypeInfoProvider>())
                typesInfo.RefreshInfo(builder.TypeInfo);
        }

        public abstract IEnumerable<IBuilder> BuildUpModel(ITypesInfo typesInfo);
    }
}