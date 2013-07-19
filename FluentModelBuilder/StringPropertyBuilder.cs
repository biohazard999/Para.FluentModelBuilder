namespace Para.FluentModelBuilder.XAF
{
    public static class StringPropertyBuilder
    {
        public static PropertyBuilder<string, T> HasRegExEditMask<T>(this PropertyBuilder<string, T> builder, string regEx)
        {
            return builder.HasEditMask(regEx).WithModelDefault(ModelDefaultKeys.EditMask.EditMaskType, ModelDefaultKeys.EditMask.Types.RegEx);
        }
    }
}