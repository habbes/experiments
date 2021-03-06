[GraphQL Mesh](https://graphql-mesh.com) is an open source library by [The Guild](https://the-guild.dev) that provides a bridge between GraphQL and other data sources, including OData. It provides a gateway that allows users to query OData services (e.g. MS Graph) using GraphQL requests.

Support for various data sources in GraphQL Mesh is based on handlers. The handler is responsible for translating the schema of the target data source into a GraphQL schema, generating the corresponding type definitions, queries and mutations. It is also responsible for implementing the resolvers that transform the queries (and mutations) into requests sent to the data source and converting the responses back into GraphQL responses.

- [OData Handler](./odata-handler.md)
- [OData-GraphQL Mapping Reference](./odata-graphql-mapping-reference.md)
- [OData Handler Limitations and Issues](./odata-handler-limitations-issues.md)