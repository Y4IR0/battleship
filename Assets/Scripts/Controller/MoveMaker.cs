using System;
using UnityEngine;

public class MoveMaker : MonoBehaviour
{
    public event Action<int, int, int> OnCellClicked;
    
    public void TriggerCellClicked(int row, int column, int owner)
    {
        OnCellClicked?.Invoke(row, column, owner);
    }
}
