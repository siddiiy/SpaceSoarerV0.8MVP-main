using UnityEngine;

public class RockRotation : MonoBehaviour
{
    public float rotationSpeed;  // Speed of rotation

    void Start()
    {
        // Assign a random rotation speed
        rotationSpeed = Random.Range(0f, 2f);
    }

    void Update()
    {
        // Rotate the rock around its local Z-axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
