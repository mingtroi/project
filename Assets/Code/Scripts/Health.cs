using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Attribute")]
    [SerializeField] private int hitPoints = 3;
    [SerializeField] private int CurrencyWorth = 60;
    [SerializeField] private Image hpBar;

    private int maxHealth;
    private bool isDestroyed = false;

    void Start()
    {
        maxHealth = hitPoints; 
        UpdateHealthBar();
    }

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;
        UpdateHealthBar(); 

        if (hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(CurrencyWorth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

    private void UpdateHealthBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = (float)hitPoints / maxHealth; 
        }
    }
}
