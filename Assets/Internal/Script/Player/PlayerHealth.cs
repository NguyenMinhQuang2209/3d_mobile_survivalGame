using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : ObjectHealth
{

    [Header("UI Config")]
    [SerializeField] private TextMeshProUGUI healthTxt;
    [SerializeField] private TextMeshProUGUI energyTxt;
    [SerializeField] private TextMeshProUGUI foodTxt;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private Slider foodSlider;

    [Space(20)]
    [Header("Default config")]
    [SerializeField] private float maxMana = 100f;
    [SerializeField] private float maxFood = 100f;
    float currentMana = 0f;
    float currentFood = 0f;

    [Space(10)]
    [Header("Food config")]
    [SerializeField] private float foodReduceRate = 0.5f;

    float plusMana = 0f;
    float plusFood = 0f;
    float plusHealth = 0f;
    float plusSpeed = 0f;
    float plusDamage = 0f;

    public enum PlayerInforUI
    {
        Health,
        Mana,
        Food,
        All
    }

    private void Start()
    {
        MyInitialized();
        currentMana = maxMana;
        currentFood = maxFood;
        UpdateUI(PlayerInforUI.All);
    }
    private void UpdateUI(Slider slider, float maxValue, float minValue, float value, string txt, TextMeshProUGUI txtMesh)
    {
        slider.maxValue = maxValue;
        slider.minValue = minValue;
        slider.value = value;
        txtMesh.text = txt;
    }
    public void UpdateUI(PlayerInforUI v)
    {
        switch (v)
        {
            case PlayerInforUI.Health:
                UpdateUI(healthSlider, GetMaxHealth(), 0, GetCurrentHealth(), GetHealthTxt(), healthTxt);
                break;
            case PlayerInforUI.Mana:
                UpdateUI(manaSlider, Mathf.RoundToInt(maxMana + plusMana), 0, Mathf.RoundToInt(currentMana), Mathf.RoundToInt(currentMana) + "/" + Mathf.RoundToInt(maxMana + plusMana), energyTxt);
                break;
            case PlayerInforUI.Food:
                UpdateUI(foodSlider, Mathf.RoundToInt(maxFood + plusFood), 0, Mathf.RoundToInt(currentFood), Mathf.RoundToInt(currentFood) + "/" + Mathf.RoundToInt(maxFood + plusFood), foodTxt);
                break;
            case PlayerInforUI.All:
            default:
                UpdateUI(healthSlider, GetMaxHealth(), 0, GetCurrentHealth(), GetHealthTxt(), healthTxt);
                UpdateUI(manaSlider, Mathf.RoundToInt(maxMana + plusMana), 0, Mathf.RoundToInt(currentMana), Mathf.RoundToInt(currentMana) + "/" + Mathf.RoundToInt(maxMana + plusMana), energyTxt);
                UpdateUI(foodSlider, Mathf.RoundToInt(maxFood + plusFood), 0, Mathf.RoundToInt(currentFood), Mathf.RoundToInt(currentFood) + "/" + Mathf.RoundToInt(maxFood + plusFood), foodTxt);
                break;
        }
    }
    private void Update()
    {
        ChangePlusHealth((int)plusHealth);
        ConsumeFood();

    }
    public override bool TakeDamage(int damage, GameObject enemy)
    {
        bool v = base.TakeDamage(damage, enemy);
        UpdateUI(PlayerInforUI.Health);
        PetController.instance.PlayerWasAttacked(enemy);
        return v;
    }

    public override bool TakeDamage(int damage)
    {
        bool v = base.TakeDamage(damage);
        UpdateUI(PlayerInforUI.Health);
        return v;
    }
    private void ConsumeFood()
    {
        currentFood = Mathf.Max(0f, currentFood - Time.deltaTime * foodReduceRate);
        UpdateUI(PlayerInforUI.Food);
    }

    public float GetPlusSpeed()
    {
        return plusSpeed;
    }
    public float GetPlusDamage()
    {
        return plusDamage;
    }
}
