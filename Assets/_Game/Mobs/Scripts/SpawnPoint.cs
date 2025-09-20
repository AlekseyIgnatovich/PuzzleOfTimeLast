using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnPoint : MonoBehaviour
{

    public GameplayManager manager;
    [SerializeField] Transform center;
    public SpriteRenderer sprite;
    public int[] columns;
    public FieldSide fieldSide;
    [Space]
    [SerializeField] Color[] colors;
    [Space]
    public ElementType type;
    public Resistance[] resistance;
    [Space]
    [SerializeField] GameObject targetReticle;
    [SerializeField] GameObject textNoHero;
    [SerializeField] MobHealthbar healthObject;
    [SerializeField] GameObject effectSpecialSlash;
    [Space]

    Mob mob;

    float timerNoHero;

    public Mob MyMob
    {
        get { return mob; }
        set
        {
            mob = value;
            if (mob == null) { UpdateTargetReticle(false); }
        }
    }

    private void Start()
    {
        UpdatePosition();
        UpdateCounter(0);
        UpdateHealth(0, 0);
        UpdateTargetReticle(false);
    }

    private void Update()
    {
        if (timerNoHero > 0)
        {
            timerNoHero -= Time.deltaTime;
            if (timerNoHero <= 0)
            {
                textNoHero.SetActive(false);
            }
        }
    }

    public void UpdateCounter(int _count)
    {
        healthObject.UpdateCounter(_count);
    }
    public void UpdateHealth(float min, float max)
    {
        healthObject.gameObject.SetActive(min > 0);
        healthObject.UpdateHealth(min, max);
    }
    public void UpdateColor()
    {
        sprite.color = colors[(int)type];
    }
    public void UpdatePosition()
    {
        healthObject.gameObject.SetActive(true);
        healthObject.transform.position = transform.position + (Vector3.down * .2f);
    }

    public void UpdateTargetReticle(bool _set)
    {
        if (MyMob == null) { targetReticle.SetActive(false); return; }
        targetReticle.SetActive(_set);
    }

    public void SetNoHero(float _timer = 1f)
    {
        textNoHero.GetComponent<TextMeshProUGUI>().color = colors[0];
        textNoHero.SetActive(true);
        timerNoHero = _timer;
    }

    public bool HasColumn(int _column)
    {
        for (int i = 0; i < columns.Length; i++)
        {
            if (columns[i] == _column)
            {
                return true;
            }
        }
        return false;
    }

    public void SpawnSpecialSlashEffect()
    {
        center.SetAsLastSibling();
        Instantiate(effectSpecialSlash, center);
    }
}
