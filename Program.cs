using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static readonly HttpClient client = new HttpClient();

    static async Task Main()
    {
        try
        {
            Console.WriteLine("Available categories: inspire, management, sports");
            Console.Write("Enter a category: ");
            string category = Console.ReadLine();

            HttpResponseMessage response = await client.GetAsync($"https://zenquotes.io/api/random?category={category}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var quoteResponse = JsonConvert.DeserializeObject<QuoteResponse[]>(responseBody);
            var quote = quoteResponse[0];

            Console.WriteLine($"Quote of the day: {quote.q}");
            Console.WriteLine($"- {quote.a}");
        }
        catch(HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine($"Message :{e.Message} ");
        }
    }
}

public class QuoteResponse
{
    public string q { get; set; } // Quote
    public string a { get; set; } // Author
}