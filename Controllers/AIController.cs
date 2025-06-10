using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using InventoryManagement.Models;

namespace InventoryManagement.Controllers
{
    public class AIController : Controller
    {
        private readonly string apiKey = "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"; 

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AskAI(string prompt)
        {
            string model = "HuggingFaceH4/zephyr-7b-beta";

         
            var inventories = InventoryController.GetInventories();

            StringBuilder contextBuilder = new StringBuilder("Here is my inventory collection:\n");
            foreach (var item in inventories)
            {
                contextBuilder.AppendLine($"- Name: {item.Name}, Category: {item.Category}, Quantity: {item.Quantity}, " +
                                          $"Price: {item.Price}, Status: {item.Status}");
            }

            string fullPrompt = $"{contextBuilder}\n\nUser question: {prompt}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var body = new
                {
                    inputs = fullPrompt,
                    parameters = new { max_new_tokens = 150 }
                };

                var content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(body),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync($"https://api-inference.huggingface.co/models/{model}", content);
                var responseString = await response.Content.ReadAsStringAsync();

                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(responseString);
                string aiReply = result?[0]?.generated_text ?? "No response from AI.";

                ViewBag.AIResponse = aiReply;
                ViewBag.UserQuestion = prompt;

                return View("Index");
            }
        }
    }
}
