using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private KeysBoard keysBoard;
    [SerializeField] private LevelUI levelUI;
    [SerializeField] private Material blackBrood;
    [SerializeField] private Material greenBrood;

    private MeshRenderer _meshRenderer;
    private Material _oldBrood;

    private int _hp = 100;
    private int _maxhp = 100;
    private int _energy = 10;
    private int _maxEnergy = 10;
    private int _exp = 0;
    private int _many = 0;
    private int _delayViewEffects = 1;
    private int _currenViewDelay = 0;

    private void Awake()
    {
        _meshRenderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
        _oldBrood = _meshRenderer.materials[0];        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelUI.ViewInitLevel(_hp, _exp, _many, _energy);
    }

    public bool DecrementViewDelay()
    {
        if (_currenViewDelay > 0)
        {
            _currenViewDelay--;
            if (_currenViewDelay == 0) return true;
        }
        return false;
    }

    public void ChangeMaterial(int numMat)
    {
        Material[] materials = _meshRenderer.materials;
        switch(numMat)
        {
            case 0:
                materials[1] = _oldBrood; break;
            case 1:
                materials[1] = blackBrood; break;
            case 2:
                materials[1] = greenBrood; break;
        }
        _meshRenderer.materials = materials;
    }

    public void AcceptBonus(Bonus bonus)
    {
        switch(bonus.BonusType)
        {
            case 0:
                if (_hp < _maxhp)
                {
                    _hp += bonus.BonusValue;
                    if (_hp > _maxhp) _hp = _maxhp;
                }
                levelUI.ViewHP(_hp);
                break;
            case 1:
                _exp += bonus.BonusValue;
                levelUI.ViewExp(_exp);
                break;
            case 2:
                _many += bonus.BonusValue;
                levelUI.ViewMany(_many);
                break;
            case 3:
                _energy += bonus.BonusValue;
                if (_energy > _maxEnergy) _energy = _maxEnergy;
                levelUI.ViewEnergy(_energy);
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                _currenViewDelay = _delayViewEffects;
                break;
        }
    }
}
