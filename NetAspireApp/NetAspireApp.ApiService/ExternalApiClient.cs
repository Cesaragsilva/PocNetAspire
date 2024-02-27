namespace NetAspireApp.ApiService
{
    public class ExternalApiClient(HttpClient client)
    {

        public async Task SendHit(string text)
        {
            await client.GetAsync($"/79110639-99a4-4306-8b79-4082a0ccdb29?data={text}");
        }
    }
}
