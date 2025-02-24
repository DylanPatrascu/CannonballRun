using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CarSelectMenu : MonoBehaviour
{

    [SerializeField] private Transform carSpawnPoint;
    [SerializeField] private List<Car> cars;
    private Car currentCar;

    private void Start()
    {
        currentCar = cars[0];
    }
    private void SpawnCar(GameObject carModel)
    {
        foreach (Transform child in carSpawnPoint) Destroy(child);
        GameObject newcar = Instantiate(carModel,carSpawnPoint);
        newcar.AddComponent(typeof(MenuCarSelection));
    }
}
