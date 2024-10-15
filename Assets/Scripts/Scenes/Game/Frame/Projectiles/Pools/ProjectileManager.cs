using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance { get; private set; }

    [SerializeField] private ProjectilePoolManagerConfig _projectileConfigs;

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

        foreach (ProjectileConfig config in _projectileConfigs.configs)
        {
            GameObject pool = new();
            ProjectilePool comp = pool.AddComponent<ProjectilePool>();
            comp.Initialize(config);
            pool.transform.parent = transform;
            pool.name = $"{config.projectileName} Pool";

            pools.Add(config.projectileName, comp);
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
        pools[name].Return(comp);
    }
}

[CreateAssetMenu(fileName = "ProjectileManager Config", menuName = "Configs/Projectiles/ProjectileManager")]
public class ProjectilePoolManagerConfig : ScriptableObject
{
    public ProjectileConfig[] configs;
}