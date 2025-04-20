using UnityEngine;

public class RamWaypointNavigator : MonoBehaviour
{
    public Waypoint currentWaypoint;
    public float moveSpeed = 30f;
    public float rotationSpeed = 5f;
    public float stopDistance = 1.5f;

    private bool isDead = false;

    private void Update()
    {
        if (isDead || currentWaypoint == null) return;

        Vector3 targetPosition = currentWaypoint.GetPosition();
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 flatDir = new Vector3(direction.x, 0f, direction.z);

        transform.position += flatDir * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(flatDir), Time.deltaTime * rotationSpeed);

        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance < stopDistance) {
            Waypoint next = GetNextWaypoint();
            if (next != null) {
                currentWaypoint = next;
            } else {
                Destroy(gameObject); // reached end of road
            }
        }
    }

    private Waypoint GetNextWaypoint()
    {
        bool shouldBranch = false;

        if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0) {
            shouldBranch = Random.value <= currentWaypoint.branchRatio;
        }

        if (shouldBranch) {

            return currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count)];
        }

        return currentWaypoint.nextWaypoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDead) {

            isDead = true;
            Debug.Log("Ram Drone hit the player!");
            other.GetComponent<CarDeath>()?.TriggerDeath();

            Destroy(gameObject);

        }
    }
}
