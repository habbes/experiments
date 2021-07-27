This is a collection of issues that I uncovered during my initial investigation of the OData handler in GraphQL Mesh before starting to make improvements. Issues that may be uncovered after starting to make improvements to the library might not be listed here.

- The filter field in `QueryOptions` input type is a string that takes an OData filter expression. This requires GraphQL users to know the OData filter syntax. This is not the ideal experience. A better implementation would be to provide a typed querying experience similar to what [Hasura provides](https://hasura.io/docs/latest/graphql/core/databases/postgres/queries/index.html#queries). [See this comment](https://github.com/Urigo/graphql-mesh/issues/787#issuecomment-667048220) and [this one](https://github.com/Urigo/graphql-mesh/issues/787#issuecomment-706294602) for more context.
- The `QueryOptions` input type is used for all operations that accept query options. But some query options like `top`, `skip`, `orderby` should only be used for operations that return collection responses.
- It doesn't seem like there's support for passing query options to functions that return a collection of entities.
- There's no support for capabilities, i.e. reading the capabilities to find out which fields are selectable, expandable, etc.
- No write support for singletons (only reading seems to be supported).
- Since bound actions are accessible through both mutations and queries, you can cause side effects to the backend through a query by calling a bound action. Also, just having the same fields in both `Query` and `Mutation` can be confusing to the developer.
- Fields corresponding to bound functions don't accept arguments even if the OData function accepts parameters. This is a bug.
- Not sure whether composite keys are supported
- The return type for fields generated for actions is `JSON` regardless of the actual return type of the corresponding action
- Fields corresponding to functions have an object type as return type even when the object type implements an interface. In such cases the return type ought to be the corresponding interface, since objects corresponding to subtypes can also be returned.
- Due to the nested fashion of field resolvers, queries that involve navigation properties or bound actions and functions may result in multiple (sequential) requests sent to the OData service even if a single OData request could achieve the same thing. This is despite the [batching feature provided by the data loader used in the OData handler](https://identitydivision.visualstudio.com/OData/_wiki/wikis/OData.wiki/23867/GraphQL-OData-Handler?anchor=resolvers-and-request-handling). The batching cannot optimize requests issued from nested resolvers where one resolver depends on the result of the parent resolver.


For more issues, check out the [open issues on GitHub](https://github.com/Urigo/graphql-mesh/issues?q=is%3Aissue+is%3Aopen++odata).