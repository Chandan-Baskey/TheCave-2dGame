using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float speed;

    Vector3 startPosition;
    Vector3 endPosition;
    float movementFactor; // 0 for not moved, 1 for fully moved

    private void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + movementVector;
    }

    private void Update()
    {
        movementFactor = Mathf.PingPong(Time.time * speed, 1); // Oscillates between 0 and 1
        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor); // Move object
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = this.transform;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}