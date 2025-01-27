using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;

namespace Lib;

public partial class SemanticBinder
{
    public static SemanticNode Bind(SlimQueryNode root, IEdmModel model, IEdmType type)
    {
        var visitor = new TreeHandler(model, type);
        var semanticTree = root.Accept(visitor);
        return semanticTree;
    }
}
