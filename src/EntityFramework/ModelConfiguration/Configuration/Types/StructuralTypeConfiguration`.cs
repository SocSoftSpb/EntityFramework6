// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.ModelConfiguration.Configuration
{
    using System.ComponentModel;
    using System.Data.Entity.Hierarchy;
    using System.Data.Entity.ModelConfiguration.Configuration.Types;
    using System.Data.Entity.Spatial;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Allows configuration to be performed for a type in a model.
    /// </summary>
    /// <typeparam name="TStructuralType"> The type to be configured. </typeparam>
    public abstract class StructuralTypeConfiguration<TStructuralType> : IStructuralTypeConfiguration
        where TStructuralType : class
    {
        /// <summary>
        /// Configures a <see cref="T:System.struct" /> property that is defined on this type.
        /// </summary>
        /// <typeparam name="T"> The type of the property being configured. </typeparam>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public PrimitivePropertyConfiguration Property<T>(
            Expression<Func<TStructuralType, T>> propertyExpression)
            where T : struct
        {
            return new PrimitivePropertyConfiguration(Property<Properties.Primitive.PrimitivePropertyConfiguration>(propertyExpression));
        }

        /// <summary>
        /// Configures a <see cref="T:System.struct" /> property that is defined on this type.
        /// </summary>
        /// <typeparam name="T"> The type of the property being configured. </typeparam>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public PrimitivePropertyConfiguration Property<T>(
            Expression<Func<TStructuralType, T?>> propertyExpression)
            where T : struct
        {
            return new PrimitivePropertyConfiguration(
                Property<Properties.Primitive.PrimitivePropertyConfiguration>(propertyExpression));
        }

        /// <summary>
        /// Configures a <see cref="T:HierarchyId" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public PrimitivePropertyConfiguration Property(
            Expression<Func<TStructuralType, HierarchyId>> propertyExpression)
        {
            return new PrimitivePropertyConfiguration(
                Property<Properties.Primitive.PrimitivePropertyConfiguration>(propertyExpression));
        }

        /// <summary>
        /// Configures a <see cref="T:System.Data.Entity.Spatial.DbGeometry" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public PrimitivePropertyConfiguration Property(
            Expression<Func<TStructuralType, DbGeometry>> propertyExpression)
        {
            return new PrimitivePropertyConfiguration(
                Property<Properties.Primitive.PrimitivePropertyConfiguration>(propertyExpression));
        }

        /// <summary>
        /// Configures a <see cref="T:System.Data.Entity.Spatial.DbGeography" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public PrimitivePropertyConfiguration Property(
            Expression<Func<TStructuralType, DbGeography>> propertyExpression)
        {
            return new PrimitivePropertyConfiguration(
                Property<Properties.Primitive.PrimitivePropertyConfiguration>(propertyExpression));
        }

        /// <summary>
        /// Configures a <see cref="T:System.String" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public StringPropertyConfiguration Property(Expression<Func<TStructuralType, string>> propertyExpression)
        {
            return new StringPropertyConfiguration(
                Property<Properties.Primitive.StringPropertyConfiguration>(propertyExpression));
        }

        /// <summary>
        /// Configures a <see cref="T:System.Byte[]" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public BinaryPropertyConfiguration Property(Expression<Func<TStructuralType, byte[]>> propertyExpression)
        {
            return new BinaryPropertyConfiguration(
                Property<Properties.Primitive.BinaryPropertyConfiguration>(propertyExpression));
        }

        /// <summary>
        /// Configures a <see cref="T:System.Decimal" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public DecimalPropertyConfiguration Property(Expression<Func<TStructuralType, decimal>> propertyExpression)
        {
            return new DecimalPropertyConfiguration(
                Property<Properties.Primitive.DecimalPropertyConfiguration>(propertyExpression));
        }

        /// <summary>
        /// Configures a <see cref="T:System.Decimal" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public DecimalPropertyConfiguration Property(Expression<Func<TStructuralType, decimal?>> propertyExpression)
        {
            return new DecimalPropertyConfiguration(
                Property<Properties.Primitive.DecimalPropertyConfiguration>(propertyExpression));
        }

        /// <summary>
        /// Configures a <see cref="T:System.DateTime" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public DateTimePropertyConfiguration Property(Expression<Func<TStructuralType, DateTime>> propertyExpression)
        {
            return new DateTimePropertyConfiguration(
                Property<Properties.Primitive.DateTimePropertyConfiguration>(propertyExpression));
        }

        /// <summary>
        /// Configures a <see cref="T:System.DateTime" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public DateTimePropertyConfiguration Property(Expression<Func<TStructuralType, DateTime?>> propertyExpression)
        {
            return new DateTimePropertyConfiguration(
                Property<Properties.Primitive.DateTimePropertyConfiguration>(propertyExpression));
        }

        /// <summary>
        /// Configures a <see cref="T:System.DateTimeOffset" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public DateTimePropertyConfiguration Property(
            Expression<Func<TStructuralType, DateTimeOffset>> propertyExpression)
        {
            return new DateTimePropertyConfiguration(
                Property<Properties.Primitive.DateTimePropertyConfiguration>(propertyExpression));
        }

        /// <summary>
        /// Configures a <see cref="T:System.DateTimeOffset" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public DateTimePropertyConfiguration Property(
            Expression<Func<TStructuralType, DateTimeOffset?>> propertyExpression)
        {
            return new DateTimePropertyConfiguration(
                Property<Properties.Primitive.DateTimePropertyConfiguration>(propertyExpression));
        }

        /// <summary>
        /// Configures a <see cref="T:System.TimeSpan" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public DateTimePropertyConfiguration Property(Expression<Func<TStructuralType, TimeSpan>> propertyExpression)
        {
            return new DateTimePropertyConfiguration(
                Property<Properties.Primitive.DateTimePropertyConfiguration>(propertyExpression));
        }

        /// <summary>
        /// Configures a <see cref="T:System.TimeSpan" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public DateTimePropertyConfiguration Property(Expression<Func<TStructuralType, TimeSpan?>> propertyExpression)
        {
            return new DateTimePropertyConfiguration(
                Property<Properties.Primitive.DateTimePropertyConfiguration>(propertyExpression));
        }

        #region Weak-typed mapping

        /// <summary>
        /// Configures a <see cref="T:System.struct" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyInfo"> A property to be configured. </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public PrimitivePropertyConfiguration Property(PropertyInfo propertyInfo)
        {
            return new PrimitivePropertyConfiguration(Property<Properties.Primitive.PrimitivePropertyConfiguration>(propertyInfo));
        }

        /// <summary>
        /// Configures a <see cref="T:System.string" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyInfo"> A property to be configured. </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public StringPropertyConfiguration PropertyString(PropertyInfo propertyInfo)
        {
            return new StringPropertyConfiguration(Property<Properties.Primitive.StringPropertyConfiguration>(propertyInfo));
        }

        /// <summary>
        /// Configures a <see cref="T:System.byte[]" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyInfo"> A property to be configured. </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public BinaryPropertyConfiguration PropertyBinary(PropertyInfo propertyInfo)
        {
            return new BinaryPropertyConfiguration(Property<Properties.Primitive.BinaryPropertyConfiguration>(propertyInfo));
        }

        /// <summary>
        /// Configures a <see cref="T:System.decimal" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyInfo"> A property to be configured. </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public DecimalPropertyConfiguration PropertyDecimal(PropertyInfo propertyInfo)
        {
            return new DecimalPropertyConfiguration(Property<Properties.Primitive.DecimalPropertyConfiguration>(propertyInfo));
        }

        /// <summary>
        /// Configures a <see cref="T:System.DateTime" /> property that is defined on this type.
        /// </summary>
        /// <param name="propertyInfo"> A property to be configured. </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public DateTimePropertyConfiguration PropertyDateTime(PropertyInfo propertyInfo)
        {
            return new DateTimePropertyConfiguration(Property<Properties.Primitive.DateTimePropertyConfiguration>(propertyInfo));
        }

        internal abstract TPrimitivePropertyConfiguration Property<TPrimitivePropertyConfiguration>(PropertyInfo propertyInfo)
            where TPrimitivePropertyConfiguration : Properties.Primitive.PrimitivePropertyConfiguration, new();

        #endregion

        internal abstract StructuralTypeConfiguration Configuration { get; }

        internal abstract TPrimitivePropertyConfiguration Property<TPrimitivePropertyConfiguration>(
            LambdaExpression lambdaExpression)
            where TPrimitivePropertyConfiguration : Properties.Primitive.PrimitivePropertyConfiguration, new();

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Gets the <see cref="Type" /> of the current instance.
        /// </summary>
        /// <returns>The exact runtime type of the current instance.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType()
        {
            return base.GetType();
        }

        /// <inheritdoc />
        public StructuralTypeConfiguration InternalConfiguration => Configuration;
    }
}
