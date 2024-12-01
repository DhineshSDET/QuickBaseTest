using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using QuickBaseApiTest.Builder;
using QuickBaseApiTest.Model.v1;

namespace QuickBaseApiTest;

public static class HttpHelper
{
    public static async Task<HttpResponseMessage> PostAsync<T>(
        HttpClient client,
        string endpoint,
        T requestPayload)
    {
        var jsonContent = JsonConvert.SerializeObject(requestPayload);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        return await client.PostAsync(endpoint, content);
    }

    public static async Task<T> ParseResponseAsync<T>(HttpResponseMessage response)
    {
        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseBody);
    }

    public static async Task<HttpResponseMessage> GetAsync<T>(
        HttpClient client,
        string endpoint)
    {
        return await client.GetAsync(endpoint);
    }

    public static async Task<HttpResponseMessage> DeleteAsync(
        HttpClient client,
        string url,
        DeleteFieldRequest requestBody)
    {
        var jsonRequest = JsonConvert.SerializeObject(requestBody);
        using var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        return await client.DeleteAsync(url);
    }

    public static async Task<HttpResponseMessage> DeleteAsync<T>(
        HttpClient client,
        string endpoint,
        T requestPayload)
    {
        // Serialize the request payload into JSON
        var jsonContent = JsonConvert.SerializeObject(requestPayload);

        // Create an HttpRequestMessage for DELETE
        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(endpoint),
            Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
        };
        return await client.SendAsync(requestMessage);
    }

    public static async Task<PostFieldResponse> ParseResponseAsync<PostFieldResponse>(HttpResponseMessage response,
        string name)
    {
        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<PostFieldResponse>(responseBody);
    }

    public static async Task<PostFieldResponse> PostAsync<T>(string response)
    {
        return JsonConvert.DeserializeObject<PostFieldResponse>(response);
    }

    public static async Task<TResponseContent> DeserializeResponseMessageBodyAsync<TResponseContent>(
        HttpResponseMessage response)
    {
        if (response.Content == null || response.Content.Headers.ContentLength <= 0) return default;

        if (response.Content.Headers.ContentType.ToString().Contains("application/xml"))
        {
            var serializer = new DataContractSerializer(typeof(TResponseContent));
            var dataContractSerializer = serializer;
            return (TResponseContent)dataContractSerializer.ReadObject(await response.Content.ReadAsStreamAsync());
        }

        if (response.Content.Headers.ContentType.ToString().Contains("application/json"))
            return JsonConvert.DeserializeObject<TResponseContent>(await response.Content.ReadAsStringAsync());

        throw new NotSupportedException(
            "Deserialization of anything other than application/xml or application/json is not supported. See inner exception for status code.",
            new HttpRequestException(
                $"StatusCode={response.StatusCode} ReasonPhrase={response.ReasonPhrase} ResponseContent={response.Content.ReadAsStringAsync().Result}"));
    }
}