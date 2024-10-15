using UnityEngine;

public abstract class SpawnableObject : MonoBehaviour
{
    // Configuration
    public static string Name { get; protected set; }
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

    public virtual void Initialize(SpawnableObjectConfig config)
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Unclassified.NullCheckComponent(_boxCollider2D);
        Unclassified.NullCheckComponent(_spriteRenderer);
        _boxCollider2D.isTrigger = true;

        Name = config.spawnableObjectName;
        Health = config.health;
        SpeedY = config.speed;
        Mass = config.mass;
        FireRate = config.fireRate;
        ProjectileOffsetX = config.projectileOffsetX;
        ProjectileOffsetY = config.projectileOffsetY;
        CollisionDamage = config.collisionDamage;
        TransparencyModifier = config.transparencyModifier;

        Size = _boxCollider2D.bounds.size;
        collisions = 0;

        Deactivate();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        collisions = 0;
        _boxCollider2D.enabled = true;
        ModifyTransparency(1.0f);
    }

    public void Deactivate()
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

    private void ReceiveDamage(int damage)
    {
        Health -= damage;

        if (Health < 1)
        {
            SpawnableObjectManager.Instance.Return(Name, this);
        }
    }

    private void ModifyTransparency(float value)
    {
        Material material = _spriteRenderer.material;
        Color color = material.color;
        color.a = value;
    }

    // Collisions
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
            Projectile projectile = other.GetComponent<Projectile>();
            Unclassified.NullCheckComponent(projectile);

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


public abstract class SpawnableObjectConfig : ScriptableObject
{
    public string spawnableObjectName;
    public int health;
    public float speed;
    public int mass;
    public float fireRate;
    public float projectileOffsetY;
    public int projectileOffsetX;
    public int collisionDamage;
    public float transparencyModifier;
    public GameObject prefab;
    public int poolInitSize;
    public int poolExpSize;
}