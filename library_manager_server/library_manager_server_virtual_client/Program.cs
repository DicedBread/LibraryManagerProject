namespace library_manager_server_virtual_client;

class Program
{
    public static async Task Main(string[] args)
    {
        await new VirtualClient().Run();
    }
}

