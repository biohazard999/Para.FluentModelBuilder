using System;

namespace Para.FluentModelBuilder.Tests
{
    // ReSharper disable InconsistentNaming
    class TargetClass
    {
        internal const string StringProperty_PropertyName = "StringProperty";

        internal const string StringProperty_Format = "{0:" + StringProperty_PropertyName + "}";

        internal string StringProperty { get; set; }

        internal DateTime DateTimeProperty { get; set; }

        internal DateTime? NullableDateTimeProperty { get; set; }

        internal ReferencedTargetClass OneToReferencedTarget { get; set; }
    }

    class ReferencedTargetClass
    {
        public string Prop { get; set; }
    }
}