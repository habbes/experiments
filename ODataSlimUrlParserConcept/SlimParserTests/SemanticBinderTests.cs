using Lib;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SlimParserTests;

public class SemanticBinderTests
{
    private static readonly IEdmModel model = GetModel();

    [Fact]
    public void BindsSemanticContextToSyntaxTreeAndReturnsSemanticTree()
    {
        // Arrange
        string source = "category eq 'electronics' or price gt 100";
        SlimQueryNode node = ExpressionParser.Parse(source.AsMemory());

        // Act
        SemanticNode tree = SemanticBinder.Bind(node, model, model.FindDeclaredType("test.ns.product"));

        // Assert
        // verify that the semantic tree has the correct nodes and edm types
        Assert.Equal(SemanticNodeKind.Or, tree.Kind);
        Assert.Equal(SemanticNodeKind.Eq, (tree as OrNode).Left.Kind);
        Assert.Equal(SemanticNodeKind.Gt, (tree as OrNode).Right.Kind);
        Assert.Equal(SemanticNodeKind.SingleValuePropertyAccess, ((tree as OrNode).Left as EqNode).Left.Kind);
        Assert.Equal(SemanticNodeKind.StringConstant, ((tree as OrNode).Left as EqNode).Right.Kind);
        Assert.Equal(SemanticNodeKind.SingleValuePropertyAccess, ((tree as OrNode).Right as GtNode).Left.Kind);
        Assert.Equal(SemanticNodeKind.IntConstant, ((tree as OrNode).Right as GtNode).Right.Kind);
        Assert.Equal("category", (((tree as OrNode).Left as EqNode).Left as SingleValuePropertyAccessNode).Property.Name);
        Assert.Equal("electronics", (((tree as OrNode).Left as EqNode).Right as StringLiteralNode).Value);
        Assert.Equal("price", (((tree as OrNode).Right as GtNode).Left as SingleValuePropertyAccessNode).Property.Name);
        Assert.Equal(100, (((tree as OrNode).Right as GtNode).Right as IntLiteralNode).Value);
    }

    [Fact]
    public void BindSemanticContextToInExpressionWithArrayOperand()
    {
        // Arrange
        string source = "category in ('electronics', 'technology')";
        SlimQueryNode node = ExpressionParser.Parse(source.AsMemory());

        // Act
        SemanticNode tree = SemanticBinder.Bind(node, model, model.FindDeclaredType("test.ns.product"));

        // Assert
        // Verify that the semantic tree has the correct nodes and edm types
        Assert.Equal(SemanticNodeKind.In, tree.Kind);
        Assert.Equal(SemanticNodeKind.SingleValuePropertyAccess, (tree as InNode).Left.Kind);
        Assert.Equal("category", ((tree as InNode).Left as SingleValuePropertyAccessNode).Property.Name);

        var arrayNode = (tree as InNode).Right;
        Assert.Equal(SemanticNodeKind.Array, arrayNode.Kind);
        Assert.Equal(2, (arrayNode as ArrayNode).Values.Count());
        Assert.Equal(SemanticNodeKind.StringConstant, (arrayNode as ArrayNode).Values.ElementAt(0).Kind);
        Assert.Equal(SemanticNodeKind.StringConstant, (arrayNode as ArrayNode).Values.ElementAt(1).Kind);
        Assert.Equal("electronics", ((arrayNode as ArrayNode).Values.ElementAt(0) as StringLiteralNode).Value);
        Assert.Equal("technology", ((arrayNode as ArrayNode).Values.ElementAt(1) as StringLiteralNode).Value);
    }

    private static IEdmModel GetModel()
    {
        var schema = """
        <?xml version='1.0' encoding='utf-8'?>
        <edmx:Edmx Version='4.0' xmlns:edmx='http://docs.oasis-open.org/odata/ns/edmx'  xmlns:odata='http://schemas.microsoft.com/oDataCapabilities'>
            <edmx:DataServices>
                <Schema Namespace='test.ns' xmlns='http://docs.oasis-open.org/odata/ns/edm'>
                    <EntityType Name='product'>
                        <Key>
                            <PropertyRef Name='id' />
                        </Key>
                        <Property Name='id' Type='Edm.Int32' Nullable='false' />
                        <Property Name='category' Type='Edm.String' />
                        <Property Name='price' Type='Edm.Int32' />
                    </EntityType>
                    <EntityContainer Name='container'>
                        <EntitySet Name="products" EntityType="test.ns.product" />
                    </EntityContainer>
                </Schema>
            </edmx:DataServices>
        </edmx:Edmx>
        """;

        var reader = new StringReader(schema);
        var xmlReader = XmlReader.Create(reader);
        var model = CsdlReader.Parse(xmlReader);

        return model;
    }
}
