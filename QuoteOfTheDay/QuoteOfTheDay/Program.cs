using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Program
{
    static readonly HttpClient client = new HttpClient();

    public static async Task Main()
    {
        Console.Write("Do you want a random quote or the quote of the day? (random/day): ");
        string quoteType = Console.ReadLine();

        await GetQuote(quoteType, client);
    }

    public static async Task GetQuote(string quoteType, HttpClient httpClient)
    {
        string endpoint = quoteType == "random" ? "random" : "today";
        HttpResponseMessage response = await httpClient.GetAsync($"https://zenquotes.io/api/{endpoint}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        var quoteResponse = JsonConvert.DeserializeObject<QuoteResponse[]>(responseBody);
        var quote = quoteResponse[0];

        Console.WriteLine($"Quote: {quote.q}");
        Console.WriteLine($"- {quote.a}");
    }
}


public class QuoteResponse
{

    public string q { get; set; } // Quote
    public string a { get; set; } // Author
}