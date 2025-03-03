using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ToDoWPF.Models;

namespace ToDoWPF.Infrastructure.Services
{
    class TaskService
    {
        private HttpClient _httpClient;
        private readonly string _url = "https://localhost:7290/api/ToDo";

        public TaskService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<TaskModel>> GetTaskAsync()
        {
            var responseMessage = await _httpClient.GetAsync(_url);
            string json = responseMessage.Content.ReadAsStringAsync().Result;

            var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var tasks = JsonSerializer.Deserialize<List<TaskModel>>(json, option);

            return tasks;
        }
    }
}
