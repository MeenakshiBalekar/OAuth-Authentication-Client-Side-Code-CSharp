﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ClientSideApplicationForOAuth.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var secret = "<Enter your secret here>";
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(
              HttpMethod.Post,
              $" https://directline.botframework.com/v3/directline/tokens/generate");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", secret);

            var userId = $"dl_{Guid.NewGuid()}";

            request.Content = new StringContent(
            JsonConvert.SerializeObject(
                new { User = new { Id = userId } }),
                Encoding.UTF8,
                "application/json");


            var response = await client.SendAsync(request);

            string token = String.Empty;
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<DirectLineToken>(body).token;
            }
            var config = new ChatConfig()
            {
                Token = token,
                UserId = userId //Guid.NewGuid().ToString()
            };
            return View(config);
        }
    }
    public class DirectLineToken
    {
        public string conversationId { get; set; }
        public string token { get; set; }
        public int expires_in { get; set; }
    }
    public class ChatConfig
    {
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}