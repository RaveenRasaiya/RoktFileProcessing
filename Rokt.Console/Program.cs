using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Rokt.Application;
using Rokt.Application.Interfaces;
using Rokt.Domain.Requests;
using System;

namespace Rokt.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                ServiceProvider serviceProvider = InitDepencyInjection();
                System.Console.WriteLine($"Event Search Application Version: 1.0.0.0");

                EventSearchRequest searchRequest = ReadArgs();

                System.Console.WriteLine($"Process started at {DateTime.Now}");

                var dataService = serviceProvider.GetService<IDataService>();

                var response = dataService.ExtractData(searchRequest);

                System.Console.WriteLine(response);
                System.Console.WriteLine($"Process completed at {DateTime.Now}");
                System.Console.WriteLine("\npress any key to exit the process...");
                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        private static EventSearchRequest ReadArgs()
        {
            System.Console.WriteLine($"Please enter file path :");
            var filePath = System.Console.ReadLine();
            System.Console.WriteLine($"Please enter start date :");
            var startDate = System.Console.ReadLine();
            System.Console.WriteLine($"Please enter end date :");
            var endDate = System.Console.ReadLine();

            DateTime.TryParse(startDate, out DateTime _startDate);
            DateTime.TryParse(endDate, out DateTime _endDate);

            var searchRequest = new EventSearchRequest
            {
                FilePath = filePath,
                StartDate = _startDate,
                EndDate = _endDate,
                FeedSeparator = ' '
            };
            return searchRequest;
        }

        private static ServiceProvider InitDepencyInjection()
        {
            return new ServiceCollection()
                    .AddSingleton<IRecordService, RecordService>()
                    .AddSingleton<IFileService, FileService>()
                    .AddSingleton<IDataService, DataService>()
                    .AddScoped<IValidationService, ValidationService>()
                    .AddSingleton(new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        DateTimeZoneHandling = DateTimeZoneHandling.Utc
                    })
                    .BuildServiceProvider();
        }
    }
}