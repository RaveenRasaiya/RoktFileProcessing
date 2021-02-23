using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Rokt.Application;
using Rokt.Application.Interfaces;
using System;
using System.Linq;

namespace Rokt.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine($"Process started at {DateTime.Now}");
            ServiceProvider serviceProvider = InitDepencyInjection();
            var fileService = serviceProvider.GetService<IDataService>();

            var result = fileService.ExtraData(@"C:\RAVEEN\EXAMS\Rokt\Files\backend-technical-test\sample3.txt", DateTime.Parse("2004-09-29T04:35:22Z"), DateTime.Parse("2004-10-03T03:32:09Z"));
            System.Console.WriteLine(result.Count());
            System.Console.WriteLine(JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            }));

            System.Console.WriteLine($"Process completed at {DateTime.Now}");
            System.Console.ReadLine();
        }

        private static ServiceProvider InitDepencyInjection()
        {
            return new ServiceCollection()
                    .AddSingleton<IFileService, FileService>()
                    .AddSingleton<IDataService, DataService>()
                    .BuildServiceProvider();
        }
    }

}
