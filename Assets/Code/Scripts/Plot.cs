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
        // Add debug to check if UI hovering state is preventing actions
        Debug.Log("Plot clicked. IsHoveringUI: " + UIManager.main.IsHoveringUI());

        if (UIManager.main.IsHoveringUI())
        {
            return;
        }

        if (towerObj != null && turret != null)
        {
            Debug.Log("Opening upgrade UI for existing turret");
            turret.OpenUpgradeUI();
            return;
        }

        Debug.Log("Attempting to add new tower");

        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if (towerToBuild == null)
        {
            Debug.LogWarning("No selected tower");
            return;
        }

        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("Can't afford tower");
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
        else
        {
            Debug.LogError("Failed to get Turret component from prefab");
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
