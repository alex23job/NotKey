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
        int numLevel = GameManager.Instance.currentPlayer.currentLevel;
        if (numLevel < 0 || numLevel > 10) numLevel = 10;
        _levelInfo = LevelInfo.levels[numLevel];
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
