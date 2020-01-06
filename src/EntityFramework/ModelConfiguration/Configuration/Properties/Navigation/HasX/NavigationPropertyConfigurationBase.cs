// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.ModelConfiguration.Configuration
{
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation;
    using System.Data.Entity.Utilities;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    /// <summary>
    /// Configures an relationship from an entity type.
    /// </summary>
    public abstract class NavigationPropertyConfigurationBase
    {
        internal readonly NavigationPropertyConfiguration NavigationPropertyConfiguration;

        /// <summary>
        /// Gets or sets the multiplicity of this end of the navigation property.
        /// </summary>
        public RelationshipMultiplicity RelationshipMultiplicity
        {
            get => NavigationPropertyConfiguration.RelationshipMultiplicity.GetValueOrDefault();
            set => NavigationPropertyConfiguration.RelationshipMultiplicity = value;
        }

        /// <summary>
        /// Configures the relationship to be many without a navigation property on the other side of the relationship.
        /// </summary>
        /// <returns> A configuration object that can be used to further configure the relationship. </returns>
        public DependentNavigationPropertyConfiguration WithMany()
        {
            NavigationPropertyConfiguration.InverseEndKind = RelationshipMultiplicity.Many;

            return new DependentNavigationPropertyConfiguration(NavigationPropertyConfiguration);
        }

        /// <summary>
        /// Configures the relationship to be many with a navigation property on the other side of the relationship.
        /// </summary>
        /// <param name="navigationPropertyInfo"> An propertyInfo representing the navigation property (collection) on the other end of the relationship.  </param>
        /// <returns> A configuration object that can be used to further configure the relationship. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public DependentNavigationPropertyConfiguration WithMany(PropertyInfo navigationPropertyInfo)
        {
            Check.NotNull(navigationPropertyInfo, nameof(navigationPropertyInfo));

            NavigationPropertyConfiguration.InverseNavigationProperty = navigationPropertyInfo;

            return WithMany();
        }
        internal NavigationPropertyConfigurationBase(NavigationPropertyConfiguration navigationPropertyConfiguration, RelationshipMultiplicity relationshipMultiplicity)
        {
            DebugCheck.NotNull(navigationPropertyConfiguration);

            navigationPropertyConfiguration.Reset();
            NavigationPropertyConfiguration = navigationPropertyConfiguration;
            NavigationPropertyConfiguration.RelationshipMultiplicity = relationshipMultiplicity;
        }
    }

    /// <summary>
    /// Configures an required relationship from an entity type.
    /// </summary>
    public class RequiredNavigationPropertyConfiguration : NavigationPropertyConfigurationBase
    {
        internal RequiredNavigationPropertyConfiguration(NavigationPropertyConfiguration navigationPropertyConfiguration)
            : base(navigationPropertyConfiguration, RelationshipMultiplicity.One) { }
    }

    /// <summary>
    /// Configures an optional relationship from an entity type.
    /// </summary>
    public class OptionalNavigationPropertyConfiguration : NavigationPropertyConfigurationBase
    {
        internal OptionalNavigationPropertyConfiguration(NavigationPropertyConfiguration navigationPropertyConfiguration)
            : base(navigationPropertyConfiguration, RelationshipMultiplicity.ZeroOrOne)
        {
        }
    }

    /// <summary>
    /// Configures a relationship that can support foreign key properties that are exposed in the object model.
    /// This configuration functionality is available via the Code First Fluent API, see <see cref="DbModelBuilder" />.
    /// </summary>
    public class DependentNavigationPropertyConfiguration : ForeignKeyNavigationPropertyConfiguration
    {
        internal DependentNavigationPropertyConfiguration(NavigationPropertyConfiguration navigationPropertyConfiguration)
            : base(navigationPropertyConfiguration) { }

        /// <summary>
        /// Configures the relationship to use foreign key property(s) that are exposed in the object model.
        /// If the foreign key property(s) are not exposed in the object model then use the Map method.
        /// </summary>
        /// <param name="foreignKeyPropertyInfo"> Property to be used as the foreign key. </param>
        /// <returns> A configuration object that can be used to further configure the relationship. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public CascadableNavigationPropertyConfiguration HasForeignKey(PropertyInfo foreignKeyPropertyInfo)
        {
            Check.NotNull(foreignKeyPropertyInfo, "foreignKeyPropertyInfo");

            var constraint = new ForeignKeyConstraintConfiguration();
            constraint.AddColumn(foreignKeyPropertyInfo);
            NavigationPropertyConfiguration.Constraint = constraint;

            return this;
        }
    }
}