using Microsoft.OData.Client;
using Microsoft.OData.SampleService.Models.TripPin;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ODataClientExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Press enter key to start");
            //Console.ReadLine();
            //ConcurrentCreateRequestsWithIndividualContexts().Wait();
            //ConcurrentFetchRequestsWithIndividualContexts().Wait();
            ConcurrentMixedRequests().Wait();
        }

        static async Task Test()
        {
            var uri = new Uri("https://services.odata.org/V4/(S(3jjpqgfjmnsmsy4twnnqedz3))/TripPinServiceRW");
            var context = new DefaultContainer(uri);
            //await GetPerson(context);
            //await GetPeople(context);
            await CreatePerson(context);
        }

        static async Task GetPerson(DefaultContainer context)
        {
            var personRequest = context.People.ByKey("russellwhyte");
            var person = await personRequest.GetValueAsync();
            Console.WriteLine(person.FirstName);
        }

        static async Task GetPeople(DefaultContainer context)
        {
            var people = await context.People.ExecuteAsync();
            foreach (var person in people)
            {
                Console.WriteLine(person.FirstName);
            }
        }

        static async Task CreatePerson(DefaultContainer context, string username = "johndoe")
        {
            var person = new Person()
            {
                UserName = username,
                FirstName = "John",
                LastName = "Doe",
            };

            person.AddressInfo.Add(new Location()
            {
                Address = "Addr1",
                City = new City
                {
                    Name = "City1",
                    Region = "Region1",
                    CountryRegion = "CR1"
                }
            });

            person.AddressInfo.Add(new Location()
            {
                Address = "Addr2",
                City = new City
                {
                    Name = "City2",
                    Region = "Region2",
                    CountryRegion = "CR2"
                }
            });

            context.AddObject("People", person);
            await context.SaveChangesAsync();
            Console.WriteLine(person.UserName);
        }

        static async Task GetProperty(DefaultContainer context)
        {
            var locations = await context.ExecuteAsync<Location>(new Uri("People('russellwhyte')/AddressInfo", UriKind.Relative));
            foreach (var location in locations)
            {
                Console.WriteLine(location.Address);
            }
        }
        static async Task ConcurrentRequestsWithSharedContext()
        {
            var uri = new Uri("https://services.odata.org/V4/(S(3jjpqgfjmnsmsy4twnnqedz3))/TripPinServiceRW");
            var context = new DefaultContainer(uri);
            await Task.WhenAll(CreatePerson(context), GetPerson(context));
        }

        static async Task ConcurrentCreateRequestsWithIndividualContexts()
        {
            var uri = new Uri("https://services.odata.org/V4/(S(3jjpqgfjmnsmsy4twnnqedz3))/TripPinServiceRW");
            var ctx1 = new DefaultContainer(uri);
            var ctx2 = new DefaultContainer(uri);
            Task createPersonTask = Task.Run(() => CreatePerson(ctx1));
            Task createPerson2Task = Task.Run(() => CreatePerson(ctx2, "foobar"));

            await Task.WhenAll(createPersonTask, createPerson2Task); ;
        }

        static async Task ConcurrentFetchRequestsWithIndividualContexts()
        {
            var uri = new Uri("https://services.odata.org/V4/(S(3jjpqgfjmnsmsy4twnnqedz3))/TripPinServiceRW");
            var ctx1 = new DefaultContainer(uri);
            var ctx2 = new DefaultContainer(uri);
            Task task1 = Task.Run(() => GetPeople(ctx1));
            Task task2 = Task.Run(() => GetPeople(ctx2));

            await Task.WhenAll(task1, task2); ;
        }

        static async Task ConcurrentMixedRequests()
        {
            var uri = new Uri("https://services.odata.org/V4/(S(3jjpqgfjmnsmsy4twnnqedz3))/TripPinServiceRW");
            var ctx1 = new DefaultContainer(uri);
            var ctx2 = new DefaultContainer(uri);
           
            Task task1 = Task.Run(() => GetPeople(ctx2));
            Task task2 = Task.Run(() => GetProperty(ctx1));

            await Task.WhenAll(task1, task2); ;
            await CreatePerson(new DefaultContainer(uri));
        }

        static async Task RepeatExperiment(Func<Task> experiment, int count)
        {
            for (int i = 0; i < count; i++)
            {
                await experiment();
            }    
        }

    }
}
