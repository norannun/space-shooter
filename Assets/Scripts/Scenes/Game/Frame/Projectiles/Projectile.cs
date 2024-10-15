using UnityEngine;

public abstract class Projectile : PoolableObject
{
    public float Speed { get; protected set; }
    public int Damage { get; protected set; }

    private BoxCollider2D _boxCollider2D;

    private float _topBoundary;

    // PoolableObject implementation
    public override void Initialize(PoolableObjectConfig config)
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        Unclassified.NullCheckComponent(_boxCollider2D);

        ProjectileConfig projectileConfig = config as ProjectileConfig;
        Speed = projectileConfig.speed;
        Damage = projectileConfig.damage;

        _topBoundary = GlobalValuesManager.Instance.TopBoundary;

        base.Initialize(config);
    }

    public override void Activate()
    {
        gameObject.SetActive(true);
        _boxCollider2D.enabled = true;
    }

    public override void Deactivate()
    {
        _boxCollider2D.enabled = false;
        gameObject.SetActive(false);
    }

    // Projectile behaviour
    protected void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 position = transform.position;

        transform.Translate(Vector3.up * Time.deltaTime * Speed);

        if (position.y > _topBoundary + _boxCollider2D.bounds.size.y / 2)
        {
            ProjectileManager.Instance.ReturnObject(Name, this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProjectileManager.Instance.ReturnObject(Name, this);
    }
}


public abstract class ProjectileConfig : PoolableObjectConfig
{
    public float speed;
    public int damage;
}
