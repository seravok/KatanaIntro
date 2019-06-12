using Microsoft.Owin.Hosting;
using Owin;
using System.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KatanaIntro
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    class Program
    {
        static void Main(string[] args)
        {
            string uri = "http://localhost:8080";
            using (WebApp.Start<startup>(uri))
            {
                Console.WriteLine("Started!");
                Console.ReadKey();
                Console.WriteLine("Stopping!");
            }
        }

    }
    public class startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.UseWelcomePage();
            //app.Use<HelloWorldComponent>();
            //app.Use(async (environment, next) =>
            //{
            //    foreach (var pair in environment.Environment)
            //    {
            //        Console.WriteLine("{0}:{1}", pair.Key, pair.Value);
            //    }

            //    await next();

            //});

            app.Use( async(environment, next) => 
            {
                Console.WriteLine("Requesting : " + environment.Request.Path);

                await next();

                Console.WriteLine("Respons : " + environment.Response.StatusCode);
            });

            ConfigureWebApi(app);

            app.UseHelloWorld();
            //app.Run(ctx =>
            //{
            //    return ctx.Response.WriteAsync("Hello World!");
            //});
        }

        private void ConfigureWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}",
                new { id = RouteParameter.Optional });
            app.UseWebApi(config);
        }
    }

    public static class AppBuilderExtentions
    {
        public static void UseHelloWorld(this IAppBuilder app)
        {
            app.Use<HelloWorldComponent>();

        }
    }
    public class HelloWorldComponent
    {
        AppFunc _next;
        public HelloWorldComponent(AppFunc next)
        {
            _next = next;
        }

        public Task Invoke(IDictionary<string, object> enviroment)
        {
            var response = enviroment["owin.ResponseBody"] as Stream;
            using (var writer = new StreamWriter(response))
            {
                return writer.WriteAsync("Hello!!");
            }
            //await _next(enviroment);
        }
    }
}
