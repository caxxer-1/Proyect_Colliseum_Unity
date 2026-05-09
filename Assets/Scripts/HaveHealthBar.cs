using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HaveHealthBar : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] GameObject overhealedBar;
    [SerializeField] TMP_Text healthInfoText;
    protected float health;
    protected float maxHealth;
    void Start()
    {
        overhealedBar.SetActive(false);
    }
    public void UpdateHealthBar()
    {
        if (health > maxHealth) overhealedBar.SetActive(true);
        else overhealedBar.SetActive(false);
        healthBar.fillAmount = health/maxHealth;
        healthInfoText.text = $"{health}/{maxHealth}";
    }
}
