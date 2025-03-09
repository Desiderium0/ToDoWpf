using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using ToDoWPF.Models;

namespace ToDoWPF.Infrastructure.Services
{
    internal class TaskService
    {
        private HttpClient _httpClient;
        private JsonSerializerOptions _option;
        private readonly string _url = "https://localhost:7290/api/ToDo";

        public TaskService()
        {
            _httpClient = new HttpClient();
            _option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        // GET /api/ToDo
        public async Task<List<TaskModel>> GetTaskAsync()
        {
            var responseMessage = await _httpClient.GetAsync(_url);
            string json = responseMessage.Content.ReadAsStringAsync().Result;

            var tasks = JsonSerializer.Deserialize<List<TaskModel>>(json, _option);

            return tasks;
        }

        // Post /api/ToDo
        public async Task AddTaskAsync()
        {
            var task = new TaskModel
            {
                Title = $"Новая задача",
                Description = "Описание задачи",
                Created = DateTime.Now,
                IsCompleted = false
            };
            var json = JsonSerializer.Serialize(task);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _httpClient.PostAsync(_url, content);
        }

        // Delete /api/ToDo/{id}
        public async Task DeleteTaskAsync(TaskModel task)
        {
            if (task != null)
                await _httpClient.DeleteAsync(_url + "/" + task.Id);
        }

        // Put /api/ToDo/{id}
        public async Task PutTaskAsync(TaskModel task)
        {
            var json = JsonSerializer.Serialize(task);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            if (task != null)
                await _httpClient.PutAsync($"{_url}/{task.Id}", content);
            
        }
    }
}
