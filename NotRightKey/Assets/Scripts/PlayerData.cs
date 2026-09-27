using System;
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
        PlayerProgress pp = new PlayerProgress(GameManager.Instance.currentPlayer.questStatus);
        pp.ChangeParams(GameManager.Instance.currentPlayer.totalScore);
        _hp = pp.MaxHP; _maxhp = pp.MaxHP; _energy = pp.MaxEnergy; _maxEnergy = pp.MaxEnergy;
        levelUI.ViewInitLevel(_hp, _exp, _many, _energy, _maxhp, _maxEnergy);
    }

    private void OnEnable()
    {
        EffectSignals.OnStatusApplied += AppleEffect;
        EffectSignals.OnEnemyLoss += EnemyLoss;
    }

    private void OnDisable()
    {
        EffectSignals.OnStatusApplied -= AppleEffect;
        EffectSignals.OnEnemyLoss += EnemyLoss;
    }

    private void EnemyLoss(int code)
    {
        int num = (code >> 16) & 0xff;
        if (num == 1)
        {
            _exp += 50;
            _many += 50;
        }
        else
        {
            _exp += 20;
            _many += 30;
        }
        levelUI.ViewExp(_exp);
        levelUI.ViewMany(_many);
    }

    private void AppleEffect(EffectData data)
    {
        switch(data.Type)
        {
            case EffectType.FireParticles:
                Damage(data.Value);
                ChangeMaterial(1);
                break;
            case EffectType.PoisonParticles:
                Damage(data.Value);
                ChangeMaterial(2);
                break;
            case EffectType.HealParticles:
                AddHP(data.Value);
                ChangeMaterial(0);
                break;
            case EffectType.WaterSplash:
                ChangeMaterial(0);
                break;
        }
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
        levelUI.PlayEffect(2);
        switch(bonus.BonusType)
        {
            case 0:
                if (_hp < _maxhp) AddHP(bonus.BonusValue);
                else
                {
                    GameManager.Instance.currentPlayer.inventory.AddItem(0, 1);
                    levelUI.ViewInventoryCounts(0, GameManager.Instance.currentPlayer.inventory.CountItemByID(0));
                }
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
                levelUI.ViewEnergy(_energy, _maxEnergy);
                break;
            case 4:
                GameManager.Instance.currentPlayer.inventory.AddItem(1, 1);
                levelUI.ViewInventoryCounts(1, GameManager.Instance.currentPlayer.inventory.CountItemByID(1));
                break;
            case 5:
                GameManager.Instance.currentPlayer.inventory.AddItem(2, 1);
                levelUI.ViewInventoryCounts(2, GameManager.Instance.currentPlayer.inventory.CountItemByID(2));
                break;
            case 6:
                GameManager.Instance.currentPlayer.inventory.AddItem(3, 1);
                levelUI.ViewInventoryCounts(3, GameManager.Instance.currentPlayer.inventory.CountItemByID(3));
                break;
            case 7:
                _currenViewDelay = _delayViewEffects;
                keysBoard.ViewColorEffects(true);
                break;
        }
    }

    public void AddHP(int hp)
    {
        if (_hp < _maxhp)
        {
            _hp += hp;
            if (_hp > _maxhp) _hp = _maxhp;
        }
        levelUI.ViewHP(_hp, _maxhp);
    }

    public void Damage(int dmg)
    {
        _hp -= dmg;
        if (_hp < 0) _hp = 0;
        levelUI.ViewHP(_hp, _maxhp);
        if (_hp == 0)
        {
            levelUI.PlayEffect(1);
            Invoke("GameLoss", 1f);
        }
    }

    private void GameLoss()
    {
        levelUI.ViewLossPanel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("magma"))
        {
            levelUI.Restart();
        }
        if (other.CompareTag("Finish"))
        {
            Invoke("ViewWinPanel", 2f);
        }
    }

    private void ViewWinPanel()
    {
        levelUI.ViewWinPanel(keysBoard.CurrentLevel, _exp, _many);
    }
}
