using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using BXB.Core;
using LitJson;
using System.Threading.Tasks;
using UnityEditor;
using System.Threading;

public class Server : MiSingleton<Server>
{
    private Socket serverSocket = null;

    public async Task LinkServer()
    {
        ProjectManager.Instance.TryGetSettingAssets<ComputerSettingAsset>(ProjectManager.AssetTypes.ComputerSettingAsset, out ComputerSettingAsset asset);
        var ip = asset.ip;
        var port = asset.port;
        await AsyncDefaule();
        int nowNumber = 0;
        int linkMaxNumerOfTimes = 5;
        while (nowNumber < linkMaxNumerOfTimes)
        {
            nowNumber++;
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, socketType: SocketType.Stream, ProtocolType.Tcp);
                var iep = new IPEndPoint(IPAddress.Parse(ip), port);
                serverSocket.Connect(iep);

                Log(Color.white, $"Link server defeated - <color=#00FF00>{nowNumber}</color> / {linkMaxNumerOfTimes}");
                break;
            }
            catch (Exception exp)
            {
                Log(Color.white, $"Link server defeated - <color=#FF0000>{nowNumber}</color> / {linkMaxNumerOfTimes}");
                await Task.Delay(1000);
            }
        }
        if (nowNumber >= 5) EditorApplication.ExecuteMenuItem("Edit/Play");
    }

    public void SendData(JsonData data)
    {
        ThreadStart threadStart = new ThreadStart(() =>
        {
            try
            {
                //ProjectManager.Instance.TryGetSettingAssets<SystemStringAsset>(ProjectManager.AssetTypes.SystemStringAsset,out SystemStringAsset asset);
                //JsonData jsonData = new JsonData();
                //jsonData["name"] = "Õı“ª";
                //jsonData["age"] = 10;
                //jsonData["anchievement"] = "10/10/10/10/10/10";
                //jsonData["time"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
                byte[] jdBytes = Encoding.UTF8.GetBytes(data.ToJson());
                serverSocket.Send(jdBytes);

                jdBytes = new byte[1024];
                var length = serverSocket.Receive(jdBytes);

                var serverDataJsonText = Encoding.UTF8.GetString(jdBytes, 0, length);
                var serverData = JsonMapper.ToObject<JsonData>(new JsonReader(serverDataJsonText));
                Log(Color.cyan, serverData.ToJson());
            }
            catch (Exception exp)
            {
                Log(Color.red, exp.ToString());
            }
        });
        Thread thread = new Thread(threadStart);
        thread.Start();

        Task<int> task = new Task<int>(() =>
        {
            Log(Color.green, GameObject.Find("Main Camera").transform.position);
            return 4;
        });
        Task.WaitAll(task);
        int a = 0;
        var rets = Task.FromResult(a);
    }
}
