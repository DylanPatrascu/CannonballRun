using System.Collections;
using UnityEngine;

public class EnemyEMP : Enemy
{

    [Header("EMP Drone Settings")]

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

    [SerializeField] private MeshRenderer hexRenderer;
    [SerializeField] private int[] hexMaterialIndices;

    private Material[] pulseMats;


    private float fireTimer = 0f;
    private bool isFiring = false;
    private float lineTimer = 0f;
    private Vector3 lockedTargetPosition;

    protected override void Start()
    {
        base.Start();

        if (player == null) {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) {
                player = playerObj.transform;
            }
        }
        if (hexRenderer == null || hexMaterialIndices == null || hexMaterialIndices.Length == 0) {
            Debug.LogError("Hex renderer or material indices not assigned!");
            return;
        }

        pulseMats = new Material[hexMaterialIndices.Length];

        Material[] allMats = hexRenderer.materials;
        
        for (int i = 0; i < hexMaterialIndices.Length; i++)
        {
            int matIndex = hexMaterialIndices[i];
            if (matIndex >= allMats.Length)
            {
                Debug.LogError($"Material index {matIndex} is out of bounds!");
                continue;
            }

            pulseMats[i] = allMats[matIndex];
            pulseMats[i].SetFloat("_PulseSpeed", 1f);
            pulseMats[i].SetFloat("_EmissionIntensity", 1f);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (player == null) return;


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

        StartCoroutine(ChargeInwardEffect()); //  This is the charging effect on the drone

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

        Vector3 localOffset = player.InverseTransformPoint(lockedTargetPosition);
        // This is the yellow segment (lock on phase)

        // This will lock onto position
        StartCoroutine(AnimateLaserColor(trackingColor, lockOnColor, 0.3f));
        float lockOnTime = 0f;
        while (lockOnTime < lockDelay) {
            if (player != null) {
                // This will lock the X movement, and will only follow z movement
                Vector3 playerPos = player.position;

                Vector3 currentWorldOffset = player.TransformPoint(new Vector3(localOffset.x, localOffset.y, 0f));

                Vector3 dynamicOffset = new Vector3(localOffset.x, localOffset.y, 0f);
                lockedTargetPosition = player.TransformPoint(dynamicOffset);



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
        for (int i = 0; i < pulseMats.Length; i++) {
            pulseMats[i].SetFloat("_PulseSpeed", 1f);
            pulseMats[i].SetFloat("_EmissionIntensity", 1f);
        }
        StartCoroutine(AnimateLaserColor(firingColor, trackingColor, 0.1f));

    }

    private IEnumerator ChargeInwardEffect() {
        float delay = 0.1f;
        if (pulseMats == null || pulseMats.Length == 0) {
            Debug.LogError("pulseMats not initialized!");
            yield break;
        }
        for (int i = 0; i < pulseMats.Length; i++) {
            Debug.Log($"Charging Hex {i}: Speed={3f + i}, Intensity={2f + i * 0.5f}");
            pulseMats[i].SetFloat("_PulseSpeed", 3f + i);
            pulseMats[i].SetFloat("_EmissionIntensity", 2f + i * 0.5f);
            yield return new WaitForSeconds(delay);
        }
    }

    protected override void Die()
    {
        isDead = true;
        StaticData.scrap += scrapReward;
        Debug.Log("EMP Drone Destroyed. Awarding " + scrapReward + " scrap!");
        base.Die();
    }

}
