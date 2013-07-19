Para.FluentModelBuilder
=======================

Fluent Model Builder for the DevExpress.ExpressApp Framework


## Don't like polution of your XPO/EntityFramework assembly with XAF Attributes? ##

We have a solution:

Use flutent syntax to assign attributes to your existing XPO code.

Inspired by EntityFramework it can provide the metadata and attributes required by the XAF-Framework.


## But How? ##


Based on the [Fluent-Interface-Pattern](http://www.martinfowler.com/bliki/FluentInterface.html) it will cure the problem of dealing with String-Magic in Attributes for Criterias, PropertyNames and other ugly stuff in your ModelCode.


## Show me, Show me! ##

For our UnitTest Project we have this simple classes:


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


Don't be afraid, that it is not a XPO/EF class, but it will be recognized by the excelent typesystem of XAF (`ITypesInfo`, `ITypeInfo`, `IMemberInfo`, ect.)

For XAF we can capsulate the mapping in a derived class called `XafBuilderManager`:

	class TestXafModelBuilderManager : XafBuilderManager
	{
		public TestXafModelBuilderManager(ITypesInfo typesInfo) : base(typesInfo)
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

As you can see here we have 2 options to map our classes: as a seperate class derived from `ModelBuilder<T>` called `TargetClassBuilder` in this example, or map all the stuff in this class, with the fluent interface.

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
				.HasCaption("Date")
				.HasDisplayFormat("{0:dd.mm.yyyy")
				.IsImmediatePostData()
				.IsVisibleInDetailView()
				.IsNotVisibleInLookupListView()

			.UsingAppearance()
				.Targeting(m => m.StringProperty)
					.When(CriteriaOperator.Parse("DateTimeProperty == @Today()"))
					.IsNotEnabled()
					.HavingPriority(99);

			For(m => m.StringProperty)
				.AllowingDelete()
				.HasCaption("Bla");

			For(m => m.StringProperty)
				.UsingAppearance()
					.When("StringProperty == 'foo'")
					.TargetingAll()
					.ExceptingTarget(m => m.NullableDateTimeProperty);
		}
	}

### How do i tell XAF to use this? ###

Basically everything is there from the XAF-Team. We have this little method in our Modules we can override called `CustomizeTypesInfo`. Everything we need to do is to let the magic begin and call our constructor:


    public sealed partial class TestModule : ModuleBase
    {
        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);
            new TestXafModelBuilderManager(typesInfo);
        }
	}


### Cool, but what about our own Attributes? ###

Thats a great question:

There are 2 possible solutions:

Use `ModelBuilder<T>.WithAttribute(Attribute attribute)` or `ModelBuilder<T>.WithAttribute<TAttribute>(Action<TAttribute> attributeOptions)`.

	For(m => m.StringProperty)
		.WithAttribute(new YourProperty("whatever"));

Or:

Write a new Extention-Method that adds syntactic sugar to the whole thing (`ConditionalAppearance` is realized with this):

	public static class ConditionalAppearancePropertyBuilder
    {
        public static ConditionalAppearancePropertyBuilder<TProp, T> UsingAppearance<TProp, T>(this PropertyBuilder<TProp, T> builder, string shortId = "Visiblity")
        {
            var appearanceBuilder =  new ConditionalAppearancePropertyBuilder<TProp, T>(builder, shortId);

            (builder as IBuilderManager).AddBuilder(appearanceBuilder);

            return appearanceBuilder;
        }

    }

Don't focus on to much detail here, this just returns a new `IBuilder` for `PropertyBuilder<TProp, T>.UsingAppearance`


The implementation it self is very simple, but needs to handle the DefaultValues of the AppearanceAttribute, so it needs to use state, to init the `_AppearanceItemTypeValue` and the `AppearanceContext` correctly

    public class ConditionalAppearancePropertyBuilder<TProp, T> : IBuilder
    {
        private readonly PropertyBuilder<TProp, T> _Builder;

        private string _AppearanceItemTypeValue;

        private string _Context;

        internal AppearanceAttribute _Attribute;

        public ConditionalAppearancePropertyBuilder(PropertyBuilder<TProp, T> builder, string shortId)
        {
            _Builder = builder;
            _Attribute = new AppearanceAttribute(typeof(T).FullName + "." + builder.MemberInfo.Name + "." + shortId);
            builder.WithAttribute(_Attribute);
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> UsingForeColor(Color color)
        {
            var converter = System.ComponentModel.TypeDescriptor.GetConverter(color);
            _Attribute.FontColor = converter.ConvertToString(color);
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> UsingBackColor(Color color)
        {
            var converter = System.ComponentModel.TypeDescriptor.GetConverter(color);
            _Attribute.BackColor = converter.ConvertToString(color);
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> UsingFontStyle(FontStyle style)
        {
            _Attribute.FontStyle = style;
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> HavingPriority(int priority)
        {
            _Attribute.Priority = priority;
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> IsVisible()
        {
            _Attribute.Visibility = ViewItemVisibility.Show;
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> IsVisibleAsEmptySpace()
        {
            _Attribute.Visibility = ViewItemVisibility.ShowEmptySpace;
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> IsNotVisible()
        {
            _Attribute.Visibility = ViewItemVisibility.Hide;
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> IsEnabled()
        {
            _Attribute.Enabled = true;
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> IsNotEnabled()
        {
            _Attribute.Enabled = false;
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> When(string criteria)
        {
            _Attribute.Criteria = criteria;
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> When(CriteriaOperator criteria)
        {
            _Attribute.Criteria = criteria.ToString();
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> ForItemsOfType(AppearanceItemType appearanceItemType)
        {
            return ForItemsOfType(appearanceItemType.ToString());
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> ForItemsOfType(string appearanceItemType)
        {
            _AppearanceItemTypeValue = _AppearanceItemTypeValue.AppendString(appearanceItemType);
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> ForLayoutItems()
        {
            return ForItemsOfType(AppearanceItemType.LayoutItem);
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> ForViewItems()
        {
            return ForItemsOfType(AppearanceItemType.ViewItem);
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> ForActions()
        {
            return ForItemsOfType(AppearanceItemType.Action);
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> InTheContextOf(string context)
        {
            _Context = _Context.AppendString(context);
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> InDetailViewContext()
        {
            return InTheContextOf(ViewType.DetailView.ToString());
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> InAnyContext()
        {
            return InTheContextOf(ViewType.Any.ToString());
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> InListViewContext()
        {
            return InTheContextOf(ViewType.ListView.ToString());
        }

        public PropertyBuilder<TProp, T> Build()
        {
            return _Builder;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> Targeting(string property)
        {
            _Attribute.TargetItems = _Attribute.TargetItems.AppendString(property);
            return this;
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> TargetingAll()
        {
            return Targeting("*");
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> ExceptingTarget(string property)
        {
            return Targeting(property);
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> Targeting<TProp2>(Expression<Func<T, TProp2>> property)
        {
            return Targeting(_Builder._Fields.GetPropertyName(property));
        }

        public ConditionalAppearancePropertyBuilder<TProp, T> ExceptingTarget<TProp2>(Expression<Func<T, TProp2>> property)
        {
            return ExceptingTarget(_Builder._Fields.GetPropertyName(property));
        }

        void IBuilder.Build()
        {
            if (!string.IsNullOrEmpty(_AppearanceItemTypeValue))
                _Attribute.AppearanceItemType = _AppearanceItemTypeValue;

            if (!string.IsNullOrEmpty(_Context))
                _Attribute.Context = _Context;
        }
    }

##Where do i get it?##

Currently it is easy as brush your teeth using NuGet:

- For XAF-Only Attributes:

`Install-Package Para.FluentModelBuilder.XAF`

- For the ConditionalAppearance part use:

`Install-Package Para.FluentModelBuilder.ConditionalAppearance`

##Questions?##

Currently this is very alpha stuff, but it works brilliant for this easy example so far :)

Feel free to contact, fork or ask me for questions on twitter, facebook or Email:

- Twitter: https://twitter.com/biohaz999
- FB: https://www.facebook.com/manuel.grundner
- E-Mail: m.grundner at paragraph-software dot at


