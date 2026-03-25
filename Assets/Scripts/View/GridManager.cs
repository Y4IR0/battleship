using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int owner = 0;
    /*
     *  1 = player1
     *  2 = player2
     */
    
    public List<Cell> cells = new List<Cell>();
    [SerializeField] Cell cellPrefab;

    
    
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
    
    List<Cell> CreateCells()
    {
        List<Cell> cells = new List<Cell>();
        
        int currentRow = 1;
        int currentColumn = 1;
        
        for (int i = 0; i < 100; i++)
        {
            // Update row and column
            currentRow = 1 + (i % 10);
            currentColumn = 1 + (int)(i / 10);
            
            // Instantiate new cell
            Cell newCell = Instantiate<Cell>(cellPrefab, transform);
            newCell.row = currentRow;
            newCell.column = currentColumn;
            newCell.owner = owner;
            
            // Like and subscribe
            newCell.OnCellClicked += HandleCellClicked;
            newCell.OnCellHoverEnter += HandleCellHoverEnter;
            newCell.OnCellHoverExit += HandleCellHoverExit;
            
            cells.Add(newCell);
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

    void HandleCellClicked(Cell cell)
    {
        GameManager.instance.TriggerCellClicked(cell.row, cell.column, cell.owner);
    }
    
    void HandleCellHoverEnter(Cell cell)
    {
        throw new NotImplementedException();
    }
    
    void HandleCellHoverExit(Cell cell)
    {
        throw new NotImplementedException();
    }

    void OnEnable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnStartRound += OnStartRound;
            if (owner == 1)
            {
                GameManager.instance.OnShipsGrid1Changed += HandleShipsGridChanged;
                GameManager.instance.OnShotsGrid1Changed += HandleShotsGridChanged;
            }
            else
            {
                GameManager.instance.OnShipsGrid2Changed += HandleShipsGridChanged;
                GameManager.instance.OnShotsGrid2Changed += HandleShotsGridChanged;
            }
        }
        else
            GameManager.OnInstanceReady += LikeAndSubscribe;
        void LikeAndSubscribe()
        {
            GameManager.instance.OnStartRound += OnStartRound;
            if (owner == 1)
            {
                GameManager.instance.OnShipsGrid1Changed += HandleShipsGridChanged;
                GameManager.instance.OnShotsGrid1Changed += HandleShotsGridChanged;
            }
            else
            {
                GameManager.instance.OnShipsGrid2Changed += HandleShipsGridChanged;
                GameManager.instance.OnShotsGrid2Changed += HandleShotsGridChanged;
            }
        }
    }

    void OnDisable()
    {
        GameManager.instance.OnStartRound -= OnStartRound;
        if (owner == 1)
        {
            GameManager.instance.OnShipsGrid1Changed -= HandleShipsGridChanged;
            GameManager.instance.OnShotsGrid1Changed -= HandleShotsGridChanged;
        }
        else
        {
            GameManager.instance.OnShipsGrid2Changed -= HandleShipsGridChanged;
            GameManager.instance.OnShotsGrid2Changed -= HandleShotsGridChanged;
        }
        
        foreach (Cell cell in cells)
        {
            cell.OnCellClicked -= HandleCellClicked;
            cell.OnCellHoverEnter -= HandleCellHoverEnter;
            cell.OnCellHoverExit -= HandleCellHoverExit;
        }
    }
}
