using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace library_manager_server_virtual_client;


class VirtualClient{
    
    static readonly HttpClient client = new HttpClient();

    public VirtualClient(){
    }   

    public async Task Run(){
        Console.WriteLine("Starting client");
        await GetBook();
    } 

    public async Task GetBook(){
        try
        {
            using HttpResponseMessage response = await client.GetAsync("http://localhost/api/Books?limit=20&offset=0");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("COnt: \n" + responseBody);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    } 
}