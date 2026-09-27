using UnityEngine;
using System.Collections.Generic;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private LevelUI levelUI;
    [SerializeField] private KeysBoard keysBoard;

    private LevelInfo _levelInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("InitLevel", 0.2f);
        levelUI.ViewHint("Доведите колобка до портала. Управление кликами по кнопкам справа снизу или этими же клавишами клавиатуры.", true);
        Invoke("HideHint", 15f);
    }

    private void HideHint()
    {
        levelUI.ViewHint("", false);
    }

    private void InitLevel()
    {
        _levelInfo = new LevelInfo(1, 212, 100, 50, new List<int> { 3, 8, 106, 208, 304, 309}, new List<int> { 2, 3, 4, 5, 6, 7, 8, 9, 101, 102, 103, 104, 105, 201, 202, 203, 207, 209, 301, 302, 305, 308, 401, 402, 405}, new List<int> { 210 });
        levelUI.ViewLevel(_levelInfo.Number);
        keysBoard.SetLevelInfo(_levelInfo);
        ViewInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewInventory()
    {
        for (int i = 0; i < 4; i++) 
        {
            levelUI.ViewInventoryCounts(i, GameManager.Instance.currentPlayer.inventory.CountItemByID(i));
        }
    }
}
