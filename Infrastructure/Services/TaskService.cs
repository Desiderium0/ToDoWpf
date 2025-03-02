using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToDoWPF.Models;

namespace ToDoWPF.Infrastructure.Services
{
    class TaskService
    {
        private HttpClient _httpClient;
        private const string _url = "https://localhost:7290/api/ToDo";

        public TaskService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<TaskModel>> GetTask()
        {
            return null;
        }
    }
}
