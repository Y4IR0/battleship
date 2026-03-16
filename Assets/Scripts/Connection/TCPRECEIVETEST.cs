using Rug.Osc;
using UnityEngine;
using UnityEngine.UI;

public class TCPRECEIVETEST : MonoBehaviour
{
    [SerializeField] private Image image;

    
    
    void HandleMessage(OscMessage message)
    {
        Color GetColor(int index)
        {
            switch (index)
            {
                case 0:
                    return Color.magenta;
                case 1:
                    return Color.red;
                case 2:
                    return Color.blue;
                case 3:
                    return Color.green;
                case 4:
                    return Color.yellow;
                case 5:
                    return Color.black;
            }
            
            return Color.white;
        }
        
        image.color = GetColor((int)message[0]);
        Debug.Log(message[1]);
        //image.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
    
    void OnEnable()
    {
        UnityTcpClient.OnMessageReceived += HandleMessage;
    }
    
    void OnDisable()
    {
        UnityTcpClient.OnMessageReceived -= HandleMessage;
    }
}
