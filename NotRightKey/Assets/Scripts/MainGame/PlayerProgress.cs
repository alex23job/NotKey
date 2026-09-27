using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerProgress
{
    private int _hp = 100;
    private int _energy = 10;

    public PlayerProgress() { }
    public PlayerProgress(int hp, int energy)
    {
        _hp = hp;
        _energy = energy;
    }

    public PlayerProgress(string csv)
    {
        string[] ar = csv.Split('#',  StringSplitOptions.RemoveEmptyEntries);
        if (ar.Length >= 2)
        {
            if (int.TryParse(ar[0], out int hp)) { _hp = hp; }
            if (int.TryParse(ar[1], out int energy)) { _energy = energy; }
        }
        else
        {
            _hp = 100;
            _energy = 10;
        }
    }

    public int MaxHP { get { return _hp; } }
    public int MaxEnergy { get { return _energy; } }

    public string ToCsvString()
    {
        char sep = '#';
        return $"{_hp}{sep}{_energy}{sep}";
    }

    public void ChangeParams(int exp)
    {
        List<int> countExp = new List<int>() { 100, 200, 400, 600, 800, 1000, 1200, 1400, 1600, 1800, 2000, 2300, 2600, 3000, 3500, 4000 };
        List<int> countHp = new List<int>() { 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 220, 240, 260, 280, 300 };
        List<int> countEnergy = new List<int>() { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 1100, 120, 130, 140, 150, 200 };
        int levelIndex = countExp.Count - 1;
        for (int i = 0; i < countExp.Count; i++)
        {
            if (exp < countExp[i])
            {
                levelIndex = i;
                break; 
            }
        }
        if (levelIndex > 0 && levelIndex < countExp.Count)
        {
            _hp = countHp[levelIndex];
            _energy = countEnergy[levelIndex];
        }
        GameManager.Instance.currentPlayer.questStatus = ToCsvString();
    }
}
