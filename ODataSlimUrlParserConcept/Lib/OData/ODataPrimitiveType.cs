using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.OData;

internal class ODataPrimitiveType : IEdmPrimitiveType
{
    public EdmPrimitiveTypeKind PrimitiveKind => throw new NotImplementedException();

    public EdmSchemaElementKind SchemaElementKind => throw new NotImplementedException();

    public string Namespace => throw new NotImplementedException();

    public string Name => throw new NotImplementedException();

    public EdmTypeKind TypeKind => throw new NotImplementedException();
}

