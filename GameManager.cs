using UnityEngine;


/// Script to be added to the GameManager to handle the logic of the game
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // 0 = empty cell, 1 = X, 2 = O
    private int[,] _board = new int[3, 3];

    // player 1 = X, player 2 = O
    public int CurrentPlayer { get; private set; } = 1;
    public bool IsGameOver { get; private set; } = false;

    [Header("Board Cells (assign in Inspector, row-major order)")]
    public CellController[] cells = new CellController[9]; // index = (row-1)*3 + (col-1)

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        ResetBoard();
        UIManager.Instance?.UpdateTurnUI(CurrentPlayer);
    }

    /// <summary>Called by the CellController script when the player taps it.</summary>
    public void RegisterMove(int row, int col, CellController cell)
    {
        if (IsGameOver) return;

        int r = row - 1; // convert to 0-indexed
        int c = col - 1;

        if (_board[r, c] != 0) return; // checks if the cell is already occupied 

        _board[r, c] = CurrentPlayer;
        cell.SetMark(CurrentPlayer);

        if (CheckWin(CurrentPlayer))
        {
            IsGameOver = true;
            UIManager.Instance?.ShowResult(CurrentPlayer);
            return;
        }

        if (CheckDraw())
        {
            IsGameOver = true;
            UIManager.Instance?.ShowResult(0); // 0 = draw
            return;
        }

        // Switch player turn
        CurrentPlayer = (CurrentPlayer == 1) ? 2 : 1;
        UIManager.Instance?.UpdateTurnUI(CurrentPlayer);
    }

    /// <summary>Restart the game — called by the Restart button.</summary>
    public void RestartGame()
    {
        ResetBoard();
        CurrentPlayer = 1;
        IsGameOver = false;

        foreach (var cell in cells)
            cell?.ResetCell();

        UIManager.Instance?.HideResult();
        UIManager.Instance?.UpdateTurnUI(CurrentPlayer);
    }

    

    private void ResetBoard()
    {
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++)
                _board[r, c] = 0;
    }

    private bool CheckWin(int player)
    {
        // Rows & columns
        for (int i = 0; i < 3; i++)
        {
            if (_board[i, 0] == player && _board[i, 1] == player && _board[i, 2] == player) return true;
            if (_board[0, i] == player && _board[1, i] == player && _board[2, i] == player) return true;
        }
        // Diagonals
        if (_board[0, 0] == player && _board[1, 1] == player && _board[2, 2] == player) return true;
        if (_board[0, 2] == player && _board[1, 1] == player && _board[2, 0] == player) return true;

        return false;
    }

    private bool CheckDraw()
    {
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++)
                if (_board[r, c] == 0) return false;
        return true;
    }

    /// <summary>I decided not to include a reset button for this version of the game
    /// but it could be added in future extensions.</summary>
}