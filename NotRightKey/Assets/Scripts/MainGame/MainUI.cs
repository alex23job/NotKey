using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    [SerializeField] private Button[] _btnLevels;
    [SerializeField] private Button[] _btnBonus;
    [SerializeField] private Text _txtMany;
    [SerializeField] private Text _txtExp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LevelInfo.CreateLevels();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewLevels()
    {
        int maxLevel = GameManager.Instance.currentPlayer.maxLevel;
        for (int i = 0; i < _btnLevels.Length; i++)
        {
            _btnLevels[i].interactable = i < maxLevel;
        }
        _txtExp.text = $"Опыт : {GameManager.Instance.currentPlayer.totalScore}";
    }

    public void LoadLevel(int level)
    {
        GameManager.Instance.currentPlayer.currentLevel = level;
        SceneManager.LoadScene("LevelScene");
    }

    public void ResetProgress()
    {
        GameManager.Instance.currentPlayer.ResetProgress();
        ViewLevels();
    }

    public void ViewStore()
    {
        ViewTotalMany(GameManager.Instance.currentPlayer.totalGold);
        for (int i = 0; i < 4; i++)
        {
            ViewCountBonus(i, GameManager.Instance.currentPlayer.inventory.CountItemByID(i));
        }
    }

    public void SaleBonus(int bonus)
    {
        if (GameManager.Instance.currentPlayer.totalGold >= 50)
        {
            GameManager.Instance.currentPlayer.inventory.AddItem(bonus, 1);
            GameManager.Instance.currentPlayer.totalGold -= 50;
            ViewStore();
            GameManager.Instance.SaveGame();
        }
    }

    public void ViewTotalMany(int many)
    {
        _txtMany.text = $"Монеты : {many}";
        for (int i = 0; i < _btnBonus.Length; i++)
        {
            _btnBonus[i].interactable = many >= 50;
        }
    }

    public void ViewCountBonus(int tp, int cnt)
    {
        if (tp >= 0 && tp < _btnBonus.Length)
        {
            Text txtCount = _btnBonus[tp].transform.GetChild(0).GetComponent<Text>();
            if (txtCount != null) txtCount.text = cnt.ToString();
        }
    }

    public void ViewDebug(string msg)
    {
        Debug.Log(msg);
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
