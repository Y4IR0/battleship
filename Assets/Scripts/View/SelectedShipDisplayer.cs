using TMPro;
using UnityEngine;

public class SelectedShipDisplayer : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] string startString = "SelectedShip: ";
    [SerializeField] private int player = 0;
    
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
        if (GameManager.instance != null)
        {
            switch (player)
            {
                case 1:
                    GameManager.instance.OnSelectedShipPlayer1Changed += ValueChanged;
                    break;
                case 2:
                    GameManager.instance.OnSelectedShipPlayer2Changed += ValueChanged;
                    break;
            }
        }
        else
        {
            GameManager.OnInstanceReady += LikeAndSubscribe;
        }
        void LikeAndSubscribe()
        {
            switch (player)
            {
                case 1:
                    GameManager.instance.OnSelectedShipPlayer1Changed += ValueChanged;
                    break;
                case 2:
                    GameManager.instance.OnSelectedShipPlayer2Changed += ValueChanged;
                    break;
            }
        }
    }
    
    void OnDisable()
    {
        switch (player)
        {
            case 1:
                GameManager.instance.OnSelectedShipPlayer1Changed -= ValueChanged;
                break;
            case 2:
                GameManager.instance.OnSelectedShipPlayer2Changed -= ValueChanged;
                break;
        }
    }
}
