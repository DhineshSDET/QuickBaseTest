using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuickBaseApiTest
{
    public static class TestDataHelper
    {
        public static IEnumerable<object[]> LoadJsonData(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var testCase = JsonSerializer.Deserialize<TestCase>(json);

            if (testCase == null)
            {
                throw new InvalidOperationException("Failed to deserialize JSON to a list of test cases.");
            }
            else
            {
                yield return new object[] { testCase.Input, testCase.Expected };
            }
        }
        public static IEnumerable<object[]> LoadMultiArrayJsonData(string filePath)
        {
            var json = File.ReadAllText(filePath);

            Console.WriteLine($"JSON Content: {json}");

            // Add options to handle case-insensitive property names
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var root = JsonSerializer.Deserialize<Root>(json);

            if (root?.TestCases == null)
            {
                throw new InvalidOperationException("Failed to deserialize JSON to a list of test cases.");
            }

            foreach (var testCase in root.TestCases)
            {
                yield return new object[] { testCase.Input, testCase.Expected };
            }
        }
        public class Root
        {
            public List<TestCase> TestCases { get; set; }
        }
        public class TestCase
        {
            public string Input { get; set; }
            public string Expected { get; set; }
        }

    }
}
