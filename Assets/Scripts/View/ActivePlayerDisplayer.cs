using TMPro;
using UnityEngine;

public class TurnDisplayer : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] string startString = "Active Player: ";
    [SerializeField] Client client;
    
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
        client.OnActivePlayerChanged += ValueChanged;
    }
    
    void OnDisable()
    {
        client.OnActivePlayerChanged -= ValueChanged;
    }
}
