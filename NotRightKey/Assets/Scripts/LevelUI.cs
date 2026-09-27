using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private Button[] _buttons;
    [SerializeField] private Text _hp;
    [SerializeField] private Text _exp;
    [SerializeField] private Text _many;
    [SerializeField] private Text _energy;
    [SerializeField] private Text _numLevel;
    [SerializeField] private Image _imgHp;
    [SerializeField] private Image _imgEnergy;

    [SerializeField] private GameObject _lossPanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private Text[] _winTextLines;

    [SerializeField] private Text[] _txtCounts;
    [SerializeField] private Text _txtHint;
    [SerializeField] private GameObject _hintPanel;
    [SerializeField] private GameObject _keyPanel;

    private List<ButtonControl> _btnControls = new List<ButtonControl> ();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            ButtonControl buttonControl = _buttons[i].gameObject.GetComponent<ButtonControl>();
            if (buttonControl != null)
            {
                _btnControls.Add(buttonControl);
                buttonControl.ChangeEnable(false);
            }
        }
        ViewInventoryCounts(-1, 0);
        ViewKeyPanel(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ArrowClick(int index)
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _btnControls[i].ChangeEnable(i == index);
        }
    }

    public void ViewInitLevel(int hp, int exp, int many, int energy, int maxHp, int maxEnergy)
    {
        ViewEnergy(energy, maxEnergy);
        ViewExp(exp);
        ViewHP(hp, maxHp);
        ViewMany(many);
    }

    public void ViewHint(string hint, bool val)
    {
        _txtHint.text = hint;
        _hintPanel.SetActive(val);
    }

    public void ViewKeyPanel(bool val)
    {
        _keyPanel.SetActive(val);
    }

    public void ViewHP(int value, int maxHp)
    {
        _hp.text = value.ToString();
        _imgHp.fillAmount = (float)value / (float)maxHp;
    }

    public void ViewExp(int value)
    {
        _exp.text = value.ToString();
    }

    public void ViewMany(int value)
    {
        _many.text = value.ToString();
    }

    public void ViewEnergy(int value, int maxEnergy)
    {
        _energy.text = value.ToString();
        _imgEnergy.fillAmount = (float)value / (float)maxEnergy;
    }

    public void ViewLevel(int value)
    {
        _numLevel.text = value.ToString();
    }

    public void ViewLossPanel()
    {
        _lossPanel.SetActive(true);
    }

    public void ViewWinPanel(LevelInfo info, int exp, int many)
    {
        _winTextLines[0].text = $"{info.Number} уровень\r\n пройден !!!";
        _winTextLines[1].text = $"Опыт : {info.LevelExp} + {exp}";
        _winTextLines[2].text = $"Монеты : {info.LevelMany} + {many}";
        _winTextLines[3].text = "";
        _winPanel.SetActive(true);
    }

    public void ViewInventoryCounts(int tp, int val)
    {
        switch(tp)
        {
            case 0: _txtCounts[tp].text = val.ToString(); break;
            case 1: _txtCounts[tp].text = val.ToString(); break;
            case 2: _txtCounts[tp].text = val.ToString(); break;
            case 3: _txtCounts[tp].text = val.ToString(); break;
            case -1: _txtCounts[0].text = "0"; _txtCounts[1].text = "0"; _txtCounts[2].text = "0"; _txtCounts[3].text = "0"; break;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainScene");
    }
}
