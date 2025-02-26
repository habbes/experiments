using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
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
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
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

    private static readonly string filterExpressionWithInAndArrays = "category in ['electronics', 'technology']";
    private static readonly string uriStringWithInAndArrays = $"products(1)?$filter={filterExpressionWithInAndArrays}";
    private static readonly Uri uriWithInAndArrays = new(uriStringWithInAndArrays, UriKind.Relative);


    [Benchmark(Baseline = true)]
    [BenchmarkCategory("ParseExpression")]
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
    [BenchmarkCategory("ParseExpression")]
    public QueryToken ParseExpression_UriQueryExpressionParser()
    {
        UriQueryExpressionParser parser = new(100);
        QueryToken expression = parser.ParseFilter(filterExpression);
        return expression;
    }

    [Benchmark]
    [BenchmarkCategory("ParseExpression")]
    public SemanticNode ParseExpression_SlimSemanticBinder()
    {
        SlimQueryNode slimQuery = ExpressionParser.Parse(filterExpression.AsMemory());
        return SemanticBinder.Bind(slimQuery, model, edmType);
    }

    [Benchmark]
    [BenchmarkCategory("ParseExpression")]
    public SlimQueryNode ParseExpression_SlimQueryParser()
    {
        SlimQueryNode slimQuery = ExpressionParser.Parse(filterExpression.AsMemory());
        return slimQuery;
    }

    [Benchmark]
    [BenchmarkCategory("ParseExpression")]
    public ExpressionLexer ParseExpression_SlimExpressionLexer()
    {
        var lexer = new ExpressionLexer(filterExpression);
        while (lexer.Read()) { }
        return lexer;
    }

    #region "ParseExpressionWithInAndArrays"

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("ParseExpressionWithInAndArrays")]
    public QueryToken ParseExpressionWithInAndArrays_UriQueryExpressionParser()
    {
        UriQueryExpressionParser parser = new(100);
        QueryToken expression = parser.ParseFilter(filterExpressionWithInAndArrays);
        return expression;
    }

    [Benchmark]
    [BenchmarkCategory("ParseExpressionWithInAndArrays")]
    public SemanticNode ParseExpressionWithInAndArrays_SlimSemanticBinder()
    {
        SlimQueryNode slimQuery = ExpressionParser.Parse(filterExpressionWithInAndArrays.AsMemory());
        return SemanticBinder.Bind(slimQuery, model, edmType);
    }

    [Benchmark]
    [BenchmarkCategory("ParseExpressionWithInAndArrays")]
    public SlimQueryNode ParseExpressionWithInAndArrays_SlimQueryParser()
    {
        SlimQueryNode slimQuery = ExpressionParser.Parse(filterExpressionWithInAndArrays.AsMemory());
        return slimQuery;
    }

    [Benchmark]
    [BenchmarkCategory("ParseExpressionWithInAndArrays")]
    public ExpressionLexer ParseExpressionWithInAndArrays_SlimExpressionLexer()
    {
        var lexer = new ExpressionLexer(filterExpressionWithInAndArrays);
        while (lexer.Read()) { }
        return lexer;
    }

    #endregion

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("QueryRoundTrip")]
    public string QueryRoundTrip_UriQueryExpressionParser()
    {
        var rewriter = new ODataQueryRewriter();
        UriQueryExpressionParser parser = new(100);
        QueryToken expression = parser.ParseFilter(filterExpression);
        expression.Accept(rewriter);
        return rewriter.GetQuery();
    }

    [Benchmark]
    [BenchmarkCategory("QueryRoundTrip")]
    public string QueryRoundTrip_SlimQueryParser()
    {
        var rewriter = new SlimQueryRewriter();
        SlimQueryNode slimQuery = ExpressionParser.Parse(filterExpression.AsMemory());
        slimQuery.Accept(rewriter);
        return rewriter.GetQuery();
    }

    #region round trip with in and arrays

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("QueryRoundTripWithInAndArrays")]
    public string QueryRoundTripWithInAndArrays_UriQueryExpressionParser()
    {
        var rewriter = new ODataQueryRewriter();
        UriQueryExpressionParser parser = new(100);
        QueryToken expression = parser.ParseFilter(filterExpressionWithInAndArrays);
        expression.Accept(rewriter);
        return rewriter.GetQuery();
    }

    [Benchmark]
    [BenchmarkCategory("QueryRoundTripWithInAndArrays")]
    public string QueryRoundTripWithInAndArrays_SlimQueryParser()
    {
        var rewriter = new SlimQueryRewriter();
        SlimQueryNode slimQuery = ExpressionParser.Parse(filterExpressionWithInAndArrays.AsMemory());
        slimQuery.Accept(rewriter);
        return rewriter.GetQuery();
    }

    #endregion

    private static IEdmModel GetModel()
    {
        var stringReader = new StringReader(Schema);
        var xmlReader = XmlReader.Create(stringReader);
        var model = CsdlReader.Parse(xmlReader);
        return model;
    }

}
