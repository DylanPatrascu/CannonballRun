using System.Collections;
using UnityEngine;

public class EnemyEMP : Enemy
{

    [Header("EMP Drone Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float followDistance = 8f;
    [SerializeField] private float followHeight = 5f;

    [Header("EMP Attack Settings")]
    [SerializeField] private float fireRate = 3f;
    [SerializeField] private float empDuration = 3f;

    [Header("Line Settings")]
    [SerializeField] private LineRenderer laserLine; //  The gameobject must have a LineRenderer component attached to it
    [SerializeField] private Color trackingColor = Color.cyan;
    [SerializeField] private Color lockOnColor = Color.yellow;
    [SerializeField] private Color firingColor = Color.red;
    [SerializeField] private float trackDuration = 2f;
    [SerializeField] private float lockDelay = 1f;
    [SerializeField] private Transform firePoint; 

    private float fireTimer = 0f;
    private bool isFiring = false;
    private float lineTimer = 0f;
    private Vector3 lockedTargetPosition;

    private void Start()
    {
        if (player == null) {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) {
                player = playerObj.transform;
            }
        }
    }

    private void Update()
    {
        if (player == null) return;

        // This is the movement mechanics, it will follow ahead of the car and hover
        Vector3 targetPosition = player.position + player.forward * followDistance;
        targetPosition.y = followHeight;
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);


        // This is the attack logic
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate) {
            AttemptEMPAttack();
            fireTimer = 0f;
        }
    }

    private IEnumerator AnimateLaserColor(Color fromColor, Color toColor, float duration)
    {
        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime / duration;

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(Color.Lerp(fromColor, toColor, t), 0f),
                    new GradientColorKey(Color.Lerp(fromColor, toColor, t), 1f)
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 1f)
                }
            );

            laserLine.colorGradient = gradient;
            yield return null;

        }
    }



    private void AttemptEMPAttack() {
        if (!isFiring) {
            StartCoroutine(TrackAndFireEMP());
        }
    }

    private IEnumerator TrackAndFireEMP() {
        isFiring = true;
        lineTimer = 0f;

        laserLine.enabled = true;

        // This is the blue segment (tracking phase)
        StartCoroutine(AnimateLaserColor(firingColor, trackingColor, 0.1f));

        // This will track the player for x seconds
        while (lineTimer < trackDuration) {
            if (player != null) {
                lockedTargetPosition = player.position;
                laserLine.SetPosition(0, firePoint.position);
                laserLine.SetPosition(1, lockedTargetPosition);
            }

            lineTimer += Time.deltaTime;
            yield return null;
        }

        // This will cache the X position
        float lockedX = lockedTargetPosition.x;
        float lockedZ = lockedTargetPosition.z;

        // This is the yellow segment (lock on phase)

        // This will lock onto position
        StartCoroutine(AnimateLaserColor(trackingColor, lockOnColor, 0.3f));
        float lockOnTime = 0f;
        while (lockOnTime < lockDelay) {
            if (player != null) {
                // This will lock the X movement, and will only follow z movement
                Vector3 playerPos = player.position;
                lockedTargetPosition = new Vector3(lockedX, playerPos.y, playerPos.z);

                laserLine.SetPosition(0, firePoint.position);
                laserLine.SetPosition(1, lockedTargetPosition);

                
            }

            lockOnTime += Time.deltaTime;
            yield return null;
        }

        // This is the red segment (Fire Phase)
        StartCoroutine(AnimateLaserColor(lockOnColor, firingColor, 0.2f));
        
        laserLine.SetPosition(0, firePoint.position);
        laserLine.SetPosition(1, lockedTargetPosition);

        yield return new WaitForSeconds(0.0f);

        // This will fire the EMP
        Ray ray = new Ray(firePoint.position, (lockedTargetPosition - firePoint.position).normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f)) {
            if (hit.collider.CompareTag("Player")) {
                CarController car = hit.collider.GetComponent<CarController>();
                if (car != null) {
                    car.DisableMovement(empDuration);
                }
            }
        }

        laserLine.enabled = false;
        isFiring = false;
        StartCoroutine(AnimateLaserColor(firingColor, trackingColor, 0.1f));

    }

    protected override void Die()
    {
        isDead = true;
        StaticData.scrap += scrapReward;
        Debug.Log("EMP Drone Destroyed. Awarding " + scrapReward + " scrap!");
        base.Die();
    }

}
