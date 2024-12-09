using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using ODataModelChangeAnalyzer;
using System.Xml;

var originalModel = ReadModel("TripPinBaseModel.csdl");
var newModel = ReadModel("TripPinUpdatedModel.csdl");

var changeAnalyzer = new ChangesAnalyzer(originalModel, newModel);
var changes = changeAnalyzer.AnalyzeChanges();

var changesWithLocations = changes.Select(change => new ChangeWithLocation(change, change.Target.Location() as CsdlLocation));

Console.WriteLine("Detected changes:\n");

int count = 0;

foreach (var change in changes)
{
    count++;
    var location = change.Target.Location() as CsdlLocation;
    Console.WriteLine(change);
    Console.WriteLine($"At line: {location!.LineNumber} pos: {location!.LinePosition}");
    Console.WriteLine();

    var error = change switch
    {
        StructuredTypeChangedFromOpenToClose c => $"Changing the type {c.NewType.FullTypeName()} from open to close is a breaking change. To fix this, add the attribute OpenType=\"True\".",
        _ => throw new NotSupportedException()
    };
}

Console.WriteLine($"\nFound {count} changes.");




IEdmModel ReadModel(string path)
{
    using var file = File.OpenRead(path);
    var xmlReader = XmlReader.Create(file);
    var model = CsdlReader.Parse(xmlReader);
    return model;
}

record ChangeWithLocation(object Change, CsdlLocation? Location);

record ConditionalBreakingChange<T>(Func<T, bool> IsBreaking);
