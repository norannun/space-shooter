using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolManager : MonoBehaviour
{
    public static ProjectilePoolManager Instance { get; private set; }


    [SerializeField] private ProjectilePoolManagerConfig poolManagerConfig;
    [SerializeField] private ProjectilePoolConfig poolConfig;

    private Dictionary<string, ProjectilePool> pools;

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
        pools = new Dictionary<string, ProjectilePool>();

        foreach (GameObject obj in poolManagerConfig.prefabs)
        {
            GameObject pool = new();
            ProjectilePool comp = pool.AddComponent<ProjectilePool>();
            comp.Initialize(obj, poolConfig);
            pool.transform.parent = transform;
            pool.name = $"{comp.ObjectName} Pool";

            pools.Add(comp.ObjectName, comp);
        }
    }

    public Projectile SpawnProjectile(string name, Vector2 position)
    {
        Projectile projectile = pools[name].Get();
        projectile.gameObject.transform.position = position;

        return projectile;
    }

    public void DestroyProjectile(string name, Projectile comp)
    {
        pools[name].ReturnProjectile(comp);
    }
}

[CreateAssetMenu(fileName = "ProjectilePoolManager Config", menuName = "Configs/Projectiles/ProjectilePoolManager")]
public class ProjectilePoolManagerConfig : ScriptableObject
{
    public GameObject[] prefabs;
}