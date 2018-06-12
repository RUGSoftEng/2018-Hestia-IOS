using Hestia.Backend.Exceptions;
using Hestia.Resources;
using Hestia.Backend.Utils;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net;

namespace Hestia.Backend
{
    /// <summary>
    /// A class which handles interaction between front and back end.
    /// </summary>
    public class NetworkHandler
    {
        string address; // address including connection method and optionally a port
        RestClient client;
        readonly bool usesAuth;
        readonly string accessToken; // auth0 access token

        public string Address
        {
            get => address;
            set
            {
                address = value;
                SetRestClient();
            }
        }

        public NetworkHandler(string address)
        {
            this.address = address;
            this.usesAuth = false;

            SetRestClient();
            TrustAllCerts();
        }

        public NetworkHandler(string address, string accessToken)
        {
            this.address = address;
            this.usesAuth = true;
            this.accessToken = accessToken;

            SetRestClient();
            TrustAllCerts();
        }

        /// <summary>
        /// Performs a http GET request given an endpoint.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns>The response in JToken form</returns>
        public virtual JToken Get(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.GET);
            JToken jsonResponse = ExecuteRequest(request);

            return jsonResponse;
        }

        /// <summary>
        /// Performs a http POST request given a json payload and an endpoint.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns>The response in JToken form</returns>
        public virtual JToken Post(JObject payload, string endpoint)
        {
            var request = new RestRequest(endpoint, Method.POST);

            request.AddParameter("application/json; charset=utf-8", payload, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            JToken jsonResponse = ExecuteRequest(request);

            return jsonResponse;
        }

        /// <summary>
        /// Performs a http DELETE request given an endpoint.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns>The response in JToken form</returns>
        public virtual JToken Delete(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.DELETE);
            JToken jsonResponse = ExecuteRequest(request);

            return jsonResponse;
        }

        /// <summary>
        /// Performs a http PUT request given a json payload and an endpoint.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns>The response in JToken form</returns>
        public virtual JToken Put(JObject payload, string endpoint)
        {
            var request = new RestRequest(endpoint, Method.PUT);

            request.AddParameter("application/json; charset=utf-8", payload, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            JToken jsonResponse = ExecuteRequest(request);

            return jsonResponse;
        }

        /// <summary>
        /// This method sets a request timeout and then executes a rest request. If the response is negative it throws an error else if returns the json response.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The response in JToken form</returns>
        JToken ExecuteRequest(RestRequest request)
        {
            request.Timeout = Int32.Parse(strings.requestTimeout);

            if (usesAuth)
            {
                request.AddParameter("authorization", string.Format("Bearer " + accessToken), ParameterType.HttpHeader);
            }

            IRestResponse response = client.Execute(request);
            string responseString = response.Content;
            JToken responseJson = null;

            if (response.IsSuccessful && response.ErrorException == null)
            {
                if (JsonValidator.IsValidJson(responseString))
                {
                    responseJson = JToken.Parse(responseString);
                }
            }
            else
            {
                if (JsonValidator.IsValidJson(responseString))
                {
                    responseJson = JToken.Parse(responseString);
                    if (responseJson["message"] != null)
                    {
                        throw new ServerInteractionException(responseJson["message"].ToString());
                    }
                    if (responseJson["error"] != null)
                    {
                        throw new ServerInteractionException(responseJson["error"].ToString());
                    }
                    throw new ServerInteractionException();
                }
                throw new ServerInteractionException(response.ErrorMessage, response.ErrorException);
            }

            return responseJson;
        }

        void SetRestClient()
        {
            Uri baseUrl = null;

            baseUrl = new Uri(address + "/");

            client = new RestClient(baseUrl);
        }

        /// <summary>
        /// This method adds a certificate validation handler. Returning true will allow ignoring ssl validation errors.
        /// </summary>
        void TrustAllCerts()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        public override string ToString()
        {
            return address;
        }
    }
}
