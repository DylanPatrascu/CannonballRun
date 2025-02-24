using UnityEngine;

[CreateAssetMenu]
public class Car : ScriptableObject
{
    public GameObject carModel;
    public float acceleration = 25f;
    public float maxSpeed = 100f;
    public float deceleration = 10f;
    public float steerStrength = 15f;
    public AnimationCurve turningCurve;
    public float dragCoefficient = 1f;
}
