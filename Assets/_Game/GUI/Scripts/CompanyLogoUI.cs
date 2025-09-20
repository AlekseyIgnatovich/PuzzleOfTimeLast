using UnityEngine;

public class CompanyLogoUI : MonoBehaviour
{

    [SerializeField] AudioData data;
    [SerializeField] AudioSource source;
    [SerializeField] GameObject nextLogo;

    private void Start()
    {
        if (source == null)
        {
            Invoke("Kill", 3f);
        }
        else
        {
            if (!data.sfxMute)
            {
                source.volume = data.sfxVolume;
                source.Play();
            }
            Invoke("Kill", 4.5f);
        }
    }

    void Kill()
    {
        if (nextLogo != null) { nextLogo.SetActive(true); }
        GameManager.instance.StartGame();
        gameObject.SetActive(false);
    }
}
