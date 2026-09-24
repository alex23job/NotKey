using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    private GameObject _imgCross;

    private void Awake()
    {
        _imgCross = transform.GetChild(1).gameObject;
    }

    public void ChangeEnable(bool value)
    {
        _imgCross.SetActive(value);
    }
}
