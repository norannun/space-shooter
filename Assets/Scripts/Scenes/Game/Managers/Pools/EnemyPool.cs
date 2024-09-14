using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance { get; private set; }

    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _initialSize = 35;
    [SerializeField] private int _expansionSize = 15;
    private ObjectPool<Enemy> _pool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _pool = new ObjectPool<Enemy>(_prefab, _initialSize, _expansionSize, transform);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Enemy GetEnemy()
    {
        return _pool.Get();
    }

    public void ReturnEnemy(Enemy comp)
    {
        _pool.Return(comp);
    }
}
