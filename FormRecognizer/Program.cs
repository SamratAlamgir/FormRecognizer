using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace FormRecognizer
{
    class Program
    {
        private const string SUBSCRIPTION_KEY = "3babc89bf9494959889621af0408fdb5";
        private const string MODEL_ID = "2ddf857b-2eac-4f99-977d-6feadac76ad5";
        private const string INCLUDE_TEXT_DETAILS = "false";
        private const string TEST_FILE_PATH = @"C:\Users\Samrat Alamgir\Desktop\Azure Congnitive\images\test-1.jpg";
        static async Task Main(string[] args)
        {
            var resultLocation = string.Empty;

            //resultLocation = await AnalyzeForm();
            //Console.WriteLine("Location URI for request:" + resultLocation);
            //// Need to wait since analyze will take some time
            //await Task.Delay(2000);

            resultLocation = "https://japaneast.api.cognitive.microsoft.com/formrecognizer/v2.0-preview/custom/models/2ddf857b-2eac-4f99-977d-6feadac76ad5/analyzeresults/e68aff47-937b-47e7-a63a-789efc6f735b";

            var resultData = await GetAnalyzeFormResult(resultLocation);

            DisplayResponseData(resultData);

            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }

        static async Task<string> AnalyzeForm()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SUBSCRIPTION_KEY);

            // Request parameters
            queryString["includeTextDetails"] = INCLUDE_TEXT_DETAILS;
            var uri = "https://japaneast.api.cognitive.microsoft.com/formrecognizer/v2.0-preview/custom/models/"+ MODEL_ID +"/analyze?" + queryString;

            HttpResponseMessage response;

            // Request body
            byte[] byteData = File.ReadAllBytes(TEST_FILE_PATH);

            string responseString = string.Empty;

            using (var content = new ByteArrayContent(byteData))
            {
                // use application/json if image url provided, image/jpeg  for byte content of image
                content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                Console.WriteLine("Analyze request sending...");

                response = await client.PostAsync(uri, content);

                Console.WriteLine("Analyze request sent");

                if (response.IsSuccessStatusCode)
                {
                    var opLocationKeyValuePair = response.Headers.SingleOrDefault(x => x.Key == "Operation-Location");
                    responseString = opLocationKeyValuePair.Value?.FirstOrDefault();
                }
            }

            return responseString;

        }

        static async Task<AnalyzeResultResponse> GetAnalyzeFormResult(string locationUri)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SUBSCRIPTION_KEY);

            //var uri = "https://japaneast.api.cognitive.microsoft.com/formrecognizer/v2.0-preview/custom/models/{modelId}/analyzeResults/{resultId}?" + queryString;

            var response = await client.GetAsync(locationUri);

            AnalyzeResultResponse resultData = null;

            if (response.IsSuccessStatusCode)
            {
                var contentString = await response.Content.ReadAsStringAsync();

                resultData = JsonConvert.DeserializeObject<AnalyzeResultResponse>(contentString);
            }

            return resultData;
        }

        static void DisplayResponseData(AnalyzeResultResponse responseData)
        {
            if (responseData != null)
            {
                var fields = responseData.analyzeResult.documentResults.First().fields;
                var fieldDict = fields.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToDictionary(prop => prop.Name, prop => prop.GetValue(fields, null));

                Console.WriteLine("Display data: ");

                foreach (var kvp in fieldDict)
                {
                    FormatOutput(kvp.Key, kvp.Value as FieldBase);
                }
            }
            else
            {
                Console.WriteLine("No data found");
            }
        }

        static void FormatOutput(string key, FieldBase fieldBase)
        {
            string displayText = $"{key}: \t";

            if (fieldBase != null)
            {
                displayText = displayText + $"{fieldBase.valueString}, Confidence: {fieldBase.confidence}";
            }

            Console.WriteLine(displayText);
        }
    }
}
