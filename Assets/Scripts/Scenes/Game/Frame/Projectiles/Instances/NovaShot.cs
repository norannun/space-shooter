
using UnityEngine;

public class NovaShot : Projectile
{
    public override void Initialize(PoolableObjectConfig config)
    {
        NovaShotConfig novaShotConfig = config as NovaShotConfig;
        // Config initialization logic below, if needed

        base.Initialize(config);
    }
}

[CreateAssetMenu(fileName = "NovaShot Config", menuName = "Configs/Projectiles/NovaShot")]
public class NovaShotConfig : ProjectileConfig
{

}