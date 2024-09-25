# Sample OData schema change analyzer

Sample analyzer that lists changes between original and updated OData schema using the `IEdmModel`. This can be useful for detecting breaking changes between API versions.

```cli
Detected changes:

StructuredTypePropertyAdded { Property = Microsoft.OData.Edm.Csdl.CsdlSemantics.CsdlSemanticsProperty, NewType = Microsoft.OData.SampleService.Models.TripPin.AirportLocation, OriginalType = Microsoft.OData.SampleService.Models.TripPin.AirportLocation, Target = Microsoft.OData.SampleService.Models.TripPin.AirportLocation, ChangeKind = Modification }
At line: 22 pos: 5

SchemaElementAdded { Target = Microsoft.OData.SampleService.Models.TripPin.Picture, ChangeKind = Addition }
At line: 26 pos: 5

StructuredTypeChangedFromOpenToClose { Target = Microsoft.OData.SampleService.Models.TripPin.Person, Original = Microsoft.OData.SampleService.Models.TripPin.Person, ChangeKind = Modification }
At line: 42 pos: 5

StructuredTypePropertyChangedToNonNullable { NewProperty = Microsoft.OData.Edm.Csdl.CsdlSemantics.CsdlSemanticsProperty, OldProperty = Microsoft.OData.Edm.Csdl.CsdlSemantics.CsdlSemanticsProperty, NewType = Microsoft.OData.SampleService.Models.TripPin.Person, OriginalType = Microsoft.OData.SampleService.Models.TripPin.Person, Target = Microsoft.OData.SampleService.Models.TripPin.Person, ChangeKind = Modification }
At line: 42 pos: 5

StructuredTypePropertyAdded { Property = Microsoft.OData.Edm.Csdl.CsdlSemantics.CsdlSemanticsProperty, NewType = Microsoft.OData.SampleService.Models.TripPin.Airline, OriginalType = Microsoft.OData.SampleService.Models.TripPin.Airline, Target = Microsoft.OData.SampleService.Models.TripPin.Airline, ChangeKind = Modification }
At line: 63 pos: 5

StructuredTypePropertyAddedToKey { NewProperty = Microsoft.OData.Edm.Csdl.CsdlSemantics.CsdlSemanticsProperty, OldProperty = Microsoft.OData.Edm.Csdl.CsdlSemantics.CsdlSemanticsProperty, NewType = Microsoft.OData.SampleService.Models.TripPin.Airline, OriginalType = Microsoft.OData.SampleService.Models.TripPin.Airline, Target = Microsoft.OData.SampleService.Models.TripPin.Airline, ChangeKind = Modification }
At line: 63 pos: 5

StructuredTypePropertyTypeChanged { NewProperty = Microsoft.OData.Edm.Csdl.CsdlSemantics.CsdlSemanticsProperty, OldProperty = Microsoft.OData.Edm.Csdl.CsdlSemantics.CsdlSemanticsProperty, NewType = Microsoft.OData.SampleService.Models.TripPin.Airport, OriginalType = Microsoft.OData.SampleService.Models.TripPin.Airport, Target = Microsoft.OData.SampleService.Models.TripPin.Airport, ChangeKind = Modification }
At line: 75 pos: 5

StructuredTypePropertyRemoved { Property = Microsoft.OData.Edm.Csdl.CsdlSemantics.CsdlSemanticsProperty, OriginalType = Microsoft.OData.SampleService.Models.TripPin.Airport, NewType = Microsoft.OData.SampleService.Models.TripPin.Airport, Target = Microsoft.OData.SampleService.Models.TripPin.Airport, ChangeKind = Modification }
At line: 75 pos: 5

StructuredTypePropertyAdded { Property = Microsoft.OData.Edm.Csdl.CsdlSemantics.CsdlSemanticsProperty, NewType = Microsoft.OData.SampleService.Models.TripPin.PlanItem, OriginalType = Microsoft.OData.SampleService.Models.TripPin.PlanItem, Target = Microsoft.OData.SampleService.Models.TripPin.PlanItem, ChangeKind = Modification }
At line: 89 pos: 5

StructuredTypePropertyAdded { Property = Microsoft.OData.Edm.Csdl.CsdlSemantics.CsdlSemanticsProperty, NewType = Microsoft.OData.SampleService.Models.TripPin.PublicTransportation, OriginalType = Microsoft.OData.SampleService.Models.TripPin.PublicTransportation, Target = Microsoft.OData.SampleService.Models.TripPin.PublicTransportation, ChangeKind = Modification }
At line: 104 pos: 5

StructuredTypePropertyAdded { Property = Microsoft.OData.Edm.Csdl.CsdlSemantics.CsdlSemanticsProperty, NewType = Microsoft.OData.SampleService.Models.TripPin.Flight, OriginalType = Microsoft.OData.SampleService.Models.TripPin.Flight, Target = Microsoft.OData.SampleService.Models.TripPin.Flight, ChangeKind = Modification }
At line: 107 pos: 5

StructuredTypePropertyDeclaringTypeChanged { NewProperty = Microsoft.OData.Edm.Csdl.CsdlSemantics.CsdlSemanticsProperty, OldProperty = Microsoft.OData.Edm.Csdl.CsdlSemantics.CsdlSemanticsProperty, NewType = Microsoft.OData.SampleService.Models.TripPin.Event, OriginalType = Microsoft.OData.SampleService.Models.TripPin.Event, Target = Microsoft.OData.SampleService.Models.TripPin.Event, ChangeKind = Modification }
At line: 113 pos: 5

SchemaElementRemoved { Target = Microsoft.OData.SampleService.Models.TripPin.Photo, ChangeKind = Deletion }
At line: 25 pos: 5


Found 13 changes.
```
