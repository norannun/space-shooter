using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public string Name { get; protected set; }
    public float Speed { get; protected set; }
    public int Damage { get; protected set; }

    private BoxCollider2D _boxCollider2D;

    private float _topBoundary;

    public virtual void Initialize(ProjectileConfig config)
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        Unclassified.NullCheckComponent(_boxCollider2D);

        Name = config.projectileName;
        Speed = config.speed;
        Damage = config.damage;

        _topBoundary = GlobalValuesManager.Instance.TopBoundary;

        Deactivate();
    }

    protected void Update()
    {
        Move();
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

    private void Move()
    {
        Vector3 position = transform.position;

        transform.Translate(Vector3.up * Time.deltaTime * Speed);

        if (position.y > _topBoundary + _boxCollider2D.bounds.size.y / 2)
        {
            ProjectileManager.Instance.DestroyProjectile(Name, this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProjectileManager.Instance.DestroyProjectile(Name, this);
    }
}


public abstract class ProjectileConfig : ScriptableObject
{
    public GameObject prefab;
    public string projectileName;
    public float speed;
    public int damage;
    public int poolInitSize;
    public int poolExpSize;
}
