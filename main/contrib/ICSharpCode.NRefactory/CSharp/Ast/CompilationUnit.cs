﻿// 
// CompilationUnit.cs
//  
// Author:
//       Mike Krüger <mkrueger@novell.com>
// 
// Copyright (c) 2010 Novell, Inc (http://www.novell.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.CSharp
{
	public class CompilationUnit : AstNode 
	{
		public static readonly Role<AstNode> MemberRole = new Role<AstNode>("Member", AstNode.Null);
		
		public override NodeType NodeType {
			get {
				return NodeType.Unknown;
			}
		}
		
		public CompilationUnit ()
		{
		}
		
		public IEnumerable<TypeDeclaration> GetTypes (bool includeInnerTypes = false)
		{
			Stack<AstNode> nodeStack = new Stack<AstNode> ();
			nodeStack.Push (this);
			while (nodeStack.Count > 0) {
				var curNode = nodeStack.Pop ();
				if (curNode is TypeDeclaration)
					yield return (TypeDeclaration)curNode;
				foreach (var child in curNode.Children) {
					if (!(child is Statement || child is Expression) &&
						 (child.Role != TypeDeclaration.MemberRole || (child is TypeDeclaration && includeInnerTypes)))
						nodeStack.Push (child);
				}
			}
		}
		
		protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
		{
			CompilationUnit o = other as CompilationUnit;
			return o != null && GetChildrenByRole(MemberRole).DoMatch(o.GetChildrenByRole(MemberRole), match);
		}
		
		public override S AcceptVisitor<T, S> (IAstVisitor<T, S> visitor, T data)
		{
			return visitor.VisitCompilationUnit (this, data);
		}
	}
}

