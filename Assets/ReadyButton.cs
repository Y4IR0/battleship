using System;
using UnityEngine;
using UnityEngine.UI;

public class ReadyButton : MonoBehaviour
{
    [SerializeField] int player;
    [SerializeField] Image image;
    [SerializeField] Color disabledColor = new Color(.8f, .2f, .2f);
    [SerializeField] Color enabledColor = new Color(0.2f, .8f, .2f);
    
    void ValueChanged(bool value)
    {
        image.color = value ? enabledColor : disabledColor;
    }

    public void OnClick()
    {
        switch (player)
        {
            case 1:
                GameManager.instance.player1Ready = !GameManager.instance.player1Ready;
                break;
            case 2:
                GameManager.instance.player2Ready = !GameManager.instance.player2Ready;
                break;
        }
    }

    void Start()
    {
        // Default visuals
        ValueChanged(false);
    }
    
    void OnEnable()
    {
        if (GameManager.instance != null)
        {
            switch (player)
            {
                case 1:
                    GameManager.instance.OnReady1Changed += ValueChanged;
                    break;
                case 2:
                    GameManager.instance.OnReady2Changed += ValueChanged;
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
                    GameManager.instance.OnReady1Changed += ValueChanged;
                    break;
                case 2:
                    GameManager.instance.OnReady2Changed += ValueChanged;
                    break;
            }
        }
    }
    
    void OnDisable()
    {
        switch (player)
        {
            case 1:
                GameManager.instance.OnReady1Changed -= ValueChanged;
                break;
            case 2:
                GameManager.instance.OnReady2Changed -= ValueChanged;
                break;
        }
    }
}
