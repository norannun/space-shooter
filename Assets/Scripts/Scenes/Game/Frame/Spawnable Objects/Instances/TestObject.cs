
using UnityEngine;

public class TestObject : SpawnableObject
{
    public override void Initialize(PoolableObjectConfig config)
    {
        TestObjectConfig testObjectConfig = config as TestObjectConfig;

        base.Initialize(config);
    }
}


[CreateAssetMenu(fileName="TestObject Config", menuName= "Configs/Spawnable Objects/TestObject Config")]
public class TestObjectConfig : SpawnableObjectConfig
{

}
