using System.Collections.Generic;

[System.Serializable]
public class Account
{
    public string userName;
    public string password;

    public AccountInformation attribute;

    public List<MyDictionary<ulong, ulong>> acticles = new List<MyDictionary<ulong, ulong>>();

    public List<ulong> unlockLevel = new List<ulong>();





    [System.Serializable]
    public struct AccountInformation
    {
        public string name;
        public ulong grade;
        public float ATK;
        public float DEF;
        public float crit;
        public float mvoeSpeed;
        public float jumpHeight;
    }

    private Account()
    {

    }
    public Account(string userName, string password)
    {
        this.userName = userName;
        this.password = password;
    }
}