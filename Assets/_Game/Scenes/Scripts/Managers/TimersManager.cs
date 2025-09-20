using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class TimersManager : MonoBehaviour
{
    public DateTime initialTitanDate;
    public DateTime nextTitan;



    // Devuelve el índice del array basado en los días transcurridos
    public int ReturnTitanIndex()
    {
        int _titansAmount = GameManager.instance.titansData.Titans.Length;
        if (_titansAmount <= 0)
        {
            Debug.LogError("TitansAmount debe ser mayor a 0.");
            return -1;
        }

        TimeSpan timeElapsed = DateTime.Now - initialTitanDate;

        int daysPassed = Mathf.CeilToInt((float)timeElapsed.TotalDays);
        nextTitan = initialTitanDate.AddDays(daysPassed + 1).Date;
        int index = daysPassed % _titansAmount;

        return index;
    }
}
