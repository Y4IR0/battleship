using TMPro;
using UnityEngine;

public class SelectedShipDisplayer : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] string startString = "SelectedShip: ";
    [SerializeField] private int player = 0;
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
                valueString = "Carrier";
                break;
            case 2:
                valueString = "Battleship";
                break;
            case 3:
                valueString = "Cruiser";
                break;
            case 4:
                valueString = "Submarine";
                break;
            case 5:
                valueString = "Destroyer";
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
        if (player == 1)
            client.OnSelectedShipPlayer1Changed += ValueChanged;
        else
            client.OnSelectedShipPlayer2Changed += ValueChanged;
    }
    
    void OnDisable()
    {
        if (player == 1)
            client.OnSelectedShipPlayer1Changed -= ValueChanged;
        else
            client.OnSelectedShipPlayer2Changed -= ValueChanged;
    }
}
