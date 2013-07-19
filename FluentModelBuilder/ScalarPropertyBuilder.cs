using System;

namespace Para.FluentModelBuilder.XAF
{
    public static class ScalarPropertyBuilder
    {
        public static PropertyBuilder<string, T> HasEditMask<T>(this PropertyBuilder<string, T> builder, string editMask)
        {
            return builder.WithModelDefault(ModelDefaultKeys.EditMask.EditMaskKey, editMask);
        }

        public static PropertyBuilder<int, T> HasEditMask<T>(this PropertyBuilder<int, T> builder, string editMask)
        {
            return builder.WithModelDefault(ModelDefaultKeys.EditMask.EditMaskKey, editMask);
        }

        public static PropertyBuilder<double, T> HasEditMask<T>(this PropertyBuilder<double, T> builder, string editMask)
        {
            return builder.WithModelDefault(ModelDefaultKeys.EditMask.EditMaskKey, editMask);
        }

        public static PropertyBuilder<TimeSpan, T> HasEditMask<T>(this PropertyBuilder<TimeSpan, T> builder, string editMask)
        {
            return builder.WithModelDefault(ModelDefaultKeys.EditMask.EditMaskKey, editMask);
        }

        public static PropertyBuilder<DateTime, T> HasEditMask<T>(this PropertyBuilder<DateTime, T> builder, string editMask)
        {
            return builder.WithModelDefault(ModelDefaultKeys.EditMask.EditMaskKey, editMask);
        }

        public static PropertyBuilder<DateTime?, T> HasEditMask<T>(this PropertyBuilder<DateTime?,T> builder, string editMask)
        {
            return builder.WithModelDefault(ModelDefaultKeys.EditMask.EditMaskKey, editMask);
        }
    }
}