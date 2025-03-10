using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject towerObj;
    public Turret turret;
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
        if (LevelManager.main != null && LevelManager.main.IsGameOver())
        {
            return;
        }
        if (LevelManager.main != null && LevelManager.main.IsGameWin())
        {
            return;
        }
        if (UIManager.main.IsHoveringUI())
        {
            return;
        }

        if (towerObj != null && turret != null)
        {
            turret.OpenUpgradeUI();
            return;
        }


        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if (towerToBuild == null)
        {
            return;
        }

        if (towerToBuild.cost > LevelManager.main.currency)
        {
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);

        towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        turret = towerObj.GetComponent<Turret>();

        if (turret != null)
        {
            turret.SetParentPlot(this);
            Debug.Log("New tower placed successfully");
        }
    }

    public void ClearTurret()
    {
        Debug.Log("ClearTurret() called - clearing plot for new tower");
        towerObj = null;
        turret = null;

        // Ensure UI manager isn't in hovering state
        UIManager.main.SetHoveringState(false);
    }

}
