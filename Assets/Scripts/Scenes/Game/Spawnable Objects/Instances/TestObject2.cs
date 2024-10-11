using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TestObject2 : SpawnableObject
{
    [SerializeField] private TestObject2Config config;

    protected void Awake()
    {
        Initialize(config);
    }

    protected override void Initialize<TestObject2Config>(TestObject2Config config)
    {
        base.Initialize(config);
    }
}

[CreateAssetMenu(fileName = "TestObject2 Config", menuName = "Configs/Spawnable Objects/TestObject2 Config")]
public class TestObject2Config : SpawnableObjectConfig
{

}
