[[_TOC_]]

## Overview

GraphQL Mesh’s OData support is provided by an [OData handler](https://www.graphql-mesh.com/docs/handlers/odata) that takes an OData CSDL document as input and generates a GraphQL schema. The handler is responsible for generating corresponding queries and mutations to possible OData requests. It is also responsible for providing the resolvers that transform the mutations and queries into web requests that are sent to the OData service.

For more information about installation, usage and configuration, check out the [official Mesh docs](https://www.graphql-mesh.com/docs/getting-started/introduction) and [OData handler docs](https://www.graphql-mesh.com/docs/handlers/odata);

## Design and Implementation

### OData Handler basics

The `ODataHandler` implements GraphQL Mesh's `MeshHandler` interface which is defined as follows (TODO: link to source):

```typescript
export interface MeshHandler<TContext = any> {
  getMeshSource: () => Promise<MeshSource<TContext>>;
}
```

The `MeshSource` interface in turn is defined as (TODO: link to source)

```typescript
export type MeshSource<ContextType = any, InitialContext = any> = {
  schema: GraphQLSchema;
  executor?: Executor;
  subscriber?: Subscriber;
  contextVariables?: (keyof InitialContext)[];
  contextBuilder?: (initialContextValue: InitialContext) => Promise<ContextType>;
  batch?: boolean;
};
```

A `MeshSource` is basically an object that tells the underlying graphql subsystem how to handle queries/mutations by exposing the relevant resolvers. The `MeshSource` returned by the `ODataHandler` contains the generated GraphQL Schema and the `contextBuilder`. The `contextBuilder` is simply a function that returns a context object. The context is an object that contains any metadata that might be needed to resolve a request.

When executing a request, GraphQL Mesh creates the context using input from the caller and extends the context object by calling the `contextBuilder` of the source handler. The context object will be passed down to resolvers when fulfilling the request.

The OData Handler's `contextBuilder` creates a context that provides a [DataLoader](https://github.com/graphql/dataloader) that will be used by the resolvers to make requests to the OData service (more on that later).

When executing a request with GraphQL mesh, you can also pass additional context properties as input. The MS Graph example adds an `accessToken` to the context which will be later used to inject an `Authorization` header to the requests sent to the MS Graph API:

```typescript
async function main() {
  const client = await getMsGraphGraphQLClient();
  const result = await client.execute(
    /* GraphQL */ `
      query fetchRecentEmails {
        me {
          displayName
          officeLocation
          messages(queryOptions: { top: 3 }) {
            subject
            isRead
            from {
              emailAddress {
                address
              }
            }
          }
        }
      }
    `,
    {},
    {
      accessToken: 'someAccessToken',
    }
  );
  console.log({
    result,
  });
}
```

And the config file would specify the headers:

```yaml
sources:
  - name: Microsoft Graph
    handler:
      odata:
        baseUrl: https://graph.microsoft.com/v1.0/
        batch: json
        operationHeaders:
          Authorization: Bearer {context.accessToken}
```

### GraphQL Schema Generation

The `ODataHandler` downloads the CSDL schema of the OData service (by hitting the `$metadata` endpoint) and parses it. It then uses the `SchemaComposer` from the [graphql-compose](https://graphql-compose.github.io/) library to build the corresponding GraphQL schema. The `SchemaComposer` allows you to programmatically build a GraphQL schema by defining types, enums, inputs, etc. It also allows you to modify the types, add/remove properties, etc. after they've been created.

The handler goes through the CSDL schema elements and uses the `SchemaComposer` to create a type for each OData type, a query for each entity set or singleton, queries for fetching entities by key, queries for unbound functions, type fields with inputs for bound functions, mutations for updating/deleting entities, mutations for unbound actions, type fields for bound actions, etc.

For a detailed mapping of OData requests and types to GraphQL, [visit this reference](https://identitydivision.visualstudio.com/OData/_wiki/wikis/OData.wiki/24217/OData-to-GraphQL-Mapping-Reference).

### Resolvers and Request handling

The `SchemaComposer` is also used to register resolvers for the queries and mutations that are generated for the GraphQL schema. The GraphQL subsystem uses a resolver function to fetch the data for a particular field (To learn more about GraphQL execution and resolver functions, [check the official docs](https://graphql.org/learn/execution/)).

The resolver functions registered by the `ODataHandler` create a HTTP request correspond to the query/mutation, send it to the OData service, transform the response to the format expected by GraphQL (TODO example of response transformation for both successful and error responses) and return it.

A resolver function is created for each query and mutation that is generated, which implements the execution of the OData request. In addition to mutations and queries, a resolver is also created for each navigation property of an entity. If the result does not already contain the expanded navigation property, the OData handler creates a "separate" OData request to fetch it (technically, the requests are not necessarily sent separately as we'll see later). The handler as a configuration option `expandNavProps` that populates the `$expand` query option with requested navigation properties before making the request, this could avoid the extra requests. But this option is set to false by default (TODO: verify whether it works as expected when set to true). The handler also populates the `$select` query option with the requested fields before executing the request.

The resolver function receives, among other arguments, the context object that we talked about earlier in this document. The resolver doesn't execute the request directly, instead it uses the `DataLoader` that's passed to the context. Before we dive into what the `DataLoader` does, let's first explore the problem it addresses. A single GraphQL request might result in multiple resolver functions being invoked (e.g. fetching entities including navigation properties, multiple queries in a single GraphQL request, etc.). This would result in multiple requests sent to the OData service which is inefficient, especially considering that we could get the same results using a single OData request (e.g. using `$expand` to fetch related data, or using `$batch` to fetch unrelated data in a single request).

What the `DataLoader` does is to coalesce requests made in near each other, e.g. within the same GraphQL request (technically, it actually coalesces requests made within the same frame execution or tick, but this can behaviour can be customized) and allows you to batch them into a single request or call to the backend server. The batching logic implemented by the `ODataHandler` simply constructs a single OData `$batch` request composed of the different individual requests and split up the $batch response to individual responses each matching its request. The handler provides implementation for multipart and json batch and you can configure which to use by using the `batch` handler config option. If no option is set, then the individual requests will be sent separately (but concurrently). Besides batching, the data loader also does caching: if the same request is executed again, it will be retrieved from cache. It's important to note that a new instance of the `DataLoader` is created for each GraphQL request, so the batching and caching are only handled within the context of a single GraphQL call.

Here's some conceptual code illustrating how the data loader pattern is used. First we define our data loader (in the handler this handled by the `dataLoaderFactory` that's passed to the `contextBuilder`):

```typescript
const dataLoader = new DataLoader(async (requests: Request[]): Promise<Response[]> => {
   const batchRequest = combineRequestsIntoBatch(requests);
   const batchResponse = await fetch(batchRequest);
   const responses = splitBatchReponseIntoIndividualResponses(batchResponse);
   return responses;
});
```

Then, instead of fetching data directly from the data source, or executing the requests directly, we pass them to the `dataLoader.load()` method. In the following example, we make 4 separate calls:

```typescript
const resp1 = await dataLoader.load(request1);
const resp2 = await dataLoader.load(request2);
// somewhere else in the codebase
const resp3 = await dataLoader.load(request3);
const resp4 = await dataLoader.load(request4);
```

From the observer's point of view, these 4 calls are independent of one another, and they may even be made at different locations of the codebase. But if they occur within the same frame of execution (depending on how the data loader scheduling has been configured), the data loader will collect all of them behind the scene and send them to the call back function which was passed to the constructor. That function will create a execute a single batch request, split the response and return the individual responses. The data loader will resolve each promise of the `dataLoader.load()` methods with the corresponding response.

To learn more the data loader, check out [the docs](https://github.com/graphql/dataloader).

(TODO sample of graphql query and generated OData batch request).