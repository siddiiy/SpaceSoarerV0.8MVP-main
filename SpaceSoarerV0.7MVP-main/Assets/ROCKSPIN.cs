using UnityEngine;

public class RockSpin : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 0, 100);

    void Update()
    {
        // Rotate the object around its own center
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
    }
}
