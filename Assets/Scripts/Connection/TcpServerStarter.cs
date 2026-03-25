using System.Threading;
using UnityEngine;

public class TcpServerStarter : MonoBehaviour
{
    public void StartServer(int port)
    {
        TcpServer.StartServer(port);
        Thread.Sleep(50);
        // disabled, idc rn
        //GameManager.instance.StartConnection();
    }
}
