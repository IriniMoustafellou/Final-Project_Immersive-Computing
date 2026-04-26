using UnityEngine;
using UnityEngine.EventSystems;


public class CellController : MonoBehaviour, IPointerClickHandler
{
    [Header("Cell Position (1-indexed)")]
    [Range(1, 3)] public int row = 1;
    [Range(1, 3)] public int col = 1;

    [Header("Mark Visuals")]
    public GameObject xMark;   
    public GameObject oMark;  

    private bool _isOccupied = false;

    void Awake()
    {
        HideMarks();
    }

  //called when player clicks on an element
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isOccupied) return;
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.IsGameOver) return;

        GameManager.Instance.RegisterMove(row, col, this);
    }

    //place an X or O mark (1 = x, 2 = o)
    public void SetMark(int markType)
    {
        _isOccupied = true;

        if (markType == 1 && xMark != null) xMark.SetActive(true);
        if (markType == 2 && oMark != null) oMark.SetActive(true);
    }

    //Reset this cell to its empty state.
    public void ResetCell()
    {
        _isOccupied = false;
        HideMarks();
    }

    private void HideMarks()
    {
        if (xMark != null) xMark.SetActive(false);
        if (oMark != null) oMark.SetActive(false);
    }
}