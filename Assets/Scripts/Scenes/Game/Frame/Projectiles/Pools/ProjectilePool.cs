using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    private ProjectileConfig _projectileConfig;
    private int _expansionSize;

    private Queue<Projectile> _pool = new Queue<Projectile>();

    public void Initialize(ProjectileConfig config)
    {
        _projectileConfig = config;
        _expansionSize = config.poolExpSize;

        Expand(config.poolInitSize);
    }

    private Projectile Expand(int toAdd)
    {
        Projectile tmp = null;  // Used if there is no objects left

        for (int i = 0; i < toAdd; i++)
        {
            Projectile comp = InitializeObject(_projectileConfig);

            _pool.Enqueue(comp);
            if (i == toAdd - 1)
            {
                tmp = comp;
            }
        }

        return tmp;
    }

    private Projectile InitializeObject(ProjectileConfig config)
    {
        GameObject obj = Instantiate(config.prefab, transform);
        Projectile comp = obj.GetComponent<Projectile>();
        Unclassified.NullCheckComponent(comp);
        comp.Initialize(config);

        return comp;
    } 

    public Projectile Get()
    {
        Projectile comp;

        if (_pool.Count > 0)
        {
            comp = _pool.Dequeue();
        }
        else
        {
            comp = Expand(_expansionSize);
        }

        comp.Activate();
        return comp;
    }

    public void Return(Projectile comp)
    {
        comp.Deactivate();
        _pool.Enqueue(comp);
    }
}
