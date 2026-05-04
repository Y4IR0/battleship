using System;
using UnityEngine;

public class CellController : MonoBehaviour
{
    public event Action<CellPresenter> OnCellClicked;
    public event Action<CellPresenter> OnCellHoverEnter;
    public event Action<CellPresenter> OnCellHoverExit;
    
    [SerializeField] CellPresenter cellPresenter;
    
    

    public void OnClick()
    {
        OnCellClicked?.Invoke(cellPresenter);
    }
    
    public void OnHoverEnter()
    {
        OnCellClicked?.Invoke(cellPresenter);
    }
    
    public void OnHoverExit()
    {
        OnCellClicked?.Invoke(cellPresenter);
    }
}
