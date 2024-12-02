using System.Net;
using System.Reflection;
using QuickBaseApiTest.Builder;
using QuickBaseApiTest.Model.v1;

namespace QuickBaseApiTest.Tests.Fields.POST;

[TestClass]
public class CreateFieldTest : BaseTest
{
    private static string endpoint = string.Empty;
    private static HttpClient httpClient;
    private static readonly string fieldLabelOne = "AutomationTestField";
    private static readonly string fieldLabelTwo = "AutomationTestOne";
    //private static int fieldId;
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
        var fieldId = responseData.Id; //storing in a static variable to delete in test clean up
        //Cleanup
        await DeleteField(fieldId);
    }
    [DataTestMethod]
    [DataRow("AutomationFieldInlineTest", "AutomationFieldInlineTest")]
    public async Task Returns200OkForInlineTestDataProvider(string inputLabel, string expectedLabel)
    {
        // Arrange
        var request = FieldRequestFactory.CreateFieldRequest(inputLabel);
        // Act
        var response = await HttpHelper.PostAsync(httpClient, endpoint, request);
        var responseData = await HttpHelper.DeserializeResponseMessageBodyAsync<PostFieldResponse>(response);
        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expected status code 200.");
        Assert.IsNotNull(responseData, "Response data should not be null.");
        Assert.AreEqual(responseData.Label, expectedLabel, "Field Label should be equal to sent in request");
        var fieldId = responseData.Id; //storing in a static variable to delete in test clean up
        //Cleanup
        await DeleteField(fieldId);
    }
    [DataTestMethod]
    [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
    public async Task Returns200OkForDynamicTestDataProvider(string inputLabel, string expectedLabel)
    {
        // Arrange
        var request = FieldRequestFactory.CreateFieldRequest(inputLabel);
        // Act
        var response = await HttpHelper.PostAsync(httpClient, endpoint, request);
        var responseData = await HttpHelper.DeserializeResponseMessageBodyAsync<PostFieldResponse>(response);
        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expected status code 200.");
        Assert.IsNotNull(responseData, "Response data should not be null.");
        Assert.AreEqual(responseData.Label, expectedLabel, "Field Label should be equal to sent in request");
        var fieldId = responseData.Id; //storing in a static variable to delete in test clean up
        //Cleanup
        await DeleteField(fieldId);
    }
    [DataTestMethod]
    [DynamicData(nameof(GetDynamicTestData), DynamicDataSourceType.Method)]
    public async Task Returns200OkForDynamicListTestDataProvider(string inputLabel, string expectedLabel)
    {
        // Arrange
        var request = FieldRequestFactory.CreateFieldRequest(inputLabel);
        // Act
        var response = await HttpHelper.PostAsync(httpClient, endpoint, request);
        var responseData = await HttpHelper.DeserializeResponseMessageBodyAsync<PostFieldResponse>(response);
        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expected status code 200.");
        Assert.IsNotNull(responseData, "Response data should not be null.");
        Assert.AreEqual(responseData.Label, expectedLabel, "Field Label should be equal to sent in request");
        var fieldId = responseData.Id; //storing in a static variable to delete in test clean up
        //Cleanup
        await DeleteField(fieldId);
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
    public static async Task DeleteField(int fieldId)
    {
        var request = new DeleteFieldRequest
        {
            FieldIds = [fieldId]
        };
        await HttpHelper.DeleteAsync<DeleteFieldRequest>(httpClient, endpoint, request);
    }
    public static IEnumerable<object[]> GetTestData()
    {
        // Get the path to the directory where the test assembly is located
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        // Navigate to the TestData folder relative to the base directory
        string filePath = Path.Combine(baseDirectory, "TestData", "testData.json");
        return TestDataHelper.LoadJsonData(filePath);
    }
    public static IEnumerable<object[]> GetDynamicTestData()
    {
        // Get the path to the directory where the test assembly is located
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        // Navigate to the TestData folder relative to the base directory
        string filePath = Path.Combine(baseDirectory, "TestData", "testDynamicData.json");
        return TestDataHelper.LoadMultiArrayJsonData(filePath);
    }
}