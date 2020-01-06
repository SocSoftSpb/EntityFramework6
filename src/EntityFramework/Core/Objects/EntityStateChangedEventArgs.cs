// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Objects
{
    using System.Data.Entity.Utilities;

    /// <summary>
    /// Provides data for the <see cref="E:System.Data.Entity.Core.Objects.ObjectStateManager.EntityStateChanged" /> event.
    /// </summary>
    public class EntityStateChangedEventArgs : EventArgs
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityStateChangedEventArgs" /> class.
        /// </summary>
        /// <param name="entity">Entity that state is changed</param>
        /// <param name="oldState">Previous state of the <paramref name="entity"/></param>
        /// <param name="newState">New state of the <paramref name="entity"/></param>
        public EntityStateChangedEventArgs(object entity, EntityState oldState, EntityState newState)
        {
            Check.NotNull(entity, "entity");

            Entity = entity;
            OldState = oldState;
            NewState = newState;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Entity that state is changed
        /// </summary>
        public object Entity
        {
            get;
        }

        /// <summary>
        /// Previous state of the <see cref="Entity"/>
        /// </summary>
        public EntityState OldState
        {
            get;
        }

        /// <summary>
        /// New state of the <see cref="Entity"/>
        /// </summary>
        public EntityState NewState
        {
            get;
        }

        #endregion
    }
}
