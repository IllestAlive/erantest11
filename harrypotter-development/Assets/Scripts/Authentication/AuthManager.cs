using System;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VillageMode.Internal
{
    public class AuthManager : Instancable<AuthManager>
    {
        public FirebaseAuth _auth => FirebaseAuth.DefaultInstance;

        [SerializeField] private TMP_InputField email;
        [SerializeField] private TMP_InputField password;

        public static string EMail;

        private void Awake()
        {
            FirebaseFirestoreSettings settings = FirebaseFirestore.DefaultInstance.Settings;
            settings.PersistenceEnabled = false;
        }

        public static string DisplayName
        {
            get
            {
                int atIndex = EMail.IndexOf('@');
                return EMail.Substring(0, atIndex);
            }
        }

        public async void Login(Action callback)
        {
            await _auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    EMail = email.text;
                    callback();
                }
                else
                {
                    Debug.Log(task.Exception.InnerException.Message);

                    if (task.Exception.InnerException.Message.Equals("One or more errors occurred. (There is no user record corresponding to this identifier. The user may have been deleted.)"))
                    {
                        Debug.Log("There is no such user, creating a new one.");
                        CreateUser(callback);
                    }
                }
            });
            
        }

        public async void CreateUser(Action callback)
        {
            await _auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    EMail = email.text;
                    callback();
                }
            });
        }

        public void OnLoginRequest()
        {
            KeepLoginDetails.Instance.TrySaveLoginDetails();
            Login(() =>
            {
                print("Logged In");
                SceneManager.LoadScene("Village");
            });
        }
    }
}
