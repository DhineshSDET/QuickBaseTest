using System.Net;
using QuickBaseApiTest.Builder;
using QuickBaseApiTest.Model.v1;

namespace QuickBaseApiTest.Tests.Fields.POST;

[TestClass]
public class CreateFieldTest : BaseTest
{
    private static string endpoint = string.Empty;
    private static HttpClient httpClient;
    private static readonly string fieldLabelOne = "AutomationFieldOne";
    private static readonly string fieldLabelTwo = "AutomationFieldTwo";
    private static int fieldId;
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
        else if (TestMethod == "FieldWithIncorrectTableIdReturns400BadRequest")
        {
            httpClient.DefaultRequestHeaders.Add("QB-Realm-Hostname", RealmHostname);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"QB-USER-TOKEN {Token}");
            endpoint = $"{BaseUrl}/fields?tableId=invalidtableid";
        }
        else if (TestMethod == "FieldWithIncorrectHeadersReturns403Forbidden")
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
    [TestCategory("Smoke")]
    [TestCategory("Regression")]
    public async Task FieldWithAllRequiredReturns200Ok()
    {
        // Arrange
        var request = FieldRequestFactory.CreateFieldRequest(fieldLabelOne);
        // Act
        var response = await HttpHelper.PostAsync(httpClient, endpoint, request);
        var responseData = await HttpHelper.DeserializeResponseMessageBodyAsync<PostFieldResponse>(response);
        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expected status code 200.");
        Assert.IsNotNull(responseData, "Response data should not be null.");
        Assert.AreEqual(responseData.Label, fieldLabelOne, "Field Label should be equal to sent in request");
        fieldId = responseData.Id; //storing in a static variable to delete in test clean up
        //Cleanup
        await DeleteField();
    }
    [TestMethod]
    public async Task FieldExistsReturn400()
    {
        // Arrange
        var request = FieldRequestFactory.CreateFieldRequest(fieldLabelTwo);
        // Act
        var response = await HttpHelper.PostAsync(httpClient, endpoint, request);
        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Expected status code 400.");
    }
    [TestMethod]
    public async Task FieldMissingAuthorizationReturns401Unauthorized()
    {
        // Arrange
        var request = FieldRequestFactory.CreateFieldRequest("AutomationFieldNameUnauthorized401");
        // Act
        var response = await HttpHelper.PostAsync(httpClient, endpoint, request);
        // Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, "Expected status code 401.");
    }
    [TestMethod]
    public async Task FieldWithIncorrectTableIdReturns400BadRequest()
    {
        // Arrange
        var request = FieldRequestFactory.CreateFieldRequest("AutomationFieldNameIncorrectTableID400");
        // Act
        var response = await HttpHelper.PostAsync(httpClient, endpoint, request);
        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Expected status code 400.");
    }
    [TestMethod]
    public async Task FieldWithIncorrectHeadersReturns403Forbidden()
    {
        // Arrange
        var request = FieldRequestFactory.CreateFieldRequest("AutomationFieldNameIncorrectHeader403");
        // Act
        var response = await HttpHelper.PostAsync(httpClient, endpoint, request);
        // Assert
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode, "Expected status code 403.");
    }
    [TestMethod]
    public async Task FieldMissingFieldLabelNameReturn403BadRequest()
    {
        // Arrange
        var request = FieldRequestFactory.CreateFieldRequest("");
        // Act
        var response = await HttpHelper.PostAsync(httpClient, endpoint, request);
        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Expected status code 400.");
    }
    [TestCleanup]
    public void Cleanup()
    {
        httpClient.Dispose();
    }
    public static async Task DeleteField()
    {
        var request = new DeleteFieldRequest
        {
            FieldIds = [fieldId]
        };
        await HttpHelper.DeleteAsync<DeleteFieldRequest>(httpClient, endpoint, request);
    }
}