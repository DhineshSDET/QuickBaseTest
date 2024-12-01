using System.Net;
using QuickBaseApiTest.Builder;
using QuickBaseApiTest.Model.v1;

namespace QuickBaseApiTest.Tests.Fields.DELETE;

[TestClass]
public class DeleteFieldTest : BaseTest
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
        endpoint = $"{BaseUrl}/fields?tableId={TableId}";
        if (TestMethod == "FieldMissingAuthorizationReturns401Unauthorized")
        {
            httpClient.DefaultRequestHeaders.Add("QB-Realm-Hostname", RealmHostname);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"{Token}");
        }
        else if (TestMethod == "IncorrectTableIdReturns400BadRequest")
        {
            httpClient.DefaultRequestHeaders.Add("QB-Realm-Hostname", RealmHostname);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"{Token}");
            endpoint = $"{BaseUrl}/fields?tableId=InvalidtableId";
        }
        else
        {
            httpClient.DefaultRequestHeaders.Add("QB-Realm-Hostname", RealmHostname);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"QB-USER-TOKEN {Token}");
        }
    }
    [TestMethod]
    [TestCategory("Regression")]
    public async Task PassingAllRequiredReturns200()
    {
        //Arrange
        var createResponse = CreateNewField(httpClient,"CreateFieldOne");
        var request = new DeleteFieldRequest
        {
            FieldIds = [createResponse.Result.Id]
        };
        //Act
        var response = await HttpHelper.DeleteAsync<DeleteFieldRequest>(httpClient, endpoint, request);
        var responseData = await HttpHelper.DeserializeResponseMessageBodyAsync<DeleteFieldResponse>(response);
        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expected status code 200.");
        Assert.IsNull(responseData.Errors);
    }
    [TestMethod]
    public async Task IncorrectTableIdReturns400BadRequest()
    {
        //Arrange
        var createResponse = CreateNewField(httpClient, "CreateFieldTwo");
        var request = new DeleteFieldRequest
        {
            FieldIds = [createResponse.Result.Id]
        };
        //Act
        var response = await HttpHelper.DeleteAsync<DeleteFieldRequest>(httpClient, endpoint, request);
        //Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Expected status code 400.");
    }
    [TestMethod]
    public async Task InvalidFieldIdReturns404BadRequest()
    {
        //Arrange
        var request = new DeleteFieldRequest
        {
            FieldIds = new List<int>(120000)
        };
        // Act
        var response = await HttpHelper.DeleteAsync<DeleteFieldRequest>(httpClient, endpoint, request);
        //Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Expected status code 404.");
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