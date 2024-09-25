using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;
using System.Diagnostics.CodeAnalysis;

namespace ODataModelChangeAnalyzer;

internal class ChangesAnalyzer
{
    private readonly IEdmModel originalModel;
    private readonly IEdmModel newModel;
    private readonly List<IModelChange> changes = new();

    public ChangesAnalyzer(IEdmModel originalModel, IEdmModel newModel)
    {
        this.originalModel = originalModel;
        this.newModel = newModel;
    }

    public virtual IEnumerable<IModelChange> AnalyzeChanges()
    {
        foreach (var newElement in newModel.SchemaElements)
        {
            // possible elements
            // schema type, term, entity container, function, action
            if (!TryFindSchemaElement(newElement, originalModel, out var oldElement))
            {
                this.AddChange(new SchemaElementAdded(newElement));
            }
            else
            {
                this.AnalyzeChanges(oldElement, newElement);
            }
        }

        foreach (var originalElement in originalModel.SchemaElements)
        {
            if (!TryFindSchemaElement(originalElement, newModel, out _))
            {
                this.AddChange(new SchemaElementRemoved(originalElement));
            }
        }

        return changes;
    }

    protected void AnalyzeChanges(IEdmSchemaElement originalElement, IEdmSchemaElement newElement)
    {
        if (originalElement is IEdmSchemaType && newElement is IEdmSchemaType)
        {
            this.AnalyzeChanges(originalElement, newElement);
        }
        else if (originalElement is IEdmEntityContainer && newElement is IEdmEntityContainer)
        {

        }
        else if (originalElement is IEdmAction && newElement is IEdmAction)
        {

        }
        else if (originalElement is IEdmFunction && newElement is IEdmFunction)
        {

        }
        else if (originalElement is IEdmTerm && newElement is IEdmTerm)
        {

        }

        ThrowUnsupportedElementTypes(originalElement, newElement);
    }

    protected void AnalyzeChanges(IEdmSchemaType originalElement, IEdmSchemaType newElement)
    {
        if (originalElement is IEdmStructuredType oldType && newElement is IEdmStructuredType newType)
        {
            AnalyzeChanges(oldType, newType);
        }
        else if (originalElement is IEdmTypeDefinition oldTypeDef && newElement is IEdmTypeDefinition newTypeDef)
        {
            AnalyzeChanges(oldTypeDef, newTypeDef);
        }
        else
        {
            ThrowUnsupportedElementTypes(originalElement, newElement);
        }
    }

    protected void AnalyzeChanges(IEdmStructuredType originalElement, IEdmStructuredType newElement)
    {
        if (originalElement is IEdmEntityType baseEntityType && newElement is IEdmEntityType newEntityType)
        {
            AnalyzeChanges(baseEntityType, newEntityType);
        }
        else if (originalElement is IEdmComplexType baseComplexType && newElement is IEdmComplexType newComplexType)
        {
            AnalyzeChanges(baseComplexType, newComplexType);
        }
        else if (originalElement.TypeKind != newElement.TypeKind)
        {
            AddChange(new StructuredTypeKindChanged(newElement, originalElement));
        }
        else
        {
            ThrowUnsupportedElementTypes(originalElement, newElement);
        }   
    }

    protected virtual void AnalyzeChanges(IEdmEntityType originalElement, IEdmEntityType newElement)
    {
        AnalyzeStructuralTypeChangesCore(originalElement, newElement);
    }

    protected virtual void AnalyzeChanges(IEdmComplexType originalElement, IEdmComplexType newElement)
    {
        AnalyzeStructuralTypeChangesCore(originalElement, newElement);
    }

    private void AnalyzeStructuralTypeChangesCore(IEdmStructuredType originalElement, IEdmStructuredType newElement)
    {
        if (newElement.IsOpen && !originalElement.IsOpen)
        {
            this.AddChange(new StructuredTypeChangedFromOpenToClose(newElement, originalElement));
        }

        if (!newElement.IsOpen && originalElement.IsOpen)
        {
            this.AddChange(new StructuredTypeChangedFromOpenToClose(newElement, originalElement));
        }

        if (newElement.BaseType == null && originalElement.BaseType != null)
        {
            this.AddChange(new StructuredTypeBaseTypeRemoved(newElement, originalElement));
        }

        if (newElement.BaseType != null && originalElement.BaseType == null)
        {
            this.AddChange(new StructuredTypeBaseTypeAdded(newElement, originalElement));
        }

        if (newElement.BaseType?.FullTypeName() != originalElement.BaseType?.FullTypeName())
        {
            this.AddChange(new StructuredTypeBaseTypeChanged(newElement, originalElement));
        }

        // We search through all structural properties, including inherited ones.
        // While inspecting properties we'll detect changes to declaring types
        foreach (var property in newElement.StructuralProperties())
        {
            if (TryFindStructuralProperty(property, originalElement, out var originalProperty))
            {
                AnalyzeChanges(originalProperty, originalElement, property, newElement);
            }
            else
            {
                this.AddChange(new StructuredTypePropertyAdded(property, newElement, originalElement));
            }
        }

        foreach (var property in originalElement.StructuralProperties())
        {
            if (!TryFindStructuralProperty(property, newElement, out _))
            {
                this.AddChange(new StructuredTypePropertyRemoved(property, originalElement, newElement));
            }
        }

        foreach (var property in newElement.NavigationProperties())
        {
            // TODO: detect new and updated navigation properties
        }

        foreach (var property in originalElement.NavigationProperties())
        {
            // TODO: detected removed navigation properties
        }
    }

    protected virtual void AnalyzeChanges(IEdmTypeDefinition originalElement, IEdmTypeDefinition newElement)
    {
       
    }

    protected virtual void AnalyzeChanges(IEdmProperty originalProperty, IEdmProperty newProperty)
    {
        if (originalProperty is IEdmStructuralProperty originalStructuralProperty && newProperty is IEdmStructuralProperty newStructuralProperty)
        {
            this.AnalyzeChanges(originalStructuralProperty, newStructuralProperty);
        }
        else if (originalProperty is IEdmNavigationProperty originalNavProperty && newProperty is IEdmNavigationProperty newNavProperty)
        {
            this.AnalyzeChanges(originalNavProperty, newNavProperty);
        }
        else
        {
            ThrowUnsupportedElementTypes(originalProperty, newProperty);
        }
    }

    protected virtual void AnalyzeChanges(IEdmStructuralProperty originalProperty, IEdmStructuredType originalType, IEdmStructuralProperty newProperty, IEdmStructuredType newType)
    {
        if (originalProperty.Type.FullName() != newProperty.Type.FullName())
        {
            // property type changed
            this.AddChange(new StructuredTypePropertyTypeChanged(newProperty, originalProperty, newType, originalType));
        }

        if (originalProperty.PropertyKind != newProperty.PropertyKind)
        {
            // property kind changed (e.g. from navigation to structural)
            this.AddChange(new StructuredTypePropertyKindChanged(newProperty, originalProperty, newType, originalType));
        }

        if (originalProperty.Type.IsNullable && !newProperty.Type.IsNullable)
        {
            // property made nullable
            this.AddChange(new StructuredTypePropertyChangedToNullable(newProperty, originalProperty, newType, originalType));
        }

        if (!originalProperty.Type.IsNullable && newProperty.Type.IsNullable)
        {
            // property no longer nullable
            this.AddChange(new StructuredTypePropertyChangedToNonNullable(newProperty, originalProperty, newType, originalType));
        }

        if (originalProperty.DeclaringType.FullTypeName() != newProperty.DeclaringType.FullTypeName())
        {
            // property declaring type changed (e.g. moved to base type or moved from base type)
            this.AddChange(new StructuredTypePropertyDeclaringTypeChanged(newProperty, originalProperty, newType, originalType));
        }

        if (!originalProperty.IsKey() == newProperty.IsKey())
        {
            // property declaring type changed (e.g. moved to base type or moved from base type)
            this.AddChange(new StructuredTypePropertyAddedToKey(newProperty, originalProperty, newType, originalType));
        }
    }

    protected virtual void AnalyzeChanges(IEdmNavigationProperty navigationProperty, IEdmNavigationProperty newProperty)
    {

    }

    protected void AddChange(IModelChange change)
    {
        this.changes.Add(change);
    }

    protected IEnumerable<IModelChange> GetChanges()
    {
        return this.changes;
    }

    private void ThrowUnsupportedElementTypes(IEdmElement originalElement, IEdmElement newElement)
    {
        throw new InvalidOperationException($"Comparing types {originalElement.GetType().Name} and {newElement.GetType().Name} is unsupported.");
    }

    private bool TryFindStructuralProperty(IEdmStructuralProperty property, IEdmStructuredType type, [NotNullWhen(true)] out IEdmStructuralProperty? foundProperty)
    {
        foundProperty = null;
        var candidate = type.FindProperty(property.Name);
        if (candidate is IEdmStructuralProperty sp)
        {
            foundProperty = sp;
            return true;
        }

        return false;
    }

    private bool TryFindSchemaElement(IEdmSchemaElement element, IEdmModel model, [NotNullWhen(true)] out IEdmSchemaElement? targetElement)
    {
        targetElement = null;

        IEdmSchemaElement? candidate = model.SchemaElements.FirstOrDefault(e =>
            e.FullName() == element.FullName() && e.SchemaElementKind == element.SchemaElementKind);

        if (candidate == null)
        {
            return false;
        }

        if (candidate.SchemaElementKind == EdmSchemaElementKind.TypeDefinition)
        {
            // ensure it's the same type of type definition
            if (element is IEdmEntityType && targetElement is not IEdmEntityType)
            {
                return false;
            }

            if (element is IEdmComplexType && targetElement is not IEdmComplexType)
            {
                return false;
            }

            if (element is IEdmTypeDefinition && targetElement is not IEdmTypeDefinition)
            {
                return false;
            }
        }

        targetElement = candidate;
        return true;
    }
}
