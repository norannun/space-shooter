
using UnityEngine;

public class NovaShot : Projectile
{
    [SerializeField] private NovaShotConfig config;

    private void Awake()
    {
        Initialize(config);
    }

    protected override void Initialize<NovaShotConfig>(NovaShotConfig config)
    {
        base.Initialize(config);
    }
}

[CreateAssetMenu(fileName = "NovaShot Config", menuName = "Configs/Projectiles/Instances/NovaShot")]
public class NovaShotConfig : ProjectileConfig
{

}