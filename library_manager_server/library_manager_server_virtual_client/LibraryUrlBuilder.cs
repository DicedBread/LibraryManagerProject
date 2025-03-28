using System.Net;

public class LibraryUrlBuilder
{

    private static CookieContainer cookieContainer = new CookieContainer();
    private static readonly HttpClient Client = new HttpClient(new HttpClientHandler(){
        CookieContainer = cookieContainer
    }); 

    readonly string domain;
    public string Url { get; private set; }

    public LibraryUrlBuilder(string domain)
    {
        this.domain = domain;
    }

    public async Task<HttpResponseMessage> GetBooks(int limit = 20, int offset = 0){
        string url = domain + $"/api/books$limit={limit}&offset={offset}";
        return await Client.GetAsync(url);
    }

    public async Task<HttpResponseMessage> GetBook(string isbn)
    {
        string url = domain + $"/api/books/{isbn}";
        return await Client.GetAsync(url);
    }

    public async Task<HttpResponseMessage> Register(string email, string password, string username){
        string url = domain + $"/api/Account/register";
        HttpRequestMessage req = new HttpRequestMessage(){
            RequestUri = new Uri(url),
            Method = HttpMethod.Post,
        };

        req.Headers.Add("email", email);
        req.Headers.Add("password", password);
        req.Headers.Add("username", username);
        
        return await Client.SendAsync(req);
    }

    public async Task<HttpResponseMessage> Login(string email, string password){
        string url = domain + $"/api/Account/login";
        HttpRequestMessage req = new HttpRequestMessage(){
            RequestUri = new Uri(url),
            Method = HttpMethod.Post,
        };
        req.Headers.Add("email", email);
        req.Headers.Add("password", password);
        return await Client.SendAsync(req);
    }

    public async Task<HttpResponseMessage> Logout(){
        string url = domain + $"/api/Account/logout";
        foreach (var item in cookieContainer.GetAllCookies().ToArray())
        {
            Console.WriteLine(cookieContainer.GetCookies(new Uri("https://localhost")).Count);
        }
        HttpRequestMessage req = new HttpRequestMessage(){
            RequestUri = new Uri(url),
            Method = HttpMethod.Post,
        };
        

        Console.WriteLine(req.Headers);

        return await Client.SendAsync(req);
    }
}