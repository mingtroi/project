using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI currencyUI;
    [SerializeField] private Animator anim;

    private bool isMenuOpen = true;

    private void Start()
    {
        UpdateCurrencyUI(); 
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("MenuOpen", isMenuOpen);
    }

    public void UpdateCurrencyUI()
    {
        if (currencyUI != null)
        {
            currencyUI.text = LevelManager.main.currency.ToString();
        }
    }
}
