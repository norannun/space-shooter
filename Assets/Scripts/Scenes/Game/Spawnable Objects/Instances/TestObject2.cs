using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject2 : SpawnableObject
{
    [SerializeField] private TestObject2Config config;

    private void Awake()
    {
        base.Initialize(config);
    }
}

[CreateAssetMenu(fileName="TestObject2 Config", menuName= "Configs/Spawnable Objects/TestObject2 Config")]
public class TestObject2Config : SpawnableObjectConfig
{

}