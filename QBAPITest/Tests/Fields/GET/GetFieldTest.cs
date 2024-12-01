using System.Net;
using QuickBaseApiTest.Builder;
using QuickBaseApiTest.Model.v1;

namespace QuickBaseApiTest.Tests.Fields.GET;

[TestClass]
public class GetFieldTest : BaseTest
{
    private static string endpoint = string.Empty;
    private static HttpClient httpClient;
    private static int fieldId = 0;
    public TestContext TestContext { get; set; }

    [TestInitialize]
    public void Initialize()
    {
        var TestMethod = TestContext.TestName;
        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Clear();
        endpoint = $"{BaseUrl}/fields/{fieldId}?tableId={TableId}";
        if (TestMethod == "MissingAuthorizationReturns401Unauthorized")
        {
            httpClient.DefaultRequestHeaders.Add("QB-Realm-Hostname", RealmHostname);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"{Token}");
        }
        else if (TestMethod == "InvalidFieldIdReturns404BadRequest")
        {
            httpClient.DefaultRequestHeaders.Add("QB-Realm-Hostname", RealmHostname);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"QB-USER-TOKEN {Token}");
            endpoint = $"{BaseUrl}/fields/00000?tableId={TableId}";
        }
        else if (TestMethod == "IncorrectTableIdReturns400BadRequest")
        {
            httpClient.DefaultRequestHeaders.Add("QB-Realm-Hostname", RealmHostname);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"QB-USER-TOKEN {Token}");
            endpoint = $"{BaseUrl}/fields/136?tableId=invalidtableid";
        }
        else if (TestMethod == "IncorrectHeadersReturns403Forbidden")
        {
            httpClient.DefaultRequestHeaders.Add("QB-Realm-Hostname", "");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"QB-USER-TOKEN {Token}");
        }
        else
        {
            httpClient.DefaultRequestHeaders.Add("QB-Realm-Hostname", RealmHostname);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"QB-USER-TOKEN {Token}");
        }
    }
    [TestMethod]
    [TestCategory("Regression")]
    public async Task PassingAllRequiredReturns200Ok()
    {
        //Arrange
        var createResponse = CreateNewField(httpClient, "CreateFieldAndGetField");
        fieldId = createResponse.Id;
        endpoint = $"{BaseUrl}/fields/{fieldId}?tableId={TableId}";
        // Act
        var response = await HttpHelper.GetAsync<GetFieldResponse>(httpClient, endpoint);
        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expected status code 200.");
    }
    [TestMethod]
    public async Task InvalidFieldIdReturns404BadRequest()
    {
        // Act
        var response = await HttpHelper.GetAsync<GetFieldResponse>(httpClient, endpoint);
        //Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expected status code 404.");
    }
    [TestMethod]
    public async Task IncorrectTableIdReturns400BadRequest()
    {
        // Act
        var response = await HttpHelper.GetAsync<GetFieldResponse>(httpClient, endpoint);
        //Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Expected status code 400.");
    }
    [TestMethod]
    public async Task MissingAuthorizationReturns401Unauthorized()
    {
        // Act
        var response = await HttpHelper.GetAsync<GetFieldResponse>(httpClient, endpoint);
        //Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, "Expected status code 401.");
    }
    [TestMethod]
    public async Task IncorrectHeadersReturns403Forbidden()
    {
        // Act
        var response = await HttpHelper.GetAsync<GetFieldResponse>(httpClient, endpoint);
        //Assert
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode, "Expected status code 403.");
    }
    [TestCleanup]
    public void Cleanup()
    {
        httpClient.Dispose();
    }
    private static async Task<PostFieldResponse> CreateNewField(HttpClient client, string label)
    {
        var request = FieldRequestFactory.CreateFieldRequest(label);
        var response = await HttpHelper.PostAsync(client, endpoint, request);
        return await HttpHelper.DeserializeResponseMessageBodyAsync<PostFieldResponse>(response);
    }
}