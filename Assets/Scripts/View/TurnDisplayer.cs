using TMPro;
using UnityEngine;

public class TurnDisplayer : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] string startString = "Turn: ";
    
    void ValueChanged(int value)
    {
        if (text == null) { return; }
        
        string valueString = "";
        switch (value)
        {
            case 0:
                valueString = "None";
                break;
            case 1:
                valueString = "Player 1";
                break;
            case 2:
                valueString = "Player 2";
                break;
        }
        text.text = startString + valueString;
    }
    
    void Start()
    {
        // Default visuals
        ValueChanged(0);
    }
    
    void OnEnable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnTurnChanged += ValueChanged;
        }
        else
        {
            GameManager.OnInstanceReady += LikeAndSubscribe;
        }
        void LikeAndSubscribe()
        {
            GameManager.instance.OnTurnChanged += ValueChanged;
        }
    }
    
    void OnDisable()
    {
        GameManager.instance.OnTurnChanged -= ValueChanged;
    }
}
