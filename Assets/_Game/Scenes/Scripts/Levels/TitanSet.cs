using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TitanSet
{

    public MobCard titan;
    public float titanHealth;
    public float timer;
    public float timerSet;
    [Space]
    public List<string> battleLog = new List<string>();
    public List<float> battleLogDamage = new List<float>();

    public void Refresh(MobCard _titan)
    {
        Debug.Log("Refreshing Titan: " + _titan);
        if (_titan != null) { titan = _titan; }
        titanHealth = titan.maxHealth;
        timer = 0;
        battleLog = new List<string>();
        battleLogDamage = new List<float>();
    }
    public void RunTimer()
    {
        if (timer > 0) { return; }
        timer = timerSet;
    }

    public void LogAttack(string _attack, float _damage)
    {
        battleLog.Add(_attack);
        battleLogDamage.Add(_damage);
    }
    public void AddLog(List<string> _log)
    {
        for (int i = 0; i < _log.Count; i++)
        {
            battleLog.Add(_log[i]);
        }
    }
    public List<string> GetLog()
    {
        return battleLog;
    }

    public List<float> GetLogDamage()
    {
        return battleLogDamage;
    }
}

