﻿using booklook_crudgui.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace booklook_crudgui.Helpers {
    public class RestService {
        readonly HttpClient _client;
        readonly JsonSerializerOptions _serializerOptions;

        public string BasePath = string.Empty;
        public string Endpoint = "/api/books";
        public string ApiUrl => BasePath + Endpoint;

        public RestService() {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            BasePath = ConfigurationManager.AppSettings["server_url"] 
                ?? throw new ApplicationException("No server url value specified");
        }

        /// <summary>
        ///     GET all books from database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Book>> GetBooks() {
            HttpResponseMessage response = await _client.GetAsync(ApiUrl);

            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Book>>(content, _serializerOptions) ?? new List<Book>();
        }

        /// <summary>
        ///     GET a single book from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Book> GetBook(long id) {
            HttpResponseMessage response = await _client.GetAsync($"{ApiUrl}/{id}");

            string content = await response.Content.ReadAsStringAsync();
            // use null forgiving operator (!) here as we know the book exists
            return JsonSerializer.Deserialize<Book>(content, _serializerOptions)!;
        }

        /// <summary>
        ///     DELETE a single book from the database
        /// </summary>
        /// <param name="id"></param>
        public async Task<HttpResponseMessage> DeleteBook(long id) {
            HttpResponseMessage response = await _client.DeleteAsync($"{ApiUrl}/{id}");

            return response;
        }

        /// <summary>
        ///     PUT a single book into the database
        /// </summary>
        /// <param name="book"></param>
        /// <param name="location"></param>
        public async Task<HttpResponseMessage> PutBook(Book book) {
            StringContent request = new StringContent(JsonSerializer.Serialize(book), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync($"{ApiUrl}/{book.Id}", request);

            return response;
        }
    }
}
