using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;

namespace EstagiosTCC.Models
{
    public class FirebaseModel
    {
        private readonly string PATH_DATABASE = "https://estagios-tcc-app.firebaseio.com/";
        private readonly string PATH_STORAGE = "estagios-tcc-app.appspot.com";
        private readonly string FIREBASE_API_KEY = "AIzaSyASUkhbpXc3Hv3PeqCP0P1F01U8SXc9jbE";

        protected readonly FirebaseAuthProvider AuthProvider;
        protected readonly FirebaseClient Database;
        protected readonly FirebaseStorage Storage;

        public FirebaseModel()
        {
            AuthProvider = new FirebaseAuthProvider(new FirebaseConfig(FIREBASE_API_KEY));
            Database = new FirebaseClient(PATH_DATABASE);
            Storage = new FirebaseStorage(PATH_STORAGE);
        }
    }
}