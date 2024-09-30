using UnityEngine;

public class SpawnableObject : MonoBehaviour
{
    public Vector2 Size { get; private set; }
    public float Speed { get; set; }

    protected void Awake()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            Size = collider.bounds.size;
        }
        else
        {
            Debug.LogError("SpawnableObject: collider is null");
        }
    }

    protected void Update()
    {
        MoveDown(Speed);
    }

    protected void MoveDown(float speed)
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed);
    }
}