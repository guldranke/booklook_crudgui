using booklook_crudgui.Models;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace booklook_crudgui.Helpers {
    public class RestService {
        readonly HttpClient _client;
        readonly JsonSerializerOptions _serializerOptions;

        public RestService() {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        /// <summary>
        ///     GET all books from database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Book>> GetBooks() {
            HttpResponseMessage response = await _client.GetAsync("http://192.168.0.11:5001/api/books");

            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Book>>(content, _serializerOptions) ?? new List<Book>();
        }

        /// <summary>
        ///     GET a single book from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Book> GetBook(long id) {
            HttpResponseMessage response = await _client.GetAsync($"http://192.168.0.11:5001/api/books/{id}");

            string content = await response.Content.ReadAsStringAsync();
            // use null forgiving operator (!) here as we know the book exists
            return JsonSerializer.Deserialize<Book>(content, _serializerOptions)!;
        }

        /// <summary>
        ///     DELETE a single book from the database
        /// </summary>
        /// <param name="id"></param>
        public async Task<Book> DeleteBook(long id) {
            Book book = await GetBook(id);
            await _client.DeleteAsync($"http://192.168.0.11:5001/api/books/{id}");

            return book;
        }
    }
}
