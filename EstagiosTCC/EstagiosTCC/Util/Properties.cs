using Newtonsoft.Json;
using Xamarin.Forms;

namespace EstagiosTCC.Util
{
    public static class Properties
    {
        /// <summary>
        /// Salva as informações de login no properties
        /// </summary>
        public static void SaveLogin(string email, string password)
        {
            RemoveLogin();
            string emailSerial = JsonConvert.SerializeObject(email);
            string passwordSerial = JsonConvert.SerializeObject(password);
            Application.Current.Properties.Add("Email", emailSerial);
            Application.Current.Properties.Add("Password", passwordSerial);
        }

        public static void RemoveLogin()
        {
            if (Application.Current.Properties.ContainsKey("Email"))
                Application.Current.Properties.Remove("Email");

            if (Application.Current.Properties.ContainsKey("Password"))
                Application.Current.Properties.Remove("Password");
        }

        /// <summary>
        /// Retorna um vetor 0-email e 1-senha se diferente de nulo
        /// </summary>
        public static string[] GetLogin()
        {
            if (Application.Current.Properties.ContainsKey("Email")
                    && Application.Current.Properties.ContainsKey("Password"))
            {
                string email = JsonConvert.DeserializeObject<string>(
                    (string)Application.Current.Properties["Email"]);
                string password = JsonConvert.DeserializeObject<string>(
                    (string)Application.Current.Properties["Password"]);
                return new string[2] { email, password };
            }
            else
                return null;
        }
    }
}