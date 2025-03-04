using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
    NPCController controller;
    public Waypoint currentWaypoint;

    private void Awake()
    {
        controller = GetComponent<NPCController>();
    }

    private void Start()
    {
        controller.SetDestination(currentWaypoint.GetPosition());
    }

    private void Update()
    {
        if (controller.reachedDestination == true)
        {
            bool shouldBranch = false;

            if(currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
            {
                shouldBranch = Random.Range(0f, 1f) <= currentWaypoint.branchRatio ? true : false;
            }

            if(shouldBranch)
            {
                //NEEDS CHANGING, JUST UPDATE THE WAYPOINT TO THE FIRST NODE IN BRANCH SO THE LANE SWITCHES PROPERLY
                currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count - 1)];
            } 
            

            currentWaypoint = currentWaypoint.nextWaypoint;
            controller.SetDestination(currentWaypoint.GetPosition());
        }
    }
}
