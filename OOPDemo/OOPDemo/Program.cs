// See https://aka.ms/new-console-template for more information
using OOPDemo;

string DataFolder = @"C:\Users\clhabins\source\repos\tinker\OOPDemo\OOPDemo\DataFolder\";

var fileHandler = new CsvFileHandler();
var jsonHandler = new JsonDataHandler();
var compositeHandler = new CompositeFileHandler();
compositeHandler.Add(fileHandler);
compositeHandler.Add(jsonHandler);

var folderDataSource = new FolderDataSource(DataFolder, compositeHandler);
var productRepistory = new ProductRepository();
var reader = new ProductReader(folderDataSource, productRepistory);
reader.ReadData(DataFolder);

var app = new CliLoop(productRepistory);
app.Start();