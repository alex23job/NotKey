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

    public void ViewInitLevel(int hp, int exp, int many, int energy)
    {
        ViewEnergy(energy);
        ViewExp(exp);
        ViewHP(hp);
        ViewMany(many);
    }

    public void ViewHP(int value)
    {
        _hp.text = value.ToString();
    }

    public void ViewExp(int value)
    {
        _exp.text = value.ToString();
    }

    public void ViewMany(int value)
    {
        _many.text = value.ToString();
    }

    public void ViewEnergy(int value)
    {
        _energy.text = value.ToString();
    }

    public void ViewLevel(int value)
    {
        _numLevel.text = value.ToString();
    }

}
