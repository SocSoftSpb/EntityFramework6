// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Query.InternalTrees
{
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Diagnostics;

    // <summary>
    // Represents a string comparison operation
    // </summary>
    internal sealed class LikeOp : ScalarOp
    {
        #region Fields

        private bool _isCommon;
        
        #endregion

        #region constructors

        internal LikeOp(TypeUsage boolType, bool isCommon)
            : base(OpType.Like, boolType)
        {
            _isCommon = isCommon;
        }

        private LikeOp()
            : base(OpType.Like)
        {
        }

        #endregion

        #region public surface

        // <summary>
        // Pattern for use in transformation rules
        // </summary>
        internal static readonly LikeOp Pattern = new LikeOp();

        // <summary>
        // 3 children - string, pattern , escape
        // </summary>
        internal override int Arity
        {
            get { return 3; }
        }

        /// <summary>
        /// Is "Common" Like
        /// </summary>
        internal bool IsCommon
        {
            get { return _isCommon; }
        }

        // <summary>
        // Visitor pattern method
        // </summary>
        // <param name="v"> The BasicOpVisitor that is visiting this Op </param>
        // <param name="n"> The Node that references this Op </param>
        [DebuggerNonUserCode]
        internal override void Accept(BasicOpVisitor v, Node n)
        {
            v.Visit(this, n);
        }

        // <summary>
        // Visitor pattern method for visitors with a return value
        // </summary>
        // <param name="v"> The visitor </param>
        // <param name="n"> The node in question </param>
        // <returns> An instance of TResultType </returns>
        [DebuggerNonUserCode]
        internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n)
        {
            return v.Visit(this, n);
        }

        #endregion
    }
}
