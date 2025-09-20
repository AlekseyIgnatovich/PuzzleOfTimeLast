using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomTabController : MonoBehaviour
{
    [SerializeField] GameObject instruction;
    public void SetHeroInstruction()
    {
        instruction.SetActive(GameManager.instance.isFirstLevelUp);
    }
}
