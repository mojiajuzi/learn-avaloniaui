using System;
using System.IO;

namespace MyMoney.Services;

public class UserDataHandlerServices
{
    public static string GetUserUploadsFolderPath()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        string userUploadsFolderPath = Path.Combine(appDataPath, "MyMoney", "Uploads");
        if (!Directory.Exists(userUploadsFolderPath))
        {
            Directory.CreateDirectory(userUploadsFolderPath);
        }

        return userUploadsFolderPath;
    }
}