using UnityEngine;

public class HomeUI : MonoBehaviour
{

    [SerializeField] GameManager manager;

    public void CloseHome()
    {
        manager.ReturnToHome();
        gameObject.SetActive(false);
    }

}
