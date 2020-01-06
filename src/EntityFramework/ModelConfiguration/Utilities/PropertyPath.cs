// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.ModelConfiguration.Utilities
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity.Utilities;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Property path (one or more PropertyInfo)
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public sealed class PropertyPath : IEnumerable<PropertyInfo>
    {
        // Note: This class is currently immutable. If you make it mutable then you
        // must ensure that instances are cloned when cloning the DbModelBuilder.
        private static readonly PropertyPath _empty = new PropertyPath();

        private readonly List<PropertyInfo> _components = new List<PropertyInfo>();

        /// <summary>
        /// ctor
        /// </summary>
        public PropertyPath(IEnumerable<PropertyInfo> components)
        {
            DebugCheck.NotNull(components);
            Debug.Assert(components.Any());

            _components.AddRange(components);
        }

        /// <summary>
        /// ctor
        /// </summary>
        public PropertyPath(PropertyInfo component)
        {
            DebugCheck.NotNull(component);

            _components.Add(component);
        }

        private PropertyPath() {}

        /// <summary>
        /// Count of properties on path
        /// </summary>
        public int Count
        {
            get { return _components.Count; }
        }

        /// <summary>
        /// Static Empty PropertyPath
        /// </summary>
        public static PropertyPath Empty
        {
            get { return _empty; }
        }

        /// <summary>
        /// Get path component by index
        /// </summary>
        public PropertyInfo this[int index]
        {
            get { return _components[index]; }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var propertyPathName = new StringBuilder();

            _components
                .Each(
                    pi =>
                        {
                            propertyPathName.Append(pi.Name);
                            propertyPathName.Append('.');
                        });

            return propertyPathName.ToString(0, propertyPathName.Length - 1);
        }

        #region Equality Members

        /// <summary>
        /// Is Equals to another
        /// </summary>
        public bool Equals(PropertyPath other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return _components.SequenceEqual(other._components, (p1, p2) => p1.IsSameAs(p2));
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(PropertyPath))
            {
                return false;
            }

            return Equals((PropertyPath)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return _components.Aggregate(
                    0,
                    (t, n) => t ^ (n.DeclaringType.GetHashCode() * n.Name.GetHashCode() * 397));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator ==(PropertyPath left, PropertyPath right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator !=(PropertyPath left, PropertyPath right)
        {
            return !Equals(left, right);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator<PropertyInfo> IEnumerable<PropertyInfo>.GetEnumerator()
        {
            return _components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _components.GetEnumerator();
        }

        #endregion
    }
}
