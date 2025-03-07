using UnityEngine;

public class Turret2 : MonoBehaviour
{
    #region Datamembers

    #region Editor Settings

    [Header("Aim")]
    [SerializeField] private bool aim;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool ignoreHeight;
    [SerializeField] private Transform aimedTransform; // Single aim for turret

    [Header("Laser")]
    [SerializeField] private LineRenderer laserRenderer;
    [SerializeField] private LineRenderer laserRenderer2; // Laser for second barrel
    [SerializeField] private LayerMask laserMask;
    [SerializeField] private float laserLength;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform prefabSpawn; // First barrel spawn point
    [SerializeField] private Transform prefabSpawn2; // Second barrel spawn point

    [Header("Gizmos")]
    [SerializeField] private bool gizmo_cameraRay = false;
    [SerializeField] private bool gizmo_ground = false;
    [SerializeField] private bool gizmo_target = false;
    [SerializeField] private bool gizmo_ignoredHeightTarget = false;

    #endregion

    #region Private Fields

    private Camera mainCamera;

    #endregion

    #endregion


    #region Methods

    #region Unity Callbacks

    private void Start()
    {
        mainCamera = Camera.main;

        if (laserRenderer != null)
        {
            laserRenderer.useWorldSpace = false;
        }

        if (laserRenderer2 != null)
        {
            laserRenderer2.useWorldSpace = false;
        }
    }

    private void Update()
    {
        Aim();
        RefreshLaser();
        Shoot();
        ChangeTargetMode();
        GizmoSettings();
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
        {
            return;
        }

        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, float.MaxValue, groundMask))
        {
            if (gizmo_cameraRay)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(ray.origin, hitInfo.point);
                Gizmos.DrawWireSphere(ray.origin, 0.5f);
            }

            var hitPosition = hitInfo.point;
            var hitGroundHeight = Vector3.Scale(hitInfo.point, new Vector3(1, 0, 1)); ;
            var hitPositionIngoredHeight = new Vector3(hitInfo.point.x, aimedTransform.position.y, hitInfo.point.z);

            if (gizmo_ground)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawWireSphere(hitGroundHeight, 0.5f);
                Gizmos.DrawLine(hitGroundHeight, hitPosition);
            }

            if (gizmo_target)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(hitInfo.point, 0.5f);
                Gizmos.DrawLine(aimedTransform.position, hitPosition);
            }

            if (gizmo_ignoredHeightTarget)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(hitPositionIngoredHeight, 0.5f);
                Gizmos.DrawLine(aimedTransform.position, hitPositionIngoredHeight);
            }
        }
    }

    #endregion

    private void Aim()
    {
        if (!aim) return;

        var (success, position) = GetMousePosition();
        if (success)
        {
            var direction = position - aimedTransform.position;

            if (ignoreHeight)
            {
                direction.y = 0;
            }

            // Apply 90-degree offset for correct turret rotation
            aimedTransform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0);
        }
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            return (true, hitInfo.point);
        }
        else
        {
            return (false, Vector3.zero);
        }
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // First barrel shooting
            var projectile1 = Instantiate(projectilePrefab, prefabSpawn.position, Quaternion.identity);
            projectile1.transform.forward = prefabSpawn.forward;

            // Second barrel shooting
            var projectile2 = Instantiate(projectilePrefab, prefabSpawn2.position, Quaternion.identity);
            projectile2.transform.forward = prefabSpawn2.forward;
        }
    }

    private void RefreshLaser()
    {
        if (laserRenderer == null || laserRenderer2 == null)
        {
            return;
        }

        // Apply 90-degree offset for both lasers
        Vector3 laserDirection = Quaternion.Euler(0, 90, 0) * aimedTransform.forward;

        // First laser (from prefabSpawn)
        Vector3 lineEnd1;
        if (Physics.Raycast(prefabSpawn.position, laserDirection, out var hitinfo1, laserLength, laserMask))
        {
            lineEnd1 = hitinfo1.point;
        }
        else
        {
            lineEnd1 = prefabSpawn.position + laserDirection * laserLength;
        }

        // Second laser (from prefabSpawn2)
        Vector3 lineEnd2;
        if (Physics.Raycast(prefabSpawn2.position, laserDirection, out var hitinfo2, laserLength, laserMask))
        {
            lineEnd2 = hitinfo2.point;
        }
        else
        {
            lineEnd2 = prefabSpawn2.position + laserDirection * laserLength;
        }

        // Set laser positions for both barrels
        laserRenderer.SetPosition(0, laserRenderer.transform.InverseTransformPoint(prefabSpawn.position));
        laserRenderer.SetPosition(1, laserRenderer.transform.InverseTransformPoint(lineEnd1));

        laserRenderer2.SetPosition(0, laserRenderer2.transform.InverseTransformPoint(prefabSpawn2.position));
        laserRenderer2.SetPosition(1, laserRenderer2.transform.InverseTransformPoint(lineEnd2));
    }

    private void ChangeTargetMode()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ignoreHeight = !ignoreHeight;
        }
    }

    private void GizmoSettings()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gizmo_cameraRay = !gizmo_cameraRay;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gizmo_ground = !gizmo_ground;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gizmo_target = !gizmo_target;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            gizmo_ignoredHeightTarget = !gizmo_ignoredHeightTarget;
        }
    }

    #endregion
}
