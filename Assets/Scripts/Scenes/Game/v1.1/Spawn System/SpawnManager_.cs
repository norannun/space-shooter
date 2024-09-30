using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager_ : MonoBehaviour
{
    public SpawnManager_ Instance { get; private set; }

    // SpawnPositionManager Test
    public GameObject spawnableTest;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            // SpawnPositionManager Test
            GameObject obj = Instantiate(spawnableTest);
            SpawnableObject comp = obj.GetComponent<SpawnableObject>();
            comp.Speed = 4.0f;
            obj.transform.parent = transform;
            obj.transform.position = SpawnPositionManager_.Instance.GetPosition(comp);
        }
    }
}
