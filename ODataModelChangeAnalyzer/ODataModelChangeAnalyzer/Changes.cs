using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataModelChangeAnalyzer;

record SchemaElementAdded(IEdmElement Target) : IModelChange
{
    public ChangeKind ChangeKind => ChangeKind.Addition;
}

record SchemaElementRemoved(IEdmElement Target) : IModelChange
{
    public ChangeKind ChangeKind => ChangeKind.Deletion;
}

record StructuredTypeChangedFromOpenToClose(IEdmElement Target, IEdmElement Original) : IModelChange
{
    public ChangeKind ChangeKind => ChangeKind.Modification;
}

record StructuredTypeChangedFromCloseToOpen(IEdmElement Target, IEdmElement Original) : IModelChange
{
    public ChangeKind ChangeKind => ChangeKind.Modification;
}

record StructuredTypeKindChanged(IEdmStructuredType NewType, IEdmStructuredType Original) : IModelChange
{
    public IEdmElement Target => NewType;
    public ChangeKind ChangeKind => ChangeKind.Modification;
}

record StructuredTypeBaseTypeRemoved(IEdmElement Target, IEdmElement Original) : IModelChange
{
    public ChangeKind ChangeKind => ChangeKind.Modification;
}

record StructuredTypeBaseTypeAdded(IEdmElement Target, IEdmElement Original) : IModelChange
{
    public ChangeKind ChangeKind => ChangeKind.Modification;
}

record StructuredTypeBaseTypeChanged(IEdmElement Target, IEdmElement Original) : IModelChange
{
    public ChangeKind ChangeKind => ChangeKind.Modification;
}

record StructuredTypePropertyAdded(IEdmStructuralProperty Property, IEdmStructuredType NewType, IEdmStructuredType OriginalType) : IModelChange
{
    public IEdmElement Target => NewType;
    public ChangeKind ChangeKind => ChangeKind.Modification;
}

record StructuredTypePropertyRemoved(
    IEdmStructuralProperty Property,
    IEdmStructuredType OriginalType,
    IEdmStructuredType NewType
) : IModelChange
{
    public IEdmElement Target => NewType;
    public ChangeKind ChangeKind => ChangeKind.Modification;
}


record StructuredTypePropertyTypeChanged(
    IEdmStructuralProperty NewProperty,
    IEdmStructuralProperty OldProperty,
    IEdmStructuredType NewType,
    IEdmStructuredType OriginalType
) : IModelChange
{
    public IEdmElement Target => NewType;
    public ChangeKind ChangeKind => ChangeKind.Modification;
}

record StructuredTypePropertyKindChanged(
    IEdmStructuralProperty NewProperty,
    IEdmStructuralProperty OldProperty,
    IEdmStructuredType NewType,
    IEdmStructuredType OriginalType
) : IModelChange
{
    public IEdmElement Target => NewType;
    public ChangeKind ChangeKind => ChangeKind.Modification;
}

record StructuredTypePropertyDeclaringTypeChanged(
    IEdmStructuralProperty NewProperty,
    IEdmStructuralProperty OldProperty,
    IEdmStructuredType NewType,
    IEdmStructuredType OriginalType
) : IModelChange
{
    public IEdmElement Target => NewType;
    public ChangeKind ChangeKind => ChangeKind.Modification;
}

record StructuredTypePropertyChangedToNullable(
    IEdmStructuralProperty NewProperty,
    IEdmStructuralProperty OldProperty,
    IEdmStructuredType NewType,
    IEdmStructuredType OriginalType
) : IModelChange
{
    public IEdmElement Target => NewType;
    public ChangeKind ChangeKind => ChangeKind.Modification;
}

record StructuredTypePropertyChangedToNonNullable(
    IEdmStructuralProperty NewProperty,
    IEdmStructuralProperty OldProperty,
    IEdmStructuredType NewType,
    IEdmStructuredType OriginalType
) : IModelChange
{
    public IEdmElement Target => NewType;
    public ChangeKind ChangeKind => ChangeKind.Modification;
}

record StructuredTypePropertyAddedToKey(
    IEdmStructuralProperty NewProperty,
    IEdmStructuralProperty OldProperty,
    IEdmStructuredType NewType,
    IEdmStructuredType OriginalType
) : IModelChange
{
    public IEdmElement Target => NewType;
    public ChangeKind ChangeKind => ChangeKind.Modification;
}

record StructuredTypePropertyRemovedFromKey(
    IEdmStructuralProperty NewProperty,
    IEdmStructuralProperty OldProperty,
    IEdmStructuredType NewType,
    IEdmStructuredType OriginalType
) : IModelChange
{
    public IEdmElement Target => NewType;
    public ChangeKind ChangeKind => ChangeKind.Modification;
}