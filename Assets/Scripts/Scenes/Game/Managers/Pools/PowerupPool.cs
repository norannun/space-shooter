using UnityEngine;

public class PowerupPool : MonoBehaviour
{
    public static PowerupPool Instance { get; private set; }

    [SerializeField] private GameObject[] _powerups;

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

    public GameObject GetRandomPowerup()
    {
        int randomIdx = Random.Range(0, _powerups.Length);
        GameObject obj = _powerups[randomIdx];

        obj.SetActive(true);
        return obj;
    }

    public void ReturnPowerup(GameObject obj)
    {
        obj.SetActive(false);
    }
}
