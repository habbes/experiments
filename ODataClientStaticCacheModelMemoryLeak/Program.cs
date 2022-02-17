using Microsoft.OData.Client;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.ModelBuilder;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using Trippin;

namespace ODataClientStaticCacheModelMemoryLeak
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter e to use EdmModel-based model, otherwise a CsdlSemanticsModel will be used: ");
            string choice = Console.ReadLine();
            Func<DataServiceContext> createDsc;
            if (choice == "e")
            {
                createDsc = CreateDscWithModelBuilder;
                Console.WriteLine("Using EdmModel-based model...");
            }
            else
            {
                createDsc = CreateDscWithCsdl;
                Console.WriteLine("Using CsdlSemanticsModel-based model...");
            }

            PrintAndWait("About to start experiments. This is a good time to take a baseline memory snapshot.");

            RunExperiment(createDsc);

            PrintAndWait("Finished experiments. This is a good time to take a final memory snapshot.");

        }

        static void RunExperiment(Func<DataServiceContext> createDsc)
        {
            for (int i = 0; i < 3; i++)
            {
                MakeRequestsWithDsc(createDsc).Wait();
            }

            PrintAndWait("About to run GC. Consider taking a memory snapshot before proceeding.");

            GC.Collect();

            PrintAndWait("GC completed. Consider taking a memory snapshot before proceeding.");

            for (int i = 0; i < 3; i++)
            {
                MakeRequestsWithDsc(createDsc).Wait();
            }
        }

        static async Task MakeRequestsWithDsc(Func<DataServiceContext> createDsc)
        {
            DataServiceContext context = createDsc();
            var peopleQuery = context.CreateQuery<Person>("People");
            DataServiceRequest request = new DataServiceRequest<Person>(peopleQuery.RequestUri);
            DataServiceResponse responses = await context.ExecuteBatchAsync(request);
            foreach (QueryOperationResponse response in responses)
            {
                if (response is QueryOperationResponse<Person> personResponse)
                {
                    foreach (Person person in personResponse)
                    {
                        Console.WriteLine(person.FirstName);
                    }
                }
            }
        }

        static DataServiceContext CreateDscWithCsdl()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly
                .GetManifestResourceNames()
                .FirstOrDefault(s => s.EndsWith("TrippinServiceCsdl.xml"));
            Stream resourceStream = assembly.GetManifestResourceStream(resourceName);
            XmlReader reader = XmlReader.Create(resourceStream);
            IEdmModel model;
            CsdlReader.TryParse(reader, out model, out var errors);


            var serviceRoot = "https://services.odata.org/V4/(S(fen3zessflkbrlnxhncwqocq))/TripPinServiceRW/";
            var context = new DataServiceContext(new Uri(serviceRoot), ODataProtocolVersion.V4);
            context.Format.UseJson(model);
            return context;
        }

        static DataServiceContext CreateDscWithModelBuilder()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Person>("People");
            builder.EntitySet<Trip>("Trips");
            builder.EntitySet<Airline>("Airlines");
            builder.EntitySet<Airport>("Airports");
            builder.Singleton<Person>("Me");

            var model = builder.GetEdmModel();
            var serviceRoot = "https://services.odata.org/V4/(S(fen3zessflkbrlnxhncwqocq))/TripPinServiceRW/";
            var context = new DataServiceContext(new Uri(serviceRoot), ODataProtocolVersion.V4);
            context.Format.UseJson(model);
            return context;
        }

        static void PrintAndWait(string message = "")
        {
            Console.WriteLine(message);
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
