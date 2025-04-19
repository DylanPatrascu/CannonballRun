using System.Collections;
using UnityEngine;

public enum GunState {Idle, Firing, Reloading}

public class Turret : MonoBehaviour
{
    [Header("Aim")]
    [SerializeField] private bool aim;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool ignoreHeight;
    [SerializeField] private Transform aimedTransform;

    [Header("Laser")]
    [SerializeField] private LineRenderer laserRenderer;
    [SerializeField] private LayerMask laserMask;
    [SerializeField] private float laserLength;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform prefabSpawn;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int currentAmmo;
    [SerializeField] private GunState gun = GunState.Idle;
    [SerializeField] private float shootDelay = 0.5f;
    [SerializeField] private float reloadTime = 2.0f;


    [Header("Gizmos")]
    [SerializeField] private bool gizmo_cameraRay = false;
    [SerializeField] private bool gizmo_ground = false;
    [SerializeField] private bool gizmo_target = false;
    [SerializeField] private bool gizmo_ignoredHeightTarget = false;


    private Camera mainCamera;


    private void Start()
    {
        gun = GunState.Idle;
        currentAmmo = maxAmmo;
        mainCamera = Camera.main;

        if (laserRenderer != null)
        {
            laserRenderer.useWorldSpace = false;
        }
    }

    private void Update()
    {
        Aim();
        RefreshLaser();
        ChangeTargetMode();
        GizmoSettings();
        Debug.Log(currentAmmo);
        
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && gun == GunState.Idle)
        {
            StartCoroutine(Shoot());

        }
        if (Input.GetKeyDown(KeyCode.R) && gun == GunState.Idle)
        {
            StartCoroutine(Reload());
        }
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

    private void Aim()
    {
        if (aim == false)
        {
            return;
        }

        var (success, position) = GetMousePosition();
        if (success)
        {
            // Direction is usually normalized, 
            // but it does not matter in this case.
            var direction = position - aimedTransform.position;

            if (ignoreHeight)
            {
                // Ignore the height difference.
                direction.y = 0;
            }

            // Make the transform look at the mouse position.
            aimedTransform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0);

        }
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Try to hit something with your ground mask.
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            return (true, hitInfo.point);
        }
        else
        {
            // Create a horizontal plane at the turret's (aimedTransform's) height.
            Plane horizontalPlane = new Plane(Vector3.up, new Vector3(0, aimedTransform.position.y, 0));
            if (horizontalPlane.Raycast(ray, out float distance))
            {
                // Get the point along the ray that intersects the plane.
                return (true, ray.GetPoint(distance));
            }
            else
            {
                return (false, Vector3.zero);
            }
        }
    }

    private IEnumerator Shoot()
    {
        gun = GunState.Firing;

        var projectile = Instantiate(projectilePrefab, prefabSpawn.position, Quaternion.identity);
        projectile.transform.forward = aimedTransform.forward;
        currentAmmo -= 1;

        yield return new WaitForSeconds(shootDelay);

        gun = GunState.Idle;
    }

    private IEnumerator Reload()
    {

        gun = GunState.Reloading;

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        gun = GunState.Idle;

    }

    private void RefreshLaser()
    {
        if (laserRenderer == null)
        {
            return;
        }

        Vector3 laserDirection = Quaternion.Euler(0, 90, 0) * aimedTransform.forward; // Apply -90 degree offset
        Vector3 lineEnd;

        if (Physics.Raycast(prefabSpawn.position, laserDirection, out var hitinfo, laserLength, laserMask))
        {
            lineEnd = hitinfo.point;
        }
        else
        {
            lineEnd = prefabSpawn.position + laserDirection * laserLength;
        }

        laserRenderer.SetPosition(0, laserRenderer.transform.InverseTransformPoint(prefabSpawn.position));
        laserRenderer.SetPosition(1, laserRenderer.transform.InverseTransformPoint(lineEnd));
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

}
