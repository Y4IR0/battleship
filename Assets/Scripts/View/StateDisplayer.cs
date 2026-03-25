using TMPro;
using UnityEngine;

public class StateDisplayer : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] string startString = "State: ";
    
    void ValueChanged(int value)
    {
        if (text == null) { return; }
        
        string valueString = "";
        switch (value)
        {
            case 0:
                valueString = "Lobby";
                break;
            case 1:
                valueString = "Placing";
                break;
            case 2:
                valueString = "Battling";
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
            GameManager.instance.OnStateChanged += ValueChanged;
        }
        else
        {
            GameManager.OnInstanceReady += LikeAndSubscribe;
        }
        void LikeAndSubscribe()
        {
            GameManager.instance.OnStateChanged += ValueChanged;
        }
    }
    
    void OnDisable()
    {
        GameManager.instance.OnStateChanged -= ValueChanged;
    }
}
