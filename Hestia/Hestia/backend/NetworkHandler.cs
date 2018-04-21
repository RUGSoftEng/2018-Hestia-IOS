﻿using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net;
using System.Runtime.Serialization;

using Hestia.backend.exceptions;
using Hestia.Resources;

namespace Hestia.backend
{
    public class NetworkHandler : ISerializable
    {
        private string ip;
        private int port;
        private RestClient client;

        public string Ip
        {
            get
            {
                return ip;
            }
            set
            {
                ip = value;
                SetRestClient();
            }
        }
        public int Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
                SetRestClient();
            }
        }

        public NetworkHandler(string ip, int port)
        {
            this.ip = ip;
            this.port = port;

            SetRestClient();

            // Trust all ssl certificates
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        public void SetIpAndPort(string ip, int port)
        {
            this.ip = ip;
            this.port = port;

            SetRestClient();
        }

        public JToken Get(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.GET);
            JToken jsonResponse = ExecuteRequest(request);

            return jsonResponse;
        }

        public JToken Post(JObject payload, string endpoint)
        {
            var request = new RestRequest(endpoint, Method.POST);

            request.AddParameter("application/json; charset=utf-8", payload, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            JToken jsonResponse = ExecuteRequest(request);

            return jsonResponse;
        }
        
        public JToken Delete(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.DELETE);
            JToken jsonResponse = ExecuteRequest(request);

            return jsonResponse;
        }

        public JToken Put(JObject payload, string endpoint)
        {
            var request = new RestRequest(endpoint, Method.PUT);

            request.AddParameter("application/json; charset=utf-8", payload, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            JToken jsonResponse = ExecuteRequest(request);

            return jsonResponse;
        }

        private JToken ExecuteRequest(RestRequest request)
        {
            request.Timeout = Int32.Parse(strings.requestTimeout);

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
                    } else if (responseJson["error"] != null)
                    {
                        throw new ServerInteractionException(responseJson["error"].ToString());
                    } else
                    {
                        throw new ServerInteractionException();
                    }
                }
                else
                {
                    throw new ServerInteractionException(response.ErrorMessage, response.ErrorException);
                }
            }

            return responseJson;
        }

        private void SetRestClient()
        {
            Uri baseUrl = new Uri("https://" + ip + ":" + port + "/");
            client = new RestClient(baseUrl);
        }

        // Stub generated by serializable interface
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
