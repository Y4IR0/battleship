using System;
using System.Linq;
using Rug.Osc;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    
    
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
        }
    }
    public void StartConnection()
    {
        UnityTcpClient.StartClient();
    }
    public void SendMessage(string address, params object[] args)
    {
        OscMessage message = new OscMessage(address, args);
        
        UnityTcpClient.SendMessage(message);
    }

    private void Update()
    {
        UnityTcpClient.ProcessMessages();
    }
}
