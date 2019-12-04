using EstagiosTCC.Model;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Permissions.Abstractions;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EstagiosTCC.Util
{
    public static class Location
    {
        public static async Task<Endereco> GetGPSPosition()
        {
            if (!await Permissions.GetPermission(Permission.Location))
                throw new Exception("Permissão negada!");

            if (!CrossGeolocator.Current.IsGeolocationAvailable || !CrossGeolocator.Current.IsGeolocationEnabled)
                throw new Exception("Ação não suportada!");

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 20;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

            var addresses = await locator.GetAddressesForPositionAsync(position, "RJHqIE53Onrqons5CNOx~FrDr3XhjDTyEXEjng-CRoA~Aj69MhNManYUKxo6QcwZ0wmXBtyva0zwuHB04rFYAPf7qqGJ5cHb03RCDw1jIW8l");

            if (addresses == null || addresses.Count() == 0)
                throw new Exception("Endereço não encontrado.");
            
            var address = addresses.FirstOrDefault();
            Endereco endereco = new Endereco()
            {
                Cep = address.PostalCode,
                Logradouro = address.Thoroughfare,
                Bairro = address.SubLocality,
                Localidade = address.Locality,
                Uf = address.AdminArea,
                Unidade = address.SubThoroughfare
            };

            if(endereco.Cep.Length < 8)
            {
                while (endereco.Cep.Length < 8)
                {
                    var aux = endereco.Cep + "0";
                    endereco.Cep = aux;
                }
            }
            
            var end = GetCEPPosition(endereco.Cep.Replace("-",""));
            
            if(end != null)
            {
                endereco.Localidade = end.Localidade;
                endereco.Uf = end.Uf;
                endereco.Bairro = string.IsNullOrEmpty(end.Bairro)? end.Bairro : endereco.Bairro;
            }

            return endereco;
        }

        public static Endereco GetCEPPosition(string cep)
        {
            string value = string.Join("", System.Text.RegularExpressions.Regex.Split(cep, @"[^\d]"));

            if (cep.Length != 8)
                return null;
            
            string url = string.Format("http://viacep.com.br/ws/{0}/json/", value);

            WebClient wc = new WebClient();
            string content = wc.DownloadString(url);

            Address address = JsonConvert.DeserializeObject<Address>(content);

            if (address.cep == null) 
                return null;

            return new Endereco()
            {
                Cep = address.cep,
                Logradouro = address.logradouro,
                Bairro = address.bairro,
                Localidade = address.localidade,
                Uf = address.uf,
            };
        }
    }

    internal class Address
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string unidade { get; set; }
        public string ibge { get; set; }
        public string gia { get; set; }
    }
}