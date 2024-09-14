using UnityEngine;

public class Powerup : MonoBehaviour
{
    public delegate void PowerupCollectedHandle(Type type, float duration);
    public static event PowerupCollectedHandle OnPowerupCollected;

    public enum Type { TripleShot, Speed, Shield }
    public enum PowerupState { Passive, Active }

    [SerializeField] private Type _type;
    [SerializeField] private PowerupState _state = PowerupState.Passive;
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private float _duration = 4.5f;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;

    private Player _player;

    private void Start()
    {
        if (_state == PowerupState.Active)
        {
            PowerupPool.Instance.ReturnPowerup(gameObject);
            OnPowerupCollected?.Invoke(_type, _duration);
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError("Powerup: _boxCollider2D is NULL");
            enabled = false;
            return;
        }

        _boxCollider2D = GetComponent<BoxCollider2D>();
        if (_boxCollider2D == null)
        {
            Debug.LogError("Powerup: _boxCollider2D is NULL");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        MoveDown();
        CheckOutOfBounds();
    }

    private void MoveDown()
    {
        if (_state == PowerupState.Passive)
        {
            transform.Translate(Vector3.down * Time.deltaTime * _speed);
        }
    }

    private void CheckOutOfBounds()
    {
        if (transform.position.y < GlobalValuesManager.Instance.BottomBoundary - _spriteRenderer.bounds.size.y / 2 && !(_state == PowerupState.Active))
        {
            PowerupPool.Instance.ReturnPowerup(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPowerupCollected?.Invoke(_type, _duration);  // Subscriber: Player

            PowerupPool.Instance.ReturnPowerup(gameObject);
        }
    }
}
