// 
// RefactoringContext.cs
//  
// Author:
//       Mike Krüger <mkrueger@novell.com>
// 
// Copyright (c) 2011 Mike Krüger <mkrueger@novell.com>
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
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.CSharp.Refactoring
{
	public abstract class RefactoringContext : AbstractActionFactory
	{
		public CompilationUnit Unit {
			get;
			protected set;
		}

		public virtual ITypeResolveContext TypeResolveContext {
			get {
				return null;
			}
		}
		
		public AstLocation Location {
			get;
			protected set;
		}
		
		public abstract bool HasCSharp3Support {
			get;
		}
		
		public abstract CSharpFormattingOptions FormattingOptions {
			get;
		}

		public abstract AstType CreateShortType (AstType fullType);
		
		public abstract AstType CreateShortType (string fullTypeName);
		
		
		public AstType CreateShortType (IType type)
		{
			return CreateShortType (new TypeSystemAstBuilder ().ConvertType (type));
		}
		
		
		public abstract ITypeDefinition GetDefinition (AstType resolvedType);

		public abstract void ReplaceReferences (IMember member, MemberDeclaration replaceWidth);
		
		public AstNode GetNode ()
		{
			return Unit.GetNodeAt (Location);
		}
		
		public T GetNode<T> () where T : AstNode
		{
			return Unit.GetNodeAt<T> (Location);
		}
		
		public abstract Script StartScript ();
		
		#region Text stuff
		public abstract string EolMarker { get; }
		public abstract bool IsSomethingSelected { get; }
		public abstract string SelectedText { get; }
		public abstract int SelectionStart { get; }
		public abstract int SelectionEnd { get; }
		public abstract int SelectionLength { get; }
		public abstract int GetOffset (AstLocation location);
		public int GetOffset (int line, int col)
		{
			return GetOffset (new AstLocation (line, col));
		}
		public abstract AstLocation GetLocation (int offset);
		public abstract string GetText (int offset, int length);
		#endregion
		
		#region Resolving
		public abstract AstType ResolveType (AstNode node);
		public abstract IEnumerable<IMember> ResolveMember (Expression expression);
		#endregion
		
		public string GetNameProposal (string name, bool camelCase = true)
		{
			string baseName = (camelCase ? char.ToLower (name [0]) : char.ToUpper (name [0])) + name.Substring (1);
			
			var type = GetNode<TypeDeclaration> ();
			if (type == null)
				return baseName;
			
			int number = -1;
			string proposedName;
			do { 
				proposedName = AppendNumberToName (baseName, number++);
			} while (type.Members.Select (m => m.GetChildByRole (AstNode.Roles.Identifier)).Any (n => n.Name == proposedName));
			return proposedName;
		}
		
		static string AppendNumberToName (string baseName, int number)
		{
			return baseName + (number > 0 ? (number + 1).ToString () : "");
		}
	}
}

