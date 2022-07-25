using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Account Data Asset",menuName ="Asset/Account Data Asset")]
public class ServerAccountData : ScriptableObject
{
    public List<MyDictionary<string, Account>> accounts = new List<MyDictionary<string, Account>>();



    public MyDictionary<string, Account> GetValue(string userName)
    {
        foreach (var parameter in accounts)
        {
            if (parameter.value.userName == userName)
            {
                return parameter;
            }
        }
        return null;
    }
}
