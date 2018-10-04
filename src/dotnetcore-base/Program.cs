using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using thehinc.data.Contracts;
using thehinc.data.IoC;
using thehinc.ioc;
using thehinc.IoC;

namespace dotnetcore_base
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IoCContainer.Container.Install (new IncInstaller ());
            IoCContainer.Container.Install (new InventoryInstaller ());
           var _inventoryContext = IoCContainer.Container.Resolve<IInventoryContext>(new {ConnectionString = ""});
           _inventoryContext.Migrate();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel()
                .UseUrls("http://0.0.0.0:60000", "http://0.0.0.0:60001");
    }
}
