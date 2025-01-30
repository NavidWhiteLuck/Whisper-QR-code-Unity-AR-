//using UnityEngine;
//using Firebase.Auth;
//using PlayFab;
//using PlayFab.ClientModels;
//using Google;
//using System.Threading.Tasks;

//public class GoogleLoginManager : MonoBehaviour
//{
//    private FirebaseAuth auth;
//    private GoogleSignInConfiguration configuration;

//    void Start()
//    {
//        auth = FirebaseAuth.DefaultInstance;
//        configuration = new GoogleSignInConfiguration
//        {
//            WebClientId = "WEB_CLIENT_ID", // bayad tabdil be tanzim dasti konam
//            RequestEmail = true,
//            RequestIdToken = true
//        };
//    }

//    public void SignInWithGoogle()
//    {
//        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleSignIn);
//    }

//    private void OnGoogleSignIn(Task<GoogleSignInUser> task)
//    {
//        if (task.IsFaulted || task.IsCanceled)
//        {
//            Debug.LogError("Google Sign-In Failed!");
//            return;
//        }

//        GoogleSignInUser googleUser = task.Result;
//        FirebaseCredential credential = GoogleAuthProvider.GetCredential(googleUser.IdToken, null);

//        auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
//        {
//            if (authTask.IsCompleted && !authTask.IsFaulted)
//            {
//                FirebaseUser user = authTask.Result;
//                RegisterWithPlayFab(user);
//            }
//            else
//            {
//                Debug.LogError("Firebase Authentication Failed!");
//            }
//        });
//    }

//    private void RegisterWithPlayFab(FirebaseUser user)
//    {
//        var request = new LoginWithGoogleAccountRequest
//        {
//            TitleId = PlayFabSettings.TitleId,
//            AccessToken = user.IdToken,
//            CreateAccount = true
//        };

//        PlayFabClientAPI.LoginWithGoogleAccount(request, result =>
//        {
//            Debug.Log("PlayFab Login Success: " + result.PlayFabId);
//            UpdateDisplayName(user.DisplayName);
//        },
//        error => Debug.LogError("PlayFab Login Failed: " + error.ErrorMessage));
//    }

//    private void UpdateDisplayName(string displayName)
//    {
//        var request = new UpdateUserTitleDisplayNameRequest
//        {
//            DisplayName = displayName
//        };

//        PlayFabClientAPI.UpdateUserTitleDisplayName(request, result =>
//        {
//            Debug.Log("Display Name Updated: " + result.DisplayName);
//        },
//        error => Debug.LogError("Failed to update display name: " + error.ErrorMessage));
//    }
//}
