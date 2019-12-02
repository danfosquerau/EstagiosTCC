using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using System.Threading.Tasks;

namespace EstagiosTCC.Util
{
    public static class ConnectionDB
    {
        private static readonly string PATH_DATABASE = "https://estagios-tcc-app.firebaseio.com/";
        private static readonly string PATH_STORAGE = "estagios-tcc-app.appspot.com";
        private static readonly string FIREBASE_API_KEY = "AIzaSyASUkhbpXc3Hv3PeqCP0P1F01U8SXc9jbE";

        /// <summary>
        /// Serviço de Autenticação do Firebase
        /// </summary>
        public static FirebaseAuthProvider Authentication;
        /// <summary>
        /// Serviço do Danco de dados do Firebase
        /// </summary>
        public static FirebaseClient Database;
        /// <summary>
        /// Serviço do Storage do Firebase
        /// </summary>
        public static FirebaseStorage Storage;

        /// <summary>
        /// Inicializar o serviço conexão para Autenticação Firebase
        /// </summary>
        public static void InitializeService()
        {
            Authentication = new FirebaseAuthProvider(new FirebaseConfig(FIREBASE_API_KEY));
            Database = null;
            Storage = null;
        }

        /// <summary>
        /// Inicializar o serviço de conexão com o Danco de dados e Storage do Firebase
        /// Executar somente após a Autenticação do usuário, para evitar erro
        /// </summary>
        public static void InitializeData(string firebaseToken)
        {
            Database = new FirebaseClient(PATH_DATABASE, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(firebaseToken)
            });
            Storage = new FirebaseStorage(PATH_STORAGE, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(firebaseToken)
            });
        }

        public static void Logout()
        {
            Authentication.Dispose();

            InitializeService();
        }
    }
}