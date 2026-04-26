using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Canvas References")]
    public TextMeshProUGUI turnText;      // TurnText
    public GameObject resultPanel;        // ResultPanel
    public TextMeshProUGUI winText;        // WinText

    [Header("Player Labels (optional — change to your liking)")]
    public string playerXLabel = "X";
    public string playerOLabel = "O";

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        HideResult();
    }

    /// <summary>Update the player turn indicator text.</summary>
    public void UpdateTurnUI(int currentPlayer)
    {
        if (turnText == null) return;
        string label = (currentPlayer == 1) ? playerXLabel : playerOLabel;
        turnText.text = $"Player {label}'s Turn";
    }

    /// <summary>Show the result panel when the game is over (0 = draw, 1 = winner).</summary>
    public void ShowResult(int winner)
    {
        if (resultPanel != null) resultPanel.SetActive(true);

        if (winText == null) return;

        if (winner == 0)
            winText.text = "It's a Draw!";
        else
        {
            string label = (winner == 1) ? playerXLabel : playerOLabel;
            winText.text = $"Player {label} Wins!";
        }
    }

    /// <summary>Hide the result panel in the beginning and if the game restarts.</summary>
    public void HideResult()
    {
        if (resultPanel != null) resultPanel.SetActive(false);
    }
    public void OnRestartButton()
    {
        GameManager.Instance?.RestartGame();
    }
}