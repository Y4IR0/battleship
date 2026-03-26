using System;

public class BattleshipBoard
{
    private int _state = 0;
    public int state { get => _state; set { _state = value; OnStateChanged?.Invoke(_state); } }
    /*
     * 0 = lobby
     * 1 = placing
     * 2 = battling
     */
    
    private int _activePlayer = 0;
    public int activePlayer { get => _activePlayer; set { _activePlayer = value; OnActivePlayerChanged?.Invoke(_activePlayer); } }
    /*
     * 0 = None
     * 1 = Player 1
     * 2 = Player 2
     */
    
    private int _selectedShipPlayer1 = 0;
    public int selectedShipPlayer1 { get => _selectedShipPlayer1; set { _selectedShipPlayer1 = value; OnSelectedShipPlayer1Changed?.Invoke(_selectedShipPlayer1); } }
    /*
     * 0 = empty
     * 1 = destroyer
     * 2 = submarine
     * 3 = cruiser
     * 4 = battleship
     * 5 = carrier
     */
    
    private int _selectedShipPlayer2 = 0;
    public int selectedShipPlayer2 { get => _selectedShipPlayer2; set { _selectedShipPlayer2 = value; OnSelectedShipPlayer2Changed?.Invoke(_selectedShipPlayer2); } }
    /*
     * 0 = empty
     * 1 = destroyer
     * 2 = submarine
     * 3 = cruiser
     * 4 = battleship
     * 5 = carrier
     */
    
    private bool _player1Ready = false;
    public bool player1Ready { get => _player1Ready; set { _player1Ready = value; OnReady1Changed?.Invoke(_player1Ready); } }
    
    private bool _player2Ready = false;
    public bool player2Ready { get => _player2Ready; set { _player2Ready = value; OnReady2Changed?.Invoke(_player2Ready); } }
    
    
    private int[,] _shipsGrid1 = new int[10, 10];
    public int[,] shipsGrid1 { get => _shipsGrid1; set { _shipsGrid1 = value; OnShipsGrid1Changed?.Invoke(_shipsGrid1); } }
    /*
     * 0 = empty
     * 1 = destroyer
     * 2 = submarine
     * 3 = cruiser
     * 4 = battleship
     * 5 = carrier
     */
    
    private int[,] _shipsGrid2 = new int[10, 10];
    public int[,] shipsGrid2 { get => _shipsGrid2; set { _shipsGrid2 = value; OnShipsGrid2Changed?.Invoke(_shipsGrid2); } }
    /*
     * 0 = empty
     * 1 = destroyer
     * 2 = submarine
     * 3 = cruiser
     * 4 = battleship
     * 5 = carrier
     */
    
    private int[,] _shotsGrid1 = new int[10, 10];
    public int[,] shotsGrid1 { get => _shotsGrid1; set { _shotsGrid1 = value; OnShotsGrid1Changed?.Invoke(_shotsGrid1); } }
    
    private int[,] _shotsGrid2 = new int[10, 10];
    public int[,] shotsGrid2 { get => _shotsGrid2; set { _shotsGrid2 = value; OnShotsGrid2Changed?.Invoke(_shotsGrid2); } }
    /*
     * 0 = empty
     * 1 = miss
     * 2 = hit
     */

    private int[] shipSizes = {2, 3, 3, 4, 5};
    
    
    
    
    public static event Action OnInstanceReady;
    public event Action OnStartRound;
    public event Action<int> OnStateChanged;
    public event Action<int> OnActivePlayerChanged;
    public event Action<int> OnSelectedShipPlayer1Changed;
    public event Action<int> OnSelectedShipPlayer2Changed;
    public event Action<bool> OnReady1Changed;
    public event Action<bool> OnReady2Changed;
    public event Action<int[,]> OnShipsGrid1Changed;
    public event Action<int[,]> OnShipsGrid2Changed;
    public event Action<int[,]> OnShotsGrid1Changed;
    public event Action<int[,]> OnShotsGrid2Changed;
    
    
    
    public event Action<int, int, int> OnCellClicked;

    
    
    void Awake()
    {
        OnInstanceReady?.Invoke();
    }
    
    public void StartRound()
    {
        state = 1;
        selectedShipPlayer1 = 5;
        selectedShipPlayer2 = 5;
        OnStartRound?.Invoke();
    }

    void AttemptEnterBattleState(bool value)
    {
        if (player1Ready && player2Ready)
        {
            state = 2;
            selectedShipPlayer1 = 0;
            selectedShipPlayer2 = 0;
        }
    }

    public void TriggerCellClicked(int row, int column, int owner)
    {
        OnCellClicked?.Invoke(row, column, owner);
    }
    
    void OnCellClickedHandler(int row, int column, int player)
    {
        switch (state)
        {
            case 0: // Lobby
                break;
            case 1: // Placing
                PlaceShip(row, column, player, player == 1 ? selectedShipPlayer1 : selectedShipPlayer2);
                SelectNextShip(player);
                break;
            case 2: // Battling
                ShootCell(row, column, player);
                break;
        }
    }
    
    void SelectNextShip(int player)
    {
        if (player == 1)
            selectedShipPlayer1 = Math.Clamp(selectedShipPlayer1 - 1, 0, selectedShipPlayer1);
        else
            selectedShipPlayer2 = Math.Clamp(selectedShipPlayer2 - 1, 0, selectedShipPlayer2);
    }

    void PlaceShip(int row, int column, int player, int shipType)
    {
        if (shipType == 0) return; // Checks if a ship is selected
        
        int[,] grid = (int[,])(player == 1 ? shipsGrid1 : shipsGrid2).Clone();

        for (int i = 0; i < shipSizes[shipType - 1]; i++)
        {
            grid[row + i-1, column-1] = shipType;
        }
        
        if (player == 1)
            shipsGrid1 = grid;
        else
            shipsGrid2 = grid;
    }

    void ShootCell(int row, int column, int player)
    {
        int[,] playerGrid = player == 1 ? shipsGrid1 : shipsGrid2;
        int[,] grid = (int[,])(player == 1 ? shotsGrid1 : shotsGrid2).Clone();
        
        bool hit = false;

        if (playerGrid[row - 1, column - 1] != 0) // Checks if cell has an occupant
            hit = true; 
        
        grid[row - 1, column - 1] = !hit ? 1 : 2;
        
        if (player == 1)
            shotsGrid1 = grid;
        else
            shotsGrid2 = grid;
    }
    
    void OnEnable()
    {
        OnCellClicked += OnCellClickedHandler;
        OnReady1Changed += AttemptEnterBattleState;
        OnReady2Changed += AttemptEnterBattleState;
    }
    
    void OnDisable()
    {
        OnCellClicked -= OnCellClickedHandler;
        OnReady1Changed -= AttemptEnterBattleState;
        OnReady2Changed -= AttemptEnterBattleState;
    }
}
