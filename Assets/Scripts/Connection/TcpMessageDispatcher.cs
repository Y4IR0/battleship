using UnityEngine;

public class TcpMessageDispatcher : MonoBehaviour
{
    void Update()
    {
        UnityTcpClient.ProcessMessages();
    }
}