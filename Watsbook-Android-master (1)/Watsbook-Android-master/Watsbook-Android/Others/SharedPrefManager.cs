using System;
using Android.App;
using Android.Content;
using Newtonsoft.Json;
using Watsbook_Android.Dtos.Responses;

namespace Watsbook_Android.Others
{
    public class SharedPrefManager
    {
        private static readonly ISharedPreferences _preferences = Application
           .Context
           .GetSharedPreferences("userinfo", FileCreationMode.Private);

        private static ISharedPreferencesEditor _editor;

        public static void SaveToken(string token)
        {
            _editor = _preferences.Edit();
            _editor.PutString("token", token);
            _editor.Apply();
        }

        public static void SaveUser(UserDetailResponse user)
        {
            _editor = _preferences.Edit();
            var serialized = JsonConvert.SerializeObject(user);

            _editor.PutString("user", serialized);
            _editor.Apply();
        }

        public static string GetToken()
        {
            var token = _preferences.GetString("token", "");

            return token;
        }

        public static UserDetailResponse GetUser()
        {
            var user = JsonConvert
                .DeserializeObject<UserDetailResponse>(_preferences.GetString("user", ""));

            return user;
        }

        public static void ClearPreferences()
        {
            _editor = _preferences.Edit();
            _editor.Clear();
            _editor.Apply();
        }

    }
}
