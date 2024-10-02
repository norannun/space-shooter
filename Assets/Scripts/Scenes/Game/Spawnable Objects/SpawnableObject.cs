using UnityEngine;

public abstract class SpawnableObject : MonoBehaviour
{
    // cfg
    public float Speed { get; protected set; }
    public Vector2 Size { get; protected set; }

    protected BoxCollider2D _boxCollider2D;

    protected virtual void Initialize<T>(T config) where T : SpawnableObjectConfig
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        Unclassified.NullCheckComponent(_boxCollider2D);

        Size = _boxCollider2D.bounds.size;
        Speed = config.speed;

        Deactivate();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        _boxCollider2D.enabled = true;
    }

    public void Deactivate()
    {
        _boxCollider2D.enabled = false;
        gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        MoveDown(Speed);
    }

    protected void MoveDown(float speed)
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed);
    }
}

public class SpawnableObjectConfig : ScriptableObject
{
    public float speed;
}