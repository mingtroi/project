using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject towerObj;
    private MonoBehaviour turret; // Chấp nhận cả Turret và TurretSlomo
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
        if (LevelManager.main != null && (LevelManager.main.IsGameOver() || LevelManager.main.IsGameWin()))
        {
            return;
        }
        if (UIManager.main.IsHoveringUI())
        {
            return;
        }

        if (towerObj != null)
        {
            if (turret != null) // ✅ Kiểm tra trước khi gọi
            {
                (turret as Turret)?.OpenUpgradeUI();
                (turret as TurretSlomo)?.OpenUpgradeUI();
            }
            return;
        }

        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if (towerToBuild == null || towerToBuild.cost > LevelManager.main.currency)
        {
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);

        // ✅ Kiểm tra lại ngay sau khi đặt
        if (towerObj != null)
        {
            Debug.LogWarning("Tower already exists here!");
            return;
        }

        towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        turret = towerObj.GetComponent<Turret>() ?? (MonoBehaviour)towerObj.GetComponent<TurretSlomo>();

        if (turret != null)
        {
            (turret as Turret)?.SetParentPlot(this);
            Debug.Log("New tower placed successfully");
        }
        else
        {
            Debug.LogError("Turret component is missing on the placed tower!");
        }
    }

    public void ClearTurret()
    {
        Debug.Log("ClearTurret() called - clearing plot for new tower");
        towerObj = null;
        turret = null;
        UIManager.main.SetHoveringState(false);
    }
}
