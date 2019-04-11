using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;


namespace CrawlerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Task<List<Car>> cars = startCrawlerasync();

            Console.WriteLine("// First Car:");
            Console.WriteLine("Model: " + cars.Result.FirstOrDefault().Model);
            Console.WriteLine("Price: " + cars.Result.FirstOrDefault().Price);
            Console.WriteLine("Link: " + cars.Result.FirstOrDefault().Link);
            Console.WriteLine("ImageUrl: " + cars.Result.FirstOrDefault().ImageUrl);
            Console.ReadLine();
        }

        private static async Task<List<Car>> startCrawlerasync()
        {
            var url = "http://www.automobile.tn/neuf/bmw.3/";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var cars = new List<Car>();


            var divs = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "").Equals("versions-item")).ToList();

            foreach (var div in divs)
            {
                var car = new Car
                {
                    Model = div.Descendants("h2").FirstOrDefault().InnerText,
                    Price = div.Descendants("div").FirstOrDefault().InnerText,
                    Link = div.Descendants("a").FirstOrDefault().ChildAttributes("href").FirstOrDefault().Value,
                    ImageUrl = div.Descendants("img").FirstOrDefault().ChildAttributes("src").FirstOrDefault().Value   
                };
                cars.Add(car);
            }

            return cars;
        }
    }
}
