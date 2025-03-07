using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject tower;
    private Color startColor;
    private int defaultOrder;
    private int hoverOrder = 6; 

    private void Start()
    {
        startColor = sr.color;
        defaultOrder = sr.sortingOrder; 
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
        sr.sortingOrder = hoverOrder; 
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
        sr.sortingOrder = defaultOrder; 
    }
    private void OnMouseDown()
    {
        if (tower != null) return;

        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("You can't afford this tower");
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);

        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);

    }
}
