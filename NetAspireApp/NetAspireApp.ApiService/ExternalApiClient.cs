namespace NetAspireApp.ApiService
{
    public class ExternalApiClient(HttpClient client)
    {
        public async Task SendHit(string text)
        {
            await client.GetAsync($"/6e070bc6-dc7c-4090-bbbf-e3a55a0b7108?data={text}");
        }
    }
}
