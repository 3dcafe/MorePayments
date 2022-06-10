using MorePayments.Models.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MorePayments.Payment
{
    public class HttpSimpleClientRepository
    {
        private string? Login { get; set; }
        private string? Password { get; set; }
        /// <summary>
        /// Header custom
        /// </summary>
        public Dictionary<string,string> Headers = new Dictionary<string,string>();
        /// <summary>
        /// Default contructor
        /// </summary>
        public HttpSimpleClientRepository() { }
        /// <summary>
        /// Contructor with basic autrization login
        /// </summary>
        /// <param name="Login"></param>
        /// <param name="Password"></param>
        public HttpSimpleClientRepository(string Login, string Password) 
        { 
            this.Login = Login;
            this.Password = Password;
        }
        /// <summary>
        /// Get request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<RestClientResponse<T>> GetAsync<T>(string url)
        {
            using (var httpClient = new HttpClient())
            {

                if (Headers.Count > 0)
                {
                    httpClient.DefaultRequestHeaders.Clear();
                    foreach (var item in Headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                if (Login?.Length > 0 && Password?.Length > 0)
                {
                    var authenticationString = $"{Login}:{Password}";
                    var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authenticationString));
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
                }

                var response = await httpClient.GetAsync(url);
                var restClientResponse = new RestClientResponse<T>
                {
                    StatusCode = (int)response.StatusCode,
                    StatusName = response.StatusCode,
                    Message = response.ReasonPhrase,
                    Response = response.StatusCode != HttpStatusCode.OK ?
                        default :
                        await response.Content.ReadFromJsonAsync<T>()
                };
                return restClientResponse;
            }
        }
        /// <summary>
        /// Post request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="objectToPost"></param>
        /// <returns></returns>
        public async Task<RestClientResponse<T>?> PostAsync<T>(string url, object objectToPost)
        {
            using (var httpClient = new HttpClient())
            {

                if (Headers.Count > 0)
                {
                    httpClient.DefaultRequestHeaders.Clear();
                    foreach (var item in Headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                if (Login?.Length>0 && Password?.Length>0)
                {
                    var authenticationString = $"{Login}:{Password}";
                    var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authenticationString));
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
                }



                var response = await httpClient.PostAsJsonAsync(url, objectToPost);

                //var test = await response.Content.ReadFromJsonAsync<T>();

                try
                {
                    var restClientResponse = new RestClientResponse<T>
                    {
                        StatusCode = (int)response.StatusCode,
                        StatusName = response.StatusCode,
                        Message = response.ReasonPhrase,
                        Response = response.StatusCode != HttpStatusCode.OK ?
                            default :
                            await response.Content.ReadFromJsonAsync<T>()
                    };
                    return restClientResponse;
                }
                catch (Exception)
                {
                    return default;
                }
            }
        }
    }
}
