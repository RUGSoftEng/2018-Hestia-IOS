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
        string address; // address including connection method
        int port;
        bool hasPort;
        RestClient client;
        bool usesAuth;
        string accessToken; // auth0 access token

        public string Address
        {
            get => address;
            set
            {
                address = value;
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
        public bool UsesAuth
        {
            get => usesAuth;
        }
        public bool HasPort
        {
            get => hasPort;
        }

        public NetworkHandler(string address)
        {
            this.address = address;
            this.hasPort = false;
            this.usesAuth = false;

            SetRestClient();
            TrustAllCerts();
        }

        public NetworkHandler(string address, int port)
        {
            this.address = address;
            this.port = port;
            this.hasPort = true;
            this.usesAuth = false;

            SetRestClient();
            TrustAllCerts();
        }

        public NetworkHandler(string address, string accessToken)
        {
            this.address = address;
            this.hasPort = false;
            this.usesAuth = true;
            this.accessToken = accessToken;

            SetRestClient();
            TrustAllCerts();
        }

        public NetworkHandler(string address, int port, string accessToken)
        {
            this.address = address;
            this.port = port;
            this.hasPort = true;
            this.usesAuth = true;
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

            if (hasPort)
            {
                baseUrl = new Uri(address + ":" + port + "/");
            }
            else
            {
                baseUrl = new Uri(address + "/");
            }

            client = new RestClient(baseUrl);
        }

        void TrustAllCerts()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        public override string ToString()
        {
            if (hasPort)
            {
                return address + ":" + port.ToString();
            }
            return address;
        }
    }
}
