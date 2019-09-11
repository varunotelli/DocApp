using DocApp.Domain.Callbacks;
using DocApp.Domain.DataContracts;
using DocApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Data
{
    public class LocationProxy : ILocationList
    {
        public async Task GetLocationsAsync(string val, ILocationCallBack addressCallBack)
        {
            string url = string.Format("https://www.practo.com/cerebro/v3/autocomplete?query={0}&indexes=%5B%22city%22%2C%22locality%22%5D", val);
            var http = new HttpClient();
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine(result);
            var serializer = new DataContractJsonSerializer(typeof(RootLocationObject));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootLocationObject)serializer.ReadObject(ms);
            System.Diagnostics.Debug.WriteLine("data="+data.results.@default.matches[0].suggestion);
            if (data != null)
                addressCallBack.PractoReadSuccess(data);
            else
                addressCallBack.PractoReadFail();

        }
    }
}
