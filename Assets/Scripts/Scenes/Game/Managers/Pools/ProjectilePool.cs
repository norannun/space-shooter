using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }

    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _intialSize = 15;
    [SerializeField] private int _expansionSize = 10;
    private ObjectPool<Projectile> _pool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _pool = new ObjectPool<Projectile>(_prefab, _intialSize, _expansionSize, transform);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Projectile SpawnProjectile(Vector2 position, float xOffset = 0.0f, float yOffset = 0.0f)
    {
        Projectile projectile = _pool.Get();
        projectile.gameObject.transform.position = new Vector2(position.x + xOffset, position.y + yOffset);

        return projectile;
    }

    public void ReturnProjectile(Projectile comp)
    {
        _pool.Return(comp);
    }
}
