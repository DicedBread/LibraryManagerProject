using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace library_manager_server_virtual_client;


class VirtualClient{
    
    // static readonly HttpClient client = new HttpClient();
    string domain = "https://localhost:8080";

    public VirtualClient(){
    }   

    public async Task Run(){
        using HttpResponseMessage res = await new LibraryUrlBuilder(domain).Login("test@test", "test");
        res.EnsureSuccessStatusCode();
        Console.WriteLine(res.StatusCode);
        Console.WriteLine(res.Headers);
        
        using HttpResponseMessage logoutRes = await new LibraryUrlBuilder(domain).Logout();
        logoutRes.EnsureSuccessStatusCode();
        Console.WriteLine(logoutRes.StatusCode);
        
    }

}