using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalMenuController : MonoBehaviour
{
    [SerializeField] GameObject home;
    [SerializeField] GameObject subscription;
    public MenuState state;

    public void SetState(MenuState _state)
    {
        state = _state;
        home.SetActive(state == MenuState.Home);
        subscription.SetActive(state == MenuState.Subscription);
        if (state == MenuState.None)
        {
            home.SetActive(false);
            subscription.SetActive(false);
        }
    }
}

public enum MenuState
{
    None,
    Home,
    Subscription
}
