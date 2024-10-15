using UnityEngine;

public abstract class SpawnableObject : PoolableObject
{
    // Configuration
    public int Health { get; protected set; }
    public float SpeedY { get; protected set; }
    public float Mass { get; protected set; }
    public float FireRate { get; protected set; }
    public float ProjectileOffsetY { get; protected set; }
    public int ProjectileOffsetX { get; protected set; }
    public int CollisionDamage { get; protected set; }
    public float TransparencyModifier { get; protected set; }

    public Vector2 Size { get; protected set; }

    // Complonents
    private BoxCollider2D _boxCollider2D;
    private SpriteRenderer _spriteRenderer;

    private int collisions;

    public override void Initialize(PoolableObjectConfig config)
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Unclassified.NullCheckComponent(_boxCollider2D);
        Unclassified.NullCheckComponent(_spriteRenderer);
        _boxCollider2D.isTrigger = true;

        SpawnableObjectConfig spawnableObjectConfig = config as SpawnableObjectConfig;
        Health = spawnableObjectConfig.health;
        SpeedY = spawnableObjectConfig.speed;
        Mass = spawnableObjectConfig.mass;
        FireRate = spawnableObjectConfig.fireRate;
        ProjectileOffsetX = spawnableObjectConfig.projectileOffsetX;
        ProjectileOffsetY = spawnableObjectConfig.projectileOffsetY;
        CollisionDamage = spawnableObjectConfig.collisionDamage;
        TransparencyModifier = spawnableObjectConfig.transparencyModifier;

        Size = _boxCollider2D.bounds.size;
        collisions = 0;

        base.Initialize(config);
    }

    public override void Activate()
    {
        gameObject.SetActive(true);
        collisions = 0;
        _boxCollider2D.enabled = true;
        ModifyTransparency(1.0f);
    }

    public override void Deactivate()
    {
        _boxCollider2D.enabled = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector2.down * Time.deltaTime * SpeedY);
    }

    // Collisions
    private void ReceiveDamage(int damage)
    {
        Health -= damage;

        if (Health < 1)
        {
            SpawnableObjectManager.Instance.ReturnObject(Name, this);
        }
    }

    private void ModifyTransparency(float value)
    {
        Material material = _spriteRenderer.material;
        Color color = material.color;
        color.a = value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Spawnable Object")
        {
            SpawnableObject spawnableObject = other.gameObject.GetComponent<SpawnableObject>();
            Unclassified.NullCheckComponent(spawnableObject);
            collisions++;

            ModifyTransparency(TransparencyModifier);
        }
        if (other.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            Unclassified.NullCheckComponent(player);

            player.ReceiveDamage(CollisionDamage);
            ReceiveDamage(player.CollisionDamage);
        }
        if (other.tag == "Projectile")
        {
            PoolableObject obj = other.GetComponent<PoolableObject>();
            Unclassified.NullCheckComponent(obj);
            Projectile projectile = obj as Projectile;

            ReceiveDamage(projectile.Damage);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        SpawnableObject spawnableObject = other.GetComponent<SpawnableObject>();
        if (spawnableObject != null)
        {
            collisions--;
        }

        if (collisions == 0)
        {
            ModifyTransparency(1.0f);
        }
    }
}


public abstract class SpawnableObjectConfig : PoolableObjectConfig
{
    public int health;
    public float speed;
    public int mass;
    public float fireRate;
    public float projectileOffsetY;
    public int projectileOffsetX;
    public int collisionDamage;
    public float transparencyModifier;
}