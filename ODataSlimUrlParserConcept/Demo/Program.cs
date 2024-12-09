// See https://aka.ms/new-console-template for more information
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.UriParser;
using System.Text;
using System.Text.Json;
using System.Xml;

Console.WriteLine("Hello, World!");

var schema = """
    <?xml version='1.0' encoding='utf-8'?>
    <edmx:Edmx Version='4.0' xmlns:edmx='http://docs.oasis-open.org/odata/ns/edmx'  xmlns:odata='http://schemas.microsoft.com/oDataCapabilities'>
        <edmx:DataServices>
            <Schema Namespace='test.ns' xmlns='http://docs.oasis-open.org/odata/ns/edm'>
                <EntityType Name='customer'>
                    <Key>
                        <PropertyRef Name='id' />
                    </Key>
                    <Property Name='id' Type='Edm.Int32' Nullable='false' />
                    <Property Name='name' Type='Edm.String' />
                </EntityType>
                <EntityType Name='product'>
                    <Key>
                        <PropertyRef Name='id' />
                    </Key>
                    <Property Name='id' Type='Edm.Int32' Nullable='false' />
                    <Property Name='name' Type='Edm.String' />
                    <Property Name='category' Type='Edm.String' />
                </EntityType>
                <EntityContainer Name='container'>
                    <EntitySet Name="customers" EntityType="test.ns.customer" />
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

var uri = new Uri("http://service/products(1)?filter=category in ('stationery', 'electronics')");

var parser = new ODataUriParser(model, new Uri(relativeUri, UriKind.Relative));

var odataUri = parser.ParseUri();
Console.ReadLine();


var jsonText = """
    { "name": "Habbes", "age": 19, "hobbies": ["programming", "music"]
    """;

ReadOnlySpan<byte> jsonData = Encoding.UTF8.GetBytes(jsonText);

var jsonReader = new Utf8JsonReader(jsonData);
jsonReader.Read();

Console.ReadLine();


// filter
// expression => identifier, listExpression
// term => identifier | constant
// constant => stringConstant, intConstant
// expression => expression op expression
// listExpression => (identifier|literal)

// filterLexer => Read(),
// SlimFilterParser
// expression.Kind = "binaryop"
//  GetLeftOperand(), GetRightOperand()
// expression.Kind == list
//   EnumerateList()
// state: inList, depth, etc.