using UnityEngine;
using UnityEngine.UI;

public class GrayscaleEffectUI : MonoBehaviour
{
    [SerializeField] Material grayscaleMaterial;
    [SerializeField] CanvasGroup hurtImg;
    LTDescr glowTween;
    Image uiImage;
    Material tempMaterial;
    bool isHurt = false;

    private void Start()
    {
        uiImage = GetComponent<Image>();

        // if (uiImage != null)
        // {
        //     uiImage.material = grayscaleMaterial;
        // }
        hurtImg.alpha = 0;

        tempMaterial = Instantiate(uiImage.material);
        uiImage.material = tempMaterial;
        tempMaterial.SetFloat("_GrayscaleAmount", 0);
    }

    public void ApplyGrayscale()
    {
        Debug.Log("Applying grey scale");
        isHurt = true;
        LeanTween.value(gameObject, 0f, 1f, 2f).setOnUpdate(UpdateGrayscale)
                 .setEase(LeanTweenType.easeInOutSine);
    }

    public void RestoreColor()
    {
        Debug.Log("Restoring Color");
        isHurt = false;
        LeanTween.value(gameObject, 1f, 0f, 2f).setOnUpdate(UpdateGrayscale)
                 .setEase(LeanTweenType.easeInOutSine);
    }

    void UpdateGrayscale(float grayscaleValue)
    {
        if (tempMaterial == null) return;

        // Cambiar el valor de escala de grises en el material instanciado
        tempMaterial.SetFloat("_GrayscaleAmount", grayscaleValue);
    }
    public void StartHurtGlowEffect()
    {
        if (hurtImg != null)
        {
            // Crear el efecto de parpadeo en bucle
            glowTween = LeanTween.alphaCanvas(hurtImg, 1f, 0.5f) // Subir alpha a 1 en 0.5 segundos
                        .setEase(LeanTweenType.easeInOutSine)
                        .setLoopPingPong(); // Efecto de ida y vuelta
        }
    }

    public void StopHurtGlowEffect()
    {
        if (glowTween != null)
        {
            LeanTween.cancel(hurtImg.gameObject, glowTween.id); // Cancelar el tween activo
            hurtImg.alpha = 0f; // Restablecer el alpha a 0
        }
    }
}