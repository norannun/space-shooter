using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public string ObjectName { get; private set; }
    private GameObject _prefab;
    private int _expansionSize;

    private Queue<Projectile> _pool = new Queue<Projectile>();

    public void Initialize(GameObject prefab, ProjectilePoolConfig config)
    {
        _prefab = prefab;
        _expansionSize = config.expansionSize;

        Expand(config.initialSize);
    }

    private Projectile Expand(int toAdd)
    {
        Projectile tmp = null;  // Used if there is no objects left

        for (int i = 0; i < toAdd; i++)
        {
            GameObject obj = GameObject.Instantiate(_prefab, transform);
            Projectile comp = obj.GetComponent<Projectile>();
            Unclassified.NullCheckComponent(comp);

            _pool.Enqueue(comp);
            if (i == toAdd - 1)
            {
                tmp = comp;
            }
        }

        ObjectName = tmp.Name;
        return tmp;
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

    public void ReturnProjectile(Projectile comp)
    {
        comp.Deactivate();
        _pool.Enqueue(comp);
    }
}

[CreateAssetMenu(fileName = "ProjectilePool Config", menuName = "Configs/Projectiles/ProjectilePool")]
public class ProjectilePoolConfig : ScriptableObject
{
    public int initialSize;
    public int expansionSize;
}
