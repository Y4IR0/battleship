using System;
using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int row;
    public int column;

    public int owner = 0;
    /*
     *  1 = player1
     *  2 = player2
     */
    
    private int _occupation = 0;
    public int occupation { get => _occupation; set { _occupation = value; OccupancyChanged(); } }
     /*
     * 0 = empty
     * 1 = destroyer
     * 2 = submarine
     * 3 = cruiser
     * 4 = battleship
     * 5 = carrier
     */

     private int _hit = 0;
     public int hit { get => _hit; set { _hit = value; HitChanged(); } }
    /*
     * 0 = empty
     * 1 = miss
     * 2 = hit
     */

     [SerializeField] TMP_Text text;
     [SerializeField] Color defaultColor = new  Color(.2f, .2f, .8f, 1);
     [SerializeField] Color occupancyColor = new  Color(.2f, .2f, .2f, 1);
     [SerializeField] Color missColor = new  Color(.1f, .1f, .4f, 1);
     [SerializeField] Color hitColor = new  Color(.8f, .2f, .2f, 1);

    public event Action<Cell> OnCellClicked;
    public event Action<Cell> OnCellHoverEnter;
    public event Action<Cell> OnCellHoverExit;




    void OccupancyChanged()
    {
        text.text = occupation.ToString();
        text.color = occupation != 0 ? occupancyColor : defaultColor;
    }
    
    void HitChanged()
    {
        switch (hit)
        {
            case 0:
                text.color = defaultColor;
                break;
            case 1:
                text.color = missColor;
                break;
            case 2:
                text.color = hitColor;
                break;
        }
    }
    

    public void OnClick()
    {
        OnCellClicked?.Invoke(this);
    }
    
    public void OnHoverEnter()
    {
        OnCellClicked?.Invoke(this);
    }
    
    public void OnHoverExit()
    {
        OnCellClicked?.Invoke(this);
    }
}
