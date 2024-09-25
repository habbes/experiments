using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ODataModelChangeAnalyzer;

internal interface IModelChange
{
    public ChangeKind ChangeKind { get; }
    /// <summary>
    /// The target element of the change. If this is an addition or modification,
    /// this refers to the element in the new model. If this is a deletion, this
    /// refers to the element in the old model.
    /// </summary>
    public IEdmElement Target { get; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum ChangeKind
{
    Addition,
    Deletion,
    Modification
}