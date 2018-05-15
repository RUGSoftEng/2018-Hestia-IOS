using Hestia.backend.exceptions;
using Hestia.Resources;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net;
using System.Runtime.Serialization;

namespace Hestia.backend
{
    public class NetworkHandler
    {
        private string ip;
        private int port;
        private bool hasPort;
        private RestClient client;
        private bool usesAuth0;
        private string accessToken; // auth0 access token

        public string Ip
        {
            get => ip;
            set
            {
                ip = value;
                SetRestClient();
            }
        }
        public int Port
        {
            get => port;
            set
            {
                port = value;
                SetRestClient();
            }
        }
        public bool UsesAuth0
        {
            get => usesAuth0;
        }
        public bool HasPort
        {
            get => hasPort;
        }

        public NetworkHandler(string ip)
        {
            this.ip = ip;
            this.hasPort = false;
            this.usesAuth0 = false;

            SetRestClient();
            TrustAllCerts();
        }

        public NetworkHandler(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            this.hasPort = true;
            this.usesAuth0 = false;

            SetRestClient();
            TrustAllCerts();
        }

        public NetworkHandler(string ip, string accessToken)
        {
            this.ip = ip;
            this.hasPort = false;
            this.usesAuth0 = true;
            this.accessToken = accessToken;

            SetRestClient();
            TrustAllCerts();
        }

        public NetworkHandler(string ip, int port, string accessToken)
        {
            this.ip = ip;
            this.port = port;
            this.hasPort = true;
            this.usesAuth0 = true;
            this.accessToken = accessToken;

            SetRestClient();

            TrustAllCerts();
        }

        public virtual JToken Get(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.GET);
            JToken jsonResponse = ExecuteRequest(request);

            return jsonResponse;
        }

        public virtual JToken Post(JObject payload, string endpoint)
        {
            var request = new RestRequest(endpoint, Method.POST);

            request.AddParameter("application/json; charset=utf-8", payload, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            JToken jsonResponse = ExecuteRequest(request);

            return jsonResponse;
        }
        
        public virtual JToken Delete(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.DELETE);
            JToken jsonResponse = ExecuteRequest(request);

            return jsonResponse;
        }

        public virtual JToken Put(JObject payload, string endpoint)
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

            if (usesAuth0)
            {
                request.AddParameter("Authorization", string.Format("Bearer " + accessToken), ParameterType.HttpHeader);
            }
            Console.WriteLine(request.ToString());

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
            Uri baseUrl = null;

            if (hasPort)
            {
                baseUrl = new Uri("https://" + ip + ":" + port + "/");
            } else
            {
                baseUrl = new Uri("https://" + ip + "/");
            }

            client = new RestClient(baseUrl);
        }

        private void TrustAllCerts()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        public override string ToString()
        {
            if (hasPort)
            {
                return ip + ":" + port.ToString();
            } else
            {
                return ip;
            }
        }
    }
}
