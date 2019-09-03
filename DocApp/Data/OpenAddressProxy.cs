using DocApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using DocApp.Domain.DataContracts;
using DocApp.Domain.Callbacks;

namespace DocApp.Data
{
    public class OpenAddressProxy: IGetAddress
    {
        public async Task GetAddressAsync(double lat, double lon, IAddressCallBack addressCallBack)
        {

            string url = string.Format("https://nominatim.openstreetmap.org/reverse?email=vasur2201@gmail.com&format=json&lat={0}&lon={1}&addressdetails=1", lat, lon);
            var http = new HttpClient();
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine(result);
            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(ms);
            System.Diagnostics.Debug.WriteLine(data.address.neighbourhood);
            if (data != null)
                addressCallBack.ReadFromAPISuccess(data);
            else
                addressCallBack.ReadFromAPIFail();
         
        }
    }
}
