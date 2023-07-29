using System;
using System.Net;
using Newtonsoft.Json;
using WooCommerceNET;

namespace LC.Crawler.BackOffice.WooCommerces;

public class CustomRestAPI : RestAPI
{
    public CustomRestAPI(string url, string key, string secret, bool authorizedHeader = true, 
        Func<string, string> jsonSerializeFilter = null, 
        Func<string, string> jsonDeserializeFilter = null, 
        Action<HttpWebRequest> requestFilter = null) : base(url, key, secret, authorizedHeader, jsonSerializeFilter, jsonDeserializeFilter, requestFilter)
    {
    }

    public override T DeserializeJSon<T>(string jsonString)
    {
        if (jsonString.Trim().StartsWith("<"))
            jsonString = jsonString.Substring(jsonString.IndexOf("{"), jsonString.Length - jsonString.IndexOf("{"));
        return JsonConvert.DeserializeObject<T>(jsonString);
    }

    public override string SerializeJSon<T>(T t)
    {
        return JsonConvert.SerializeObject(t);
    }
}