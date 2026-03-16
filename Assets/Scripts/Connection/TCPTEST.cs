using System.Linq;
using Rug.Osc;
using UnityEngine;

public class TCPTEST : MonoBehaviour
{
    private int clickedTimes = 0;

    public void OnClick()
    {
        clickedTimes++;
        clickedTimes %= 5;
        GameManager.instance.SendMessage("/yo/mama", clickedTimes, Time.time);
        
        //OscMessage message = new OscMessage("/yo/mama");
        //message.Append(clickedTimes);
        //UnityTcpClient.SendMessage(message);
        
        //Debug.Log(message.Count);
    }
}
