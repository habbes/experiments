// See https://aka.ms/new-console-template for more information
using Lib;
using Lib.SampleVisitors;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.UriParser;
using Microsoft.OData.UriParser.Aggregation;
using System.Text;
using System.Text.Json;
using System.Xml;
using static Lib.ExpressionLexer;

Console.WriteLine("Hello, World!");

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

var serviceRoot = new Uri("http://service");
var relativeUri = "products(1)?$filter=category eq 'electronics'";

var filterExpression = "category eq 'electronics' or price gt 100";

var uri = new Uri("http://service/products(1)?filter=category in ('stationery', 'electronics')");

//var parser = new ODataUriParser(model, new Uri(relativeUri, UriKind.Relative));
//var queryParser = new ODataQueryOptionParser(
//    model,
//    model.FindDeclaredType("test.ns.product"),
//    model.EntityContainer.FindEntitySet("products"),
//    new Dictionary<string, string> { { "$filter", filterExpression } });



////var odataUri = parser.ParseUri();
//var filter = queryParser.ParseFilter();
//Console.ReadLine();

UriQueryExpressionParser queryExpressionParser = new(100);
filterExpression = "category in ['electronics']";
QueryToken filterToken = queryExpressionParser.ParseFilter(filterExpression);

//Console.ReadLine();
////Console.WriteLine(filterToken.Kind);
//Console.ReadLine();
CollectMemory();
SlimQueryNode slimQuery = ExpressionParser.Parse(filterExpression.AsMemory());
//Console.ReadLine();
//CollectMemory();

var odataTranslator = new ODataQueryRewriter();
filterToken.Accept(odataTranslator);
Console.WriteLine($"Query generated with OData translater: {odataTranslator.GetQuery()}");
Console.ReadLine();

var slimRewriter = new SlimQueryRewriter();
slimQuery.Accept(slimRewriter);
Console.WriteLine($"Query generated with OData translater: {slimRewriter.GetQuery()}");
Console.ReadLine();

var semanticTree = SemanticBinder.Bind(slimQuery, model, model.FindDeclaredType("test.ns.product"));

Console.ReadLine();



void CollectMemory()
{
    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.Collect();
}
// filter
// filterToken => identifier, listExpression
// term => identifier | constant
// constant => stringConstant, intConstant
// filterToken => filterToken op filterToken
// listExpression => (identifier|literal)

// filterLexer => Read(),
// SlimFilterParser
// filterToken.Kind = "binaryop"
//  GetLeftOperand(), GetRightOperand()
// filterToken.Kind == list
//   EnumerateList()
// state: inList, depth, etc.
