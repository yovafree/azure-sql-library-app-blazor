//AuthorService.cs

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using BookManagementApp.Models;
using BookManagementApp.Extensions;

namespace BookManagementApp.Services
{

    public class AuthorResponse
    {
        public List<Author> Value { get; set; }
    }

    public class AuthorService
    {
        private readonly HttpClient _httpClient;

        public AuthorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            try
            {
                var responseStream = await _httpClient.GetStreamAsync($"http://127.0.0.1:5001/api/Author/id/{id}");
                var reader = new StreamReader(responseStream);
                var responseString = await reader.ReadToEndAsync();

                var authorResponse = JsonSerializer.Deserialize<AuthorResponse>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var author = authorResponse.Value.FirstOrDefault();
                return author;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching author: {ex}");
                return null;
            }
        }

        public async Task AddAuthorAsync(Author author)
        {
            var authorWithoutId = new
            {
                author.first_name,
                author.middle_name,
                author.last_name
            };

            var response = await _httpClient.PostAsJsonAsync("http://127.0.0.1:5001/api/Author", authorWithoutId);
            response.EnsureSuccessStatusCode(); 
        }
        
        public async Task<bool> UpdateAuthorAsync(Author author)
        {

            var authorWithoutId = new
            {
                author.first_name,
                author.middle_name,
                author.last_name
            };

            var response = await _httpClient.PatchAsJsonAsync($"http://127.0.0.1:5001/api/Author/id/{author.id}", authorWithoutId);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://127.0.0.1:5001/api/Author/id/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}        