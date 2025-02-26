using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib;

public partial class SemanticBinder
{
    internal class TreeHandler : ISyntacticExpressionHandler<SemanticNode>
    {
        IEdmModel _model;
        IEdmType _rootType;
        Stack<State> _stateStack = new Stack<State>();

        public TreeHandler(IEdmModel model, IEdmType type)
        {
            _model = model;
            _rootType = type;
            _stateStack.Push(new State(type));
        }

        public SemanticNode HandleAnd(SlimQueryNode andNode)
        {
            var left = andNode.GetLeft().Accept(this);
            var right = andNode.GetRight().Accept(this);
            ValidateSameType(left, right);
            return new AndNode(left, right);
        }

        public SemanticNode HandleEq(SlimQueryNode eqNode)
        {
            var left = eqNode.GetLeft().Accept(this);
            var right = eqNode.GetRight().Accept(this);
            ValidateSameType(left, right);
            return new EqNode(left, right);
        }

        public SemanticNode HandleGt(SlimQueryNode gtNode)
        {
            var left = gtNode.GetLeft().Accept(this);
            var right = gtNode.GetRight().Accept(this);
            ValidateSameType(left, right);
            return new GtNode(left, right);
        }

        public SemanticNode HandleIdentifier(SlimQueryNode identifier)
        {
            var state = _stateStack.Peek();
            if (state.ParentType is not IEdmStructuredType structuredType)
            {
                throw new InvalidOperationException("Cannot use identifier in this context since there's not parent type for property access");
            }

            // TODO compare FindProperty with scanning through the properties and comparing withut having to _get string
            var prop = structuredType.FindProperty(identifier.GetIdentifier());
            if (prop == null)
            {
                throw new InvalidOperationException($"Property {identifier.GetString()} not found in type {structuredType.FullTypeName()}");
            }

            return new SingleValuePropertyAccessNode(prop);
        }

        public SemanticNode HandleIntConstant(SlimQueryNode intConst)
        {
            return new IntLiteralNode(intConst.GetInt());
        }

        public SemanticNode HandleBoolConstant(SlimQueryNode boolConst)
        {
            return new BoolLiteralNode(boolConst.GetBoolean());
        }

        public SemanticNode HandleOr(SlimQueryNode orNode)
        {
            var left = orNode.GetLeft().Accept(this);
            var right = orNode.GetRight().Accept(this);
            ValidateSameType(left, right);
            return new OrNode(left, right);
        }

        public SemanticNode HandleStringConstant(SlimQueryNode stringConst)
        {
            return new StringLiteralNode(stringConst.GetString());
        }

        public SemanticNode HandleArray(SlimQueryNode arrayNode)
        {
            var values = arrayNode.EnumerateArray().Select(v => v.Accept(this));
            return new ArrayNode(values);
        }

        public SemanticNode HandleIn(SlimQueryNode inNode)
        {
            var left = inNode.GetLeft().Accept(this);
            var right = inNode.GetRight().Accept(this);

            ValidateSameType(left.EdmType, right.EdmType.AsCollection().ElementType());
            return new InNode(left, right);
        }

        private static void ValidateSameType(SemanticNode left, SemanticNode right)
        {
            if (left.EdmType.Definition != right.EdmType.Definition)
            {
                throw new InvalidOperationException($"Cannot compare values of different types: {left.EdmType.Definition.FullTypeName()} and {right.EdmType.Definition.FullTypeName()}");
            }
        }

        private static void ValidateSameType(IEdmTypeReference left, IEdmTypeReference right)
        {
            if (left.Definition != right.Definition)
            {
                throw new InvalidOperationException($"Cannot compare values of different types: {left.Definition.FullTypeName()} and {right.Definition.FullTypeName()}");
            }
        }



        record struct State(IEdmType ParentType);
    }
}
