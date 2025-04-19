using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour
{

    [SerializeField] private SplineSampler m_splineSampler;
    [SerializeField] private GameObject m_buildings;
    [SerializeField] private List<GameObject> m_buildingPrefabs = new List<GameObject>();
    [SerializeField] MeshFilter m_roadMesh;

    [SerializeField] private float m_buildingDistance = 30;
    [SerializeField] private float m_buildingFrequency = 10;
    [SerializeField] private float m_buildingSize = 50;

    public Coroutine m_coroutine;

    private void Start()
    {
        if (m_coroutine != null) StopCoroutine(m_coroutine);
        m_coroutine = StartCoroutine(SwapBuildings());
    }

    public IEnumerator SwapBuildings()
    {
        yield return null;
        RemoveBuildings();
        SpawnBuildings();
    }

    private void SpawnBuildings()
    {
        Vector3 p1;
        Vector3 p2;

        float step = 1f / m_buildingFrequency;
        for (int i = 0; i < m_buildingFrequency; i++)
        {
            float t = step * i;
            m_splineSampler.SampleSplineWidth(t, m_buildingDistance, out p1, out p2);

            GameObject building1 = Instantiate(m_buildingPrefabs[Random.Range(0, m_buildingPrefabs.Count - 1)]);
            building1.transform.position = p1;
            building1.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
            building1.transform.localScale = new Vector3(m_buildingSize, m_buildingSize, m_buildingSize);
            if (CheckBoundaries(building1)) building1.transform.SetParent(m_buildings.transform);


            GameObject building2 = Instantiate(m_buildingPrefabs[Random.Range(0, m_buildingPrefabs.Count - 1)]);
            building2.transform.position = p2;
            building2.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
            building2.transform.localScale = new Vector3(m_buildingSize, m_buildingSize, m_buildingSize);
            if (CheckBoundaries(building2)) building2.transform.SetParent(m_buildings.transform);
        }

    }

    private bool CheckBoundaries(GameObject checkBuilding)
    {
        Bounds checkBuildingBounds = checkBuilding.GetComponent<MeshFilter>().sharedMesh.bounds;
        checkBuildingBounds = TransformBounds(checkBuilding.transform, checkBuildingBounds);
        Bounds roadBounds = m_roadMesh.sharedMesh.bounds;
        roadBounds = TransformBounds(m_roadMesh.transform, roadBounds);
        

        if (roadBounds.Intersects(checkBuildingBounds))
        {
            Destroy(checkBuilding);
            return false;
        }
        else
        {
            foreach (Transform building in m_buildings.transform)
            {
                Bounds buildingBounds = building.GetComponent<MeshFilter>().sharedMesh.bounds;
                buildingBounds = TransformBounds(building.transform, buildingBounds);
                if (checkBuildingBounds.Intersects(buildingBounds))
                {
                    Debug.Log("big booms");
                    Destroy(checkBuilding);
                    return false;
                }
            }
        }

        return true;
    }

    private void RemoveBuildings()
    {
        foreach (Transform child in m_buildings.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private Bounds TransformBounds(Transform transform, Bounds bounds)
    {
        Vector3 worldCenter = transform.TransformPoint(bounds.center);
        Vector3 worldExtents = transform.TransformVector(bounds.extents);
        return new Bounds(worldCenter, worldExtents * 2);
    }

}
