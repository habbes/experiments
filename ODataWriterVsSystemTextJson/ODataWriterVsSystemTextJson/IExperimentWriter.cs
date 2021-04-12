using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ODataWriterVsSystemTextJson
{
    interface IExperimentWriter
    {
        Task WriteCustomers(IEnumerable<Customer> payload, Stream stream);
    }
}
