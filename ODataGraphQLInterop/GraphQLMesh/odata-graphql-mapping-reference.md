This is a reference that shows how different types and queries are translated from OData to GraphQL by the OData handler in GraphQL Mesh. For a primer on GraphQL schema and type system, [visit the official docs](https://graphql.org/learn/schema/).

Most of the examples are based on the TripPin sample OData service available at: https://services.odata.org/TripPinRESTierService

[[_TOC_]]

## Scalars

| OData     | GraphQL
------------|----------
`Edm.Binary` | `String`
`Edm.Stream` | `String`
`Edm.String` | `String`
`Edm.Int16` | `Int`
`Edm.Byte` | `Byte`
`Edm.Int32` | `Int`
`Edm.Int64` | `BigInt`
`Edm.Double` | `Float`
`Edm.Boolean` | `Boolean`
`Edm.Guid` | `GUID`
`Edm.DateTimeOffset` | `DateTime`
`Edm.Date` | `Date`
`Edm.TimeOfDay` | `String`
`Edm.Single` | `Float`
`Edm.Duration` | `ISO8601Duration`
`Edm.Decimal` | `Float`
`Edm.SByte` | `Byte`
`Edm.GeographyPoint` | `String`

## Enums

An OData enum is translated to a GraphQL enum whose values match the names of the members of the OData enum.

Examples:

**OData Enum**:
```xml
<EnumType Name="PersonGender">
  <Member Name="Male" Value="0"/>
  <Member Name="Female" Value="1"/>
  <Member Name="Unknown" Value="2"/>
</EnumType>
```

**GraphQL Enum**:

```graphql
enum PersonGender {
  Male
  Female
  Unknown
}
```

## Query Options

An input type is generated to allow query options to be passed to the OData service. It's used as an optional input for queries and fields that access navigation properties. It's defined as follows:

```graphql
input QueryOptions {
  orderby: String
  top: Int
  skip: Int
  filter: String
  inlinecount: InlineCount
  count: Boolean
}

enum InlineCount {
  allpages
  none
}
```

The properties of the query options input will be added to the relevant query string arguments and appended to the request URL sent to the OData service.

**Note** that the filter property takes a string that represents an OData filter expression, using OData syntax. It's passed as is to the OData query option. This is not ideal for GraphQL users who do not know (or do not want to know) OData syntax.

## Entity Types and Complex Types

Entity types and complex types are translated into GraphQL object types with the same name and corresponding fields.

### Navigation Properties

Single Navigation Properties are added as simples fields to the object type.

Collection Navigation properties are translated as fields which take a `queryOptions` argument:
`PlanItems(queryOptions: QueryOptions): [PlanItem]`

An additional field is added for each collection navigation property to fetch a related entity by id. It takes the name of the navigation property and adds the `ById` suffix: `PlanItemsById(id: ID): PlanItem`.

### Complex Type Example

**OData:**
```xml
<ComplexType Name="City">
  <Property Name="Name" Type="Edm.String" />
  <Property Name="CountryRegion" Type="Edm.String" />
  <Property Name="Region" Type="Edm.String" />
</ComplexType>
```

**GraphQL:**
```
type City {
  Name: String
  CountryRegion: String
  Region: String
}
```

### Entity Type Example

**OData:**
```xml
<EntityType Name="Trip">
  <Key>
    <PropertyRef Name="TripId" />
  </Key>
  <Property Name="TripId" Type="Edm.Int32" Nullable="false" />
  <Property Name="ShareId" Type="Edm.Guid" Nullable="false" />
  <Property Name="Name" Type="Edm.String" />
  <Property Name="Budget" Type="Edm.Single" Nullable="false" />
  <Property Name="Description" Type="Edm.String" />
  <Property Name="Tags" Type="Collection(Edm.String)" />
  <Property Name="StartsAt" Type="Edm.DateTimeOffset" Nullable="false" />
  <Property Name="EndsAt" Type="Edm.DateTimeOffset" Nullable="false" />
  <NavigationProperty Name="PlanItems" Type="Collection(Trippin.PlanItem)" />
</EntityType>
```

**GraphQL:**

```graphql
type Trip {
  TripId: Int!
  ShareId: GUID!
  Name: String
  Budget: Float!
  Description: String
  Tags: [String]
  StartsAt: DateTime!
  EndsAt: DateTime!
  PlanItems(queryOptions: QueryOptions): [PlanItem]
  PlanItemsById(id: ID): PlanItem
  GetInvolvedPeople: [Person]
}
```

**Note**: `GetInvolvedPeople` refers to a bound function. We address how bound functions are translated [here](#bound-functions).

## Inheritance and Abstract types

In OData, entity (and complex types) can extend other types through inheritance. In GraphQL interfaces are used to create base types that other object types can implement.

The `ODataHandler` generates a [GraphQL interface type](https://graphql.org/learn/schema/#interfaces) for each base entity/complex type in the OData schema that other types inherit from. In addition to the interface, an object type is also created that implements the interface and has the same fields. Other object types that correspond to sub types of the base type will also implement the generated interface.

The names of the generated GraphQL types depend on whether the OData base type is abstract or not.

### When the base OData type is abstract
The following GraphQL types are generated:
- An interface with the same name as the abstract type (no prefix)
- An object type that implements the interface and has the same fields. It will have the same name but with the `T` prefix.

For example, given an OData abstract entity type called `MyAbstractType`, the following GraphQL types will be generated:
- `interface MyAbstractType`
- `type TMyAbstractType implements MyAbstractType`

### When the base OData type is NOT abstract
The following GraphQL types are generated:
- An interface with the same name as the OData type, but with the `I` prefix
- An object type that implements the interface and has the same fields. It will have the same name as the entity type (no prefix).

For example, given the entity/complex type `MyType`, the following GraphQL types will be generated:
- `interface IMyType`
- `type MyType implements IMyType`

### Complex Type Example

**OData:**
```xml
<ComplexType Name="Location">
  <Property Name="Address" Type="Edm.String" />
  <Property Name="City" Type="Trippin.City" />
</ComplexType>
<ComplexType Name="City">
  <Property Name="Name" Type="Edm.String" />
  <Property Name="CountryRegion" Type="Edm.String" />
  <Property Name="Region" Type="Edm.String" />
</ComplexType>
<ComplexType Name="AirportLocation" BaseType="Trippin.Location">
  <Property Name="Loc" Type="Edm.GeographyPoint" />
</ComplexType>
<ComplexType Name="EventLocation" BaseType="Trippin.Location">
  <Property Name="BuildingInfo" Type="Edm.String" />
</ComplexType>
```

**GraphQL:**
```graphql
interface ILocation {
  Address: String
  City: City
}

type Location implements ILocation {
  Address: String
  City: City
}

type AirportLocation implements ILocation {
  Loc: String
  Address: String
  City: City
}

type EventLocation implements ILocation {
  BuildingInfo: String
  Address: String
  City: City
}
```

### Entity Type Example

**OData**

```xml
<EntityType Name="PlanItem">
  <Key>
    <PropertyRef Name="PlanItemId" />
  </Key>
  <Property Name="PlanItemId" Type="Edm.Int32" Nullable="false" />
  <Property Name="ConfirmationCode" Type="Edm.String" />
  <Property Name="StartsAt" Type="Edm.DateTimeOffset" Nullable="false" />
  <Property Name="EndsAt" Type="Edm.DateTimeOffset" Nullable="false" />
  <Property Name="Duration" Type="Edm.Duration" Nullable="false" />
</EntityType>
<EntityType Name="Event" BaseType="Trippin.PlanItem">
  <Property Name="OccursAt" Type="Trippin.EventLocation" />
  <Property Name="Description" Type="Edm.String" />
</EntityType>
<EntityType Name="PublicTransportation" BaseType="Trippin.PlanItem">
  <Property Name="SeatNumber" Type="Edm.String" />
</EntityType>
<EntityType Name="Flight" BaseType="Trippin.PublicTransportation">
  <Property Name="FlightNumber" Type="Edm.String" />
  <NavigationProperty Name="Airline" Type="Trippin.Airline" />
  <NavigationProperty Name="From" Type="Trippin.Airport" />
  <NavigationProperty Name="To" Type="Trippin.Airport" />
</EntityType>
```

**GraphQL**

```graphql
interface IPlanItem {
  PlanItemId: Int!
  ConfirmationCode: String
  StartsAt: DateTime!
  EndsAt: DateTime!
  Duration: ISO8601Duration!
}

type PlanItem implements IPlanItem {
  PlanItemId: Int!
  ConfirmationCode: String
  StartsAt: DateTime!
  EndsAt: DateTime!
  Duration: ISO8601Duration!
}

type Event implements IPlanItem {
  OccursAt: EventLocation
  Description: String
  PlanItemId: Int!
  ConfirmationCode: String
  StartsAt: DateTime!
  EndsAt: DateTime!
  Duration: ISO8601Duration!
}

interface IPublicTransportation {
  SeatNumber: String
  PlanItemId: Int!
  ConfirmationCode: String
  StartsAt: DateTime!
  EndsAt: DateTime!
  Duration: ISO8601Duration!
}

type PublicTransportation implements IPublicTransportation & IPlanItem {
  SeatNumber: String
  PlanItemId: Int!
  ConfirmationCode: String
  StartsAt: DateTime!
  EndsAt: DateTime!
  Duration: ISO8601Duration!
}

type Flight implements IPublicTransportation {
  FlightNumber: String
  Airline: Airline
  From: Airport
  To: Airport
  SeatNumber: String
  PlanItemId: Int!
  ConfirmationCode: String
  StartsAt: DateTime!
  EndsAt: DateTime!
  Duration: ISO8601Duration!
}
```

*Note*: `PublicTransportation` inherits from `PlanItem`, but it's also a base type (of `Flight`). So the `IPublicTransportation` interface is created in addition to `IPlanItem`. And the object type `PublicTransportation` implements both interfaces.

(TODO add example for abstract types)

## Entity Types and Complex Types as Input types

For each entity and complex type that is used as an argument to an operation, a corresponding GraphQL [input type](https://graphql.org/learn/schema/#input-types) is also created. The input type is used for arguments to operations that take that type of object as input. For example, when creating an entity, an input type corresponding to that entity is used. The input type takes the name of the corresponding OData type with the `Input` suffix.

The input type will be generated with the same fields as the corresponding entity/complex type. However, unlike generated object types and interfaces, **the input type does not have fields corresponding to navigation properties or bound functions/actions**.

Fields that refer to other complex types will use the corresponding input type rather than object type (see examples below). In a GraphQL schema, [you cannot mix input types and output types](https://graphql.org/learn/schema/#input-types).

### Update Input Type

An additional input type is created for each entity type that can be used in an update operation (i.e. an OData PATCH/PUT request against the entity set). This additional input type has the same fields as the "default" input type we've talked about, the only exception is that all the fields are made optional (i.e. nullable). It takes the same name as the OData type but with the `UpdateInput` suffix.

### Complex Type Example

**OData:**
```xml
<ComplexType Name="City">
  <Property Name="Name" Type="Edm.String" />
  <Property Name="CountryRegion" Type="Edm.String" />
  <Property Name="Region" Type="Edm.String" />
</ComplexType>

<ComplexType Name="AirportLocation" BaseType="Trippin.Location">
  <Property Name="Loc" Type="Edm.GeographyPoint" />
</ComplexType>
```

**GraphQL:**
```graphql
input CityInput {
  Name: String
  CountryRegion: String
  Region: String
}

input AirportLocationInput {
  Loc: String
  Address: String
  City: CityInput
}
```

*Note*: The `City` field of `AirportLocationInput` refers to the input type `CityInput` and not the object type (output type) `City`.

*Note*: Input types don't implement interfaces. The corresponding OData type inherits from `Location`, but in the generated GraphQL input types, this information is lost.

### Entity Type Example
**OData:**
```xml
<EntityType Name="Airport">
  <Key>
    <PropertyRef Name="IcaoCode" />
  </Key>
  <Property Name="Name" Type="Edm.String" />
  <Property Name="IcaoCode" Type="Edm.String" Nullable="false" />
  <Property Name="IataCode" Type="Edm.String" />
  <Property Name="Location" Type="Trippin.AirportLocation" />
</EntityType>
```
**GraphQL:**
```graphql
input AirportInput {
  Name: String
  IcaoCode: String!
  IataCode: String
  Location: AirportLocationInput
}

input AirportUpdateInput {
  Name: String
  IcaoCode: String
  IataCode: String
  Location: AirportLocationInput
}
```

*Note:* In `AirportInput`, the `ICaoCode` field is non-nullable (required), but in the `AirportUpdateInput` it's optional.

## Open Types

If the type is open, a field called `rest` of type `JSON` is added to the generated object type, interface and input type. It resolves the full result object returned from OData the service (i.e. the object in the response with all its properties)

(TODO: more info and example)


## Entity Sets and CRUD operations

For each entity set in the entity container, a few queries and mutations are generated to allow fetching, creating, updating and deleting entities. Read operations are created as fields of the base `Query` type and Create/Update/Delete operations are created as fields of the base `Mutation` type.

### Read operations

The following queries are generated:
- A query to fetch entities in the entity set. The query takes the name of the entity set. It takes [`queryOptions: QueryOptions`](#query-options) as an optional argument. The return type is a list of the [corresponding object type](#entity-types-and-complex-types) ([or interface if the entity type has sub types](#inheritance-and-abstract-types)).
- A query for fetching an entity by its key. The name is formed by the name of the entity set + `By` + name of key field. It takes the key as argument. The return type is the corresponding object type or interface.
- A query to count entities. The name is formed by the name of the entity set + `Count`. It takes an option `queryOptions: QueryOptions` argument. The return type is `Int`.

### Write Operations
The following mutations are generated:
- A mutation to create an entity. The name is formed by `create` + entity set name. It takes the [corresponding input type](#entity-types-and-complex-types-as-input-types) as an optional argument. The return type is the [corresponding object type](#entity-types-and-complex-types) ([or interface if the entity type has sub types](#inheritance-and-abstract-types).
- A mutation to update an entity by key. The name is formed by `update` + entity set name + `By` + key field name. It accepts the key and update input as arguments, the update input takes the [corresponding update input type](#update-input-type). The return type is the [corresponding object type](#entity-types-and-complex-types) ([or interface if the entity type has sub types](#inheritance-and-abstract-types))
- A mutation to delete an entity by key. The name is formed by `delete` + entity set name + `By` + key field name.
 It accepts the key as argument. The return type is `JSON`.
- Fields to fetch the entity set collection as well as an entity by id are also added to the `Mutation` type in addition to the `Query`. This is because it's possible to nest a call to a bound action on the entities returned by these endpoints.

### Example

Assuming the OData service exposes an entity set called `People` of entity type `Person`, the following table shows the generated GraphQL queries and mutations and corresponding OData endpoints.

| OData | GraphQL | Target
--------|---------|------
`GET /People` | `People(queryOptions: QueryOptions): [IPerson]` | `Query` and `Mutation`
`GET /People/<UserName>` | `PeopleByUserName(UserName: String!): IPerson` | `Query` and `Mutation`
`GET /People/$count` | `PeopleCount(queryOptions: QueryOptions): Int` | Query
`POST /People` | `createPeople(input: PersonInput): IPerson` | Mutation
`PATCH /People/<UserName>` | `updatePeopleByUserName(UserName: String!, input: PersonUpdateInput): IPerson` | Mutation
`DELETE /People/<UserName>` | `deletePeopleByUserName(UserName: String!): JSON` | Mutation

## Singletons

For each singleton a query is created to fetch the singleton entity. It takes no arguments. The return type is the [corresponding object type](#entity-types-and-complex-types) ([or interface if the entity type has sub types](#inheritance-and-abstract-types). The query is added as a field of the base `Query` type.

### Example

Assume the OData service exposes a singleton called `Me` of type `Person`. The following table shows the generated GraphQL queries and mutations and corresponding OData endpoints.

| OData | GraphQL | Target
--------|---------|-----
`GET /Me` | `Me: IPerson` | `Query`

## Bound Functions

Each bound function is added as a field to both the object type and interface that correspond to the entity type the function is bound to. The field has the same name as the OData function. The return type also matches the return type of the OData function.

**Important**: The OData handler does not add arguments to the generated field even when the OData function has parameters. **This is a bug**. Ideally, it should create arguments that match the parameters of the OData function.

*Note*: If the return type of the OData function is an entity type, the GraphQL return type will be the object type matching the entity type even if the entity type has sub types. This is different from the return type of [entity set read operations](#read-operations) which is matched to an interface if one exists.(TODO: investigate whether this should be the correct behavior or whether it's a bug).

### Example

**OData:**
```xml
<Function Name="GetFavoriteAirline" IsBound="true" EntitySetPath="person">
  <Parameter Name="person" Type="Trippin.Person" />
  <ReturnType Type="Trippin.Airline" />
</Function>
<Function Name="GetFriendsTrips" IsBound="true">
  <Parameter Name="person" Type="Trippin.Person" />
  <Parameter Name="userName" Type="Edm.String" Nullable="false" Unicode="false" />
  <ReturnType Type="Collection(Trippin.Trip)" />
</Function>
<Function Name="GetInvolvedPeople" IsBound="true">
  <Parameter Name="trip" Type="Trippin.Trip" />
  <ReturnType Type="Collection(Trippin.Person)" />
</Function>
```
**GraphQL:**

```graphql
interface IPerson {
  # other fields...

  GetFavoriteAirline: Airline
  GetFriendsTrips: [Trip]
}

type Person implements IPerson {
  # other fields...

  GetFavoriteAirline: Airline
  GetFriendsTrips: [Trip]
}

type Trip {
  # other fields...

  GetInvolvedPeople: [Person]
}
```

- Note that `GetFavoriteAirline` and `GetFriendsTrips` are added to both `IPerson` and `Person` types. They are also added to the other types that implement the `IPerson` interface (e.g. `Manager`).
- Note that `GetInvolvedPeople` returns `[Person]` instead of `[IPerson]`
- Note also that none of the fields accept arguments.

### Request mapping

**OData**
`GET People("russelwhite")/GetFavoriteAirline`

**GraphQL**

```graphql
query PeopleById(UserName: "russellwhyte") {
   GetFavoriteAirline
}
```
Note that we cannot translate OData requests to functions that accept arguments because of the above-mentioned bug in the OData handler.

### Resolver implementation and request handling for bound functions
Based on the current resolve implementation, the above GraphQL query will result in 2 OData requests:
- `GET People("russellwhyte")`
- `GET People("russelwhite")/GetFavoriteAirline`

To construct the second request, the `@odata.id` from the response of the first request is used (`People("russellwhyte")`) and the name of the operation is appended to it. These requests are **not executed in parallel** since one depends on the response of the other. Generally resolvers for fields are executed after the resolver of the parent object (TODO: confirm that this is the case). It seems there's potential room for optimizations here.

## Bound Actions
- Each bound action is added as a field to both the object type and interface corresponding to the OData entity type it is bound to
- The generated field has the same name as the OData action
- The field accepts arguments matching the names and types of the parameters of the OData action
- If a parameter type is a complex or entity type, then the argument type will be the corresponding [input type](#entity-types-and-complex-types-as-input-types)
- The return type is `JSON` regardless of the actual return type of the OData action. (TODO: this seems like room for improvement)

### Example

**OData:**
```xml
<Action Name="UpdateLastName" IsBound="true">
  <Parameter Name="person" Type="Trippin.Person" />
  <Parameter Name="lastName" Type="Edm.String" Nullable="false" Unicode="false" />
  <ReturnType Type="Edm.Boolean" Nullable="false" />
</Action>
<Action Name="ShareTrip" IsBound="true">
  <Parameter Name="personInstance" Type="Trippin.Person" />
  <Parameter Name="userName" Type="Edm.String" Nullable="false" Unicode="false" />
  <Parameter Name="tripId" Type="Edm.Int32" Nullable="false" />
</Action>
```

**GraphQL:**
```graphql
interface IPerson {
  # other fields...

  UpdateLastName(person: PersonInput, lastName: String!): JSON
  ShareTrip(personInstance: PersonInput, userName: String!, tripId: Int!): JSON
}

type Person implements IPerson {
  # other fields...

  UpdateLastName(person: PersonInput, lastName: String!): JSON
  ShareTrip(personInstance: PersonInput, userName: String!, tripId: Int!): JSON
}
```

- Note that just like bound functions, the bound actions are added to both `IPerson` and `Person` (as well as other object types which implement `IPerson`).
- Note that they both use `PersonInput` as the type of the first argument.
- Note that they both use the return type `JSON` even though `UpdateLastName` returns a boolean and `ShareTrip` has no return value in OData.

### Request mapping

**OData:**
```
POST People('russellwhyte')/ShareTrip

{
  "userName": "scottketchum",
  "tripId": 3
}
```

**GraphQL:**

```graphql
mutation PeopleById("russellwhyte") {
   ShareTrip(userName: "scottketchum", tripId: 3)
}
```

- Note that `personInstance` argument is optional, so we don't need to pass it to the field.
- Note that we can access `PeopleById` through a mutation since the field is added to both the `Query` and `Mutation` types. (We can also access it through `People`).

### Resolver implementation and request handling for bound actions
[Similar to bound functions](#resolver-implementation-and-request-handling-for-bound-functions), the above GraphQL mutation will result in 2 OData requests:
- `GET People("russellwhyte")`
- `POST People("russelwhyte")/ShareTrip` (with parameters in the request body like in the request mapping example)

The resolver for the `ShareTrip` field uses the `@odata.id` from the containing object to construct the 2nd request to call the action. These 2 requests are made sequentially.

## Unbound Functions
- Each unbound function as added as field to the base `Query` type
- The field has the same name as the OData function
- The field accepts arguments matching the names and types of the OData function's parameters
- The return type matches the return type of the OData function. If the OData return type is an entity type, the GraphQL return type is the corresponding object type (the object type is used even if there's a corresponding interface, **this could be a potential bug**)

### Example

**OData:**
```xml
<Function Name="GetPersonWithMostFriends">
  <ReturnType Type="Trippin.Person" />
</Function>
<Function Name="GetNearestAirport">
  <Parameter Name="lat" Type="Edm.Double" Nullable="false" />
  <Parameter Name="lon" Type="Edm.Double" Nullable="false" />
  <ReturnType Type="Trippin.Airport" />
</Function>
```
**GraphQL:**
```graphql
type Query {
  GetPersonWithMostFriends: Person
  GetNearestAirport(lat: Float!, lon: Float!): Airport
}
```

Note that `GetPersonWithMostFriends` returns `Person` instead of `IPerson`.

### Request mapping

**OData:**
```
GET GetNearestAirport(lat = 20.14, lon = 39.6)
```
**GraphQL:**
```graphql
query GetNearestAirport(lat: 20.14, lon: 39.6) {}
```

### Resolver implementation for unbound functions

The resolver makes `GET` request to the corresponding OData function, passing the parameter as part of the request URL. (TODO: looking at the code, it doesn't seem like string arguments are quoted, **this could be a potential bug**)

## Unbound Actions
- Each unbound action is added as a field to the base `Mutation` type
- It has the same name as the OData action
- It accepts arguments matching the names and types of the OData action
- The return type is `JSON` regardless of the return type of the OData action

### Example

**OData:**
```xml
<Action Name="ResetDataSource" />
```
**GraphQL:**
```graphql
type Mutation {
  ResetDataSource: JSON
}
```

### Request mapping

**OData:**
```
POST ResetDataSource
```

**GraphQL:**
```graphql
mutation ResetDataSource {}
```

### Resolver implementation for unbound actions

The resolver makes a `POST` request to the corresponding OData action, passing the arguments as JSON payload in the request body.

## Final GraphQL Query and Mutation types

A GraphQL schema defines "base" [query and mutation types](https://graphql.org/learn/schema/#the-query-and-mutation-types). These represent the entry points to every GraphQL query. As we've seen, the `Query` type generated by the `ODataHandler` encapsulates all the fields that represent top-level read operations, the `Mutation` type encapsulates fields that represent all the top-level write operations (or operations with side-effects) or read operations that return entities (since these can have side effects by nesting calls to bound actions).

### Example

**OData:**
```xml
<EntityContainer Name="Container">
  <EntitySet Name="People" EntityType="Trippin.Person">
    <NavigationPropertyBinding Path="BestFriend" Target="People" />
    <NavigationPropertyBinding Path="Friends" Target="People" />
    <NavigationPropertyBinding Path="Trippin.Employee/Peers" Target="People" />
    <NavigationPropertyBinding Path="Trippin.Manager/DirectReports" Target="People" />
  </EntitySet>
  <EntitySet Name="Airlines" EntityType="Trippin.Airline">
    <Annotation Term="Org.OData.Core.V1.OptimisticConcurrency">
      <Collection>
        <PropertyPath>
          Name
        </PropertyPath>
      </Collection>
    </Annotation>
  </EntitySet>
  <EntitySet Name="Airports" EntityType="Trippin.Airport" />
  <Singleton Name="Me" Type="Trippin.Person">
    <NavigationPropertyBinding Path="BestFriend" Target="People" />
    <NavigationPropertyBinding Path="Friends" Target="People" />
    <NavigationPropertyBinding Path="Trippin.Employee/Peers" Target="People" />
    <NavigationPropertyBinding Path="Trippin.Manager/DirectReports" Target="People" />
  </Singleton>
  <FunctionImport Name="GetPersonWithMostFriends" Function="Trippin.GetPersonWithMostFriends" EntitySet="People" />
  <FunctionImport Name="GetNearestAirport" Function="Trippin.GetNearestAirport" EntitySet="Airports" />
  <ActionImport Name="ResetDataSource" Action="Trippin.ResetDataSource" />
</EntityContainer>
```

**GraphQL:**

```graphql
schema {
  query: Query
  mutation: Mutation
}

type Query {
  GetPersonWithMostFriends: Person
  GetNearestAirport(lat: Float!, lon: Float!): Airport
  Me: Person
  People(queryOptions: QueryOptions): [IPerson]
  PeopleByUserName(UserName: String!): IPerson
  PeopleCount(queryOptions: QueryOptions): Int
  Airlines(queryOptions: QueryOptions): [Airline]
  AirlinesByAirlineCode(AirlineCode: String!): Airline
  AirlinesCount(queryOptions: QueryOptions): Int
  Airports(queryOptions: QueryOptions): [Airport]
  AirportsByIcaoCode(IcaoCode: String!): Airport
  AirportsCount(queryOptions: QueryOptions): Int
}

type Mutation {
  ResetDataSource: JSON
  People(queryOptions: QueryOptions): [IPerson]
  PeopleByUserName(UserName: String!): IPerson
  createPeople(input: PersonInput): IPerson
  deletePeopleByUserName(UserName: String!): JSON
  updatePeopleByUserName(UserName: String!, input: PersonUpdateInput): IPerson
  Airlines(queryOptions: QueryOptions): [Airline]
  AirlinesByAirlineCode(AirlineCode: String!): Airline
  createAirlines(input: AirlineInput): Airline
  deleteAirlinesByAirlineCode(AirlineCode: String!): JSON
  updateAirlinesByAirlineCode(AirlineCode: String!, input: AirlineUpdateInput): Airline
  Airports(queryOptions: QueryOptions): [Airport]
  AirportsByIcaoCode(IcaoCode: String!): Airport
  createAirports(input: AirportInput): Airport
  deleteAirportsByIcaoCode(IcaoCode: String!): JSON
  updateAirportsByIcaoCode(IcaoCode: String!, input: AirportUpdateInput): Airport
}
```