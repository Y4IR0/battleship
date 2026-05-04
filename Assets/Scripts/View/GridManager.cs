using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] MoveMaker moveMaker;
    
    public int owner = 0;
    /*
     *  1 = player1
     *  2 = player2
     */
    
    public List<CellPresenter> cells = new List<CellPresenter>();
    [SerializeField] CellPresenter cellPresenterPrefab;

    
    
    void OnStartRound()
    {
        ClearGrid();
        cells = CreateCells();
    }
    
    void ClearGrid()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].OnCellClicked -= HandleCellClicked;
            cells[i].OnCellHoverEnter -= HandleCellHoverEnter;
            cells[i].OnCellHoverExit -= HandleCellHoverExit;
            Destroy(cells[i].gameObject);
        }
        
        cells.Clear();
    }
    
    List<CellPresenter> CreateCells()
    {
        List<CellPresenter> cells = new List<CellPresenter>();
        
        int currentRow = 1;
        int currentColumn = 1;
        
        for (int i = 0; i < 100; i++)
        {
            // Update row and column
            currentRow = 1 + (i % 10);
            currentColumn = 1 + (int)(i / 10);
            
            // Instantiate new cell
            CellPresenter newCellPresenter = Instantiate<CellPresenter>(cellPresenterPrefab, transform);
            newCellPresenter.row = currentRow;
            newCellPresenter.column = currentColumn;
            newCellPresenter.owner = owner;
            
            // Like and subscribe
            newCellPresenter.OnCellClicked += HandleCellClicked;
            newCellPresenter.OnCellHoverEnter += HandleCellHoverEnter;
            newCellPresenter.OnCellHoverExit += HandleCellHoverExit;
            
            cells.Add(newCellPresenter);
        }
        
        return cells;
    }

    void HandleShipsGridChanged(int[,] grid)
    {
        int currentRow = 1;
        int currentColumn = 1;
        
        for (int i = 0; i < 100; i++)
        {
            // Update row and column
            currentRow = (i % 10);
            currentColumn = (int)(i / 10);
            
            // Update cell
            cells[i].occupation = grid[currentRow, currentColumn];
        }
    }
    
    void HandleShotsGridChanged(int[,] grid)
    {
        int currentRow = 1;
        int currentColumn = 1;
        
        for (int i = 0; i < 100; i++)
        {
            // Update row and column
            currentRow = (i % 10);
            currentColumn = (int)(i / 10);
            
            // Update cell
            cells[i].hit = grid[currentRow, currentColumn];
        }
    }

    void HandleCellClicked(CellPresenter cellPresenter)
    {
        moveMaker.TriggerCellClicked(cellPresenter.row, cellPresenter.column, cellPresenter.owner);
    }
    
    void HandleCellHoverEnter(CellPresenter cellPresenter)
    {
        throw new NotImplementedException();
    }
    
    void HandleCellHoverExit(CellPresenter cellPresenter)
    {
        throw new NotImplementedException();
    }

    void OnEnable()
    {
        moveMaker.OnStartRound += OnStartRound;
        
        if (owner == 1)
        {
            moveMaker.OnShipsGrid1Changed += HandleShipsGridChanged;
            moveMaker.OnShotsGrid1Changed += HandleShotsGridChanged;
        }
        else
        {
            moveMaker.OnShipsGrid2Changed += HandleShipsGridChanged;
            moveMaker.OnShotsGrid2Changed += HandleShotsGridChanged;
        }
    }

    void OnDisable()
    {
        moveMaker.OnStartRound -= OnStartRound;
        
        if (owner == 1)
        {
            moveMaker.OnShipsGrid1Changed -= HandleShipsGridChanged;
            moveMaker.OnShotsGrid1Changed -= HandleShotsGridChanged;
        }
        else
        {
            moveMaker.OnShipsGrid2Changed -= HandleShipsGridChanged;
            moveMaker.OnShotsGrid2Changed -= HandleShotsGridChanged;
        }
        
        foreach (CellPresenter cell in cells)
        {
            cell.OnCellClicked -= HandleCellClicked;
            cell.OnCellHoverEnter -= HandleCellHoverEnter;
            cell.OnCellHoverExit -= HandleCellHoverExit;
        }
    }
}
