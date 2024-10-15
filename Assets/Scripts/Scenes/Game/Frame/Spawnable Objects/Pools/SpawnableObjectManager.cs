using System.Collections.Generic;
using UnityEngine;

public class SpawnableObjectManager : MonoBehaviour
{
    public static SpawnableObjectManager Instance { get; private set; }

    [SerializeField] private SpawnableObjectManagerConfig _spawnableObjectConfigs;

    private Dictionary<string, SpawnableObjectPool> pools;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        pools = new Dictionary<string, SpawnableObjectPool>();

        foreach (SpawnableObjectConfig config in _spawnableObjectConfigs.configs)
        {
            GameObject pool = new();
            SpawnableObjectPool comp = pool.AddComponent<SpawnableObjectPool>();
            comp.Initialize(config);
            pool.transform.parent = transform;
            pool.name = $"{config.spawnableObjectName} Pool";

            pools.Add(config.spawnableObjectName, comp);
        }
    }

    public SpawnableObject Get(string name)
    {
        SpawnableObject obj = pools[name].Get();

        return obj;
    }

    public void Return(string name, SpawnableObject comp)
    {
        pools[name].Return(comp);
    }
}

[CreateAssetMenu(fileName = "SpawnableObjectManager Config", menuName = "Configs/Spawnable Objects/SpawnableObjectManager")]
public class SpawnableObjectManagerConfig : ScriptableObject
{
    public SpawnableObjectConfig[] configs;
}
