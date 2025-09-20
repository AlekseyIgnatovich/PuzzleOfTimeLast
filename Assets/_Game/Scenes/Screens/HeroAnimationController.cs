using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroAnimationController : MonoBehaviour
{
    [SerializeField] Image bgIMG;
    [SerializeField] Image heroIMG;
    [SerializeField] GameObject glow;
    [SerializeField] ParticleSystem glowVFX;
    [SerializeField] Color[] elementColors;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Image shineIMG;
    [SerializeField] TextMeshProUGUI mainHeroNameTMP;
    [SerializeField] TextMeshProUGUI mainRarityTMP;
    Coroutine animationCoroutine;

    [Header("HERO CARD ELEMENTS")]
    [SerializeField] Image heroCardIMG;
    [SerializeField] TextMeshProUGUI heroNameTMP;
    [SerializeField] TextMeshProUGUI rarityTMP;
    [SerializeField] TextMeshProUGUI maxHealthTMP;
    [SerializeField] TextMeshProUGUI attackTMP;
    [SerializeField] TextMeshProUGUI defenseTMP;
    [SerializeField] TextMeshProUGUI levelTMP;
    [SerializeField] TextMeshProUGUI descriptionTMP;
    [SerializeField] Image gemType;
    [SerializeField] Sprite[] gemSprites;
    [SerializeField] Sprite[] cardSprites;
    [SerializeField] GameObject panel;

    public HeroCard currentHero;


    private void Start()
    {
        SetInitialValues();
        //StartScreenAnimation();
        if (currentHero != null)
        {
            SetCard(currentHero, 1);
        }
    }


    private void Update()
    {
        // if (Input.GetMouseButtonDown(2))
        // {
        //     SetInitialValues();
        //     StartScreenAnimation();
        // }
    }




    public void SetInitialValues()
    {
        panel.SetActive(false);
        transform.localScale = new Vector3(1, 0, 1);
        heroIMG.transform.localScale = Vector2.zero;
        glow.transform.localScale = Vector2.zero;
        heroCardIMG.transform.localScale = Vector2.zero;
        mainHeroNameTMP.transform.localScale = Vector2.zero;
        mainRarityTMP.transform.localScale = Vector2.zero;
        canvasGroup.alpha = 0f;
    }

    public void SetCard(HeroCard _hero, int _level)
    {
        currentHero = _hero;
        heroNameTMP.text = currentHero.Name;
        rarityTMP.text = currentHero.rarity.ToString();
        maxHealthTMP.text = currentHero.maxHealth.ToString();
        attackTMP.text = currentHero.attack.ToString();
        defenseTMP.text = currentHero.defense.ToString();
        levelTMP.text = _level.ToString();
        heroIMG.sprite = currentHero.sprite;
        heroCardIMG.sprite =
        gemType.sprite = gemSprites[(int)currentHero.type];
        descriptionTMP.text = currentHero.heroClass.description.ToString();
        heroCardIMG.sprite = cardSprites[(int)currentHero.type];
        var vfxMain = glowVFX.main;
        vfxMain.startColor = elementColors[(int)currentHero.type];
        shineIMG.color = elementColors[(int)currentHero.type];
        mainRarityTMP.color = elementColors[(int)currentHero.type];
        mainRarityTMP.text = currentHero.rarity.ToString();
        mainHeroNameTMP.text = currentHero.Name.ToString();
    }

    public void StartScreenAnimation()
    {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        panel.SetActive(true);
        LeanTween.scaleY(gameObject, 1, 0.24f).setEaseOutSine();
        animationCoroutine = StartCoroutine(AnimationCoroutine());
    }

    IEnumerator AnimationCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        LeanTween.scale(glow, Vector2.one * 100f, 3f).setEaseInOutSine();
        // Tweenea el valor del alpha usando setOnUpdate
        LeanTween.value(gameObject, 0f, 1f, 2.3f)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnUpdate((float alpha) =>
                 {
                     canvasGroup.alpha = alpha;
                 });
        yield return new WaitForSeconds(2.3f);
        LeanTween.scale(heroIMG.gameObject, Vector2.one, 0.88f).setEaseOutBack();
        LeanTween.scale(mainRarityTMP.gameObject, Vector2.one, 0.37f).setEaseOutBack();
        LeanTween.scale(mainHeroNameTMP.gameObject, Vector2.one, 0.46f).setEaseOutBack();
        LeanTween.value(gameObject, 1f, 0f, 0.77f)
         .setEase(LeanTweenType.easeInOutQuad)
         .setOnUpdate((float alpha) =>
         {
             canvasGroup.alpha = alpha;
         });
        LeanTween.scale(heroIMG.gameObject, Vector2.one * 1.2f, 1f).setEaseInBack().setDelay(1f);
        yield return new WaitForSeconds(2f);
        mainHeroNameTMP.transform.localScale = Vector2.zero;
        mainRarityTMP.transform.localScale = Vector2.zero;

        LeanTween.scale(glow, Vector2.one * 200f, 0.27f).setEaseInOutSine();
        LeanTween.value(gameObject, 0f, 1f, 1f)
        .setEase(LeanTweenType.easeInOutQuad)
        .setOnUpdate((float alpha) =>
        {
            canvasGroup.alpha = alpha;
        }).setOnComplete(() =>
        {
            LeanTween.scale(heroCardIMG.gameObject, Vector2.one, 0.27f).setEaseOutBack();
            LeanTween.scale(heroIMG.gameObject, Vector2.one * 0.6f, 0.18f).setEaseInBounce();
            LeanTween.value(gameObject, 1f, 0f, 0.27f)
             .setEase(LeanTweenType.easeInOutQuad)
             .setOnUpdate((float alpha) =>
             {
                 canvasGroup.alpha = alpha;
             });
        });
    }
}
