using UnityEditor;
using UnityEngine;

public class MiDataService
{
    static string accountNumber = "";
    public static string GetUserName(string useName)
    {
        var name = $"God-{useName}";
        return name;
    }
    public static void Register(string userName, string password)
    {
        if (string.Equals(userName, string.Empty) || string.Equals(password, string.Empty))
        {
            Debug.Log($"User Name Or Password Is Null      {userName}  {password}");
            return;
        }
        userName = GetUserName(userName);
        var accountdata = AssetDatabase.LoadAssetAtPath<ServerAccountData>("Assets/Service/Data/Account Data Asset.asset");
        var data = accountdata.GetValue(userName);
        if (data == null)
        {
            Account account = new Account(userName, password);
            MyDictionary<string, Account> myDictionary = new MyDictionary<string, Account>(userName, account);
            accountdata.accounts.Add(myDictionary);
        }
        else
        {
            Debug.Log($"User Name {userName} Existenced, Password Is: <color=#00FF00>{data.value.password}</color>");
        }
    }
    public static bool Login(string userName, string password)
    {
        userName = GetUserName(userName);
        var accountdata = AssetDatabase.LoadAssetAtPath<ServerAccountData>("Assets/Service/Data/Account Data Asset.asset");
        var data = accountdata.GetValue(userName);
        bool active = false;
        if (data != null)
        {
            if (data.value.password == password)
            {
                active = true;
                accountNumber = data.value.userName;
                Debug.Log("Play Game");
            }
            else
            {
                Debug.Log($"Password {password} Error     <color=#00FF00>{data.value.password}</color>");
            }
        }
        else
        {
            Debug.Log($"User Name {userName} Absent");
        }
        return active;
    }
    public static void LogOut(string userName, string password)
    {
        userName = GetUserName(userName);
        var accountdata = AssetDatabase.LoadAssetAtPath<ServerAccountData>("Assets/Service/Data/Account Data Asset.asset");
        var data = accountdata.GetValue(userName);
        if (data != null && data.value.password == password)
        {
            accountdata.accounts.Remove(data);
        }
        else
        {
            Debug.Log($"User Name {userName}  Or  Password {password} Absent");
        }
    }

    public static Account GetAccountInfoData()
    {
        var account = accountNumber;
        var accountdata = AssetDatabase.LoadAssetAtPath<ServerAccountData>("Assets/Service/Data/Account Data Asset.asset");
        var data = accountdata.GetValue(account);
        Account accountInfo = null;
        if (data != null)
        {
            accountInfo = data.value;
        }
        return accountInfo;
    }
}

