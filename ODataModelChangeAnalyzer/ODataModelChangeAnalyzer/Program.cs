using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using ODataModelChangeAnalyzer;
using System.Xml;

var originalModel = ReadModel("TripPinBaseModel.csdl");
var newModel = ReadModel("TripPinBaseModel.csdl");

var changeAnalyzer = new ChangesAnalyzer(originalModel, newModel);
var changes = changeAnalyzer.AnalyzeChanges();

Console.WriteLine("Detected changes:\n");

int count = 0;
foreach (var change in changes)
{
    Console.WriteLine(change);
    count++;
}

Console.WriteLine($"\nFound {count} changes.");


IEdmModel ReadModel(string path)
{
    using var file = File.OpenRead(path);
    var xmlReader = XmlReader.Create(file);
    var model = CsdlReader.Parse(xmlReader);
    return model;
}
