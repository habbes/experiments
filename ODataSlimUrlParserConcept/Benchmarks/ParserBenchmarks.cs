using BenchmarkDotNet.Attributes;
using Lib;
using Lib.SampleVisitors;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Benchmarks;

[MemoryDiagnoser]
public class ParserBenchmarks
{
    private const string Schema = """
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

    private static readonly IEdmModel model = GetModel();

    private static readonly IEdmType edmType = model.FindDeclaredType("test.ns.product");

    private static readonly IEdmNavigationSource navSource = model.EntityContainer.FindEntitySet("products");

    private static readonly string filterExpression = "category eq 'electronics' or price gt 100";

    private static readonly string uriString = $"products(1)?$filter={filterExpression}";

    private static readonly Uri uri = new(uriString, UriKind.Relative);

    private static Dictionary<string, string> queryOptions = new() { { "$filter", filterExpression } };


    [Benchmark]
    public FilterClause ParseExpression_ODataQueryOptionParser()
    {
        // We have to construct the query options parser in the benchmark since it caches
        // the filter clause after parsing.
        ODataQueryOptionParser queryParser = new(
            model,
            edmType,
            navSource,
            queryOptions);

        var filterClause = queryParser.ParseFilter();
        return filterClause;
    }

    [Benchmark]
    public QueryToken ParseExpression_UriQueryExpressionParser()
    {
        UriQueryExpressionParser parser = new(100);
        QueryToken expression = parser.ParseFilter(filterExpression);
        return expression;
    }

    [Benchmark]
    public SemanticNode ParseExpression_SlimSemanticBinder()
    {
        SlimQueryNode slimQuery = ExpressionParser.Parse(filterExpression.AsMemory());
        return SemanticBinder.Bind(slimQuery, model, edmType);
    }

    [Benchmark]
    public SlimQueryNode ParseExpression_SlimQueryParser()
    {
        SlimQueryNode slimQuery = ExpressionParser.Parse(filterExpression.AsMemory());
        return slimQuery;
    }

    [Benchmark]
    public ExpressionLexer ParseExpression_SlimExpressionLexer()
    {
        var lexer = new ExpressionLexer(filterExpression);
        while (lexer.Read()) { };
        return lexer;
    }

    [Benchmark]
    public string QueryRoundTrip_UriQueryExpressionParser()
    {
        var rewriter = new ODataQueryRewriter();
        UriQueryExpressionParser parser = new(100);
        QueryToken expression = parser.ParseFilter(filterExpression);
        expression.Accept(rewriter);
        return rewriter.GetQuery();
    }

    [Benchmark]
    public string QueryRoundTrip_SlimQueryParser()
    {
        var rewriter = new SlimQueryRewriter();
        SlimQueryNode slimQuery = ExpressionParser.Parse(filterExpression.AsMemory());
        slimQuery.Accept(rewriter);
        return rewriter.GetQuery();
    }



    private static IEdmModel GetModel()
    {
        var stringReader = new StringReader(Schema);
        var xmlReader = XmlReader.Create(stringReader);
        var model = CsdlReader.Parse(xmlReader);
        return model;
    }
}
