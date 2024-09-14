using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //State
    private enum EnemyState { Active, OutOfVision, Destroyed };
    [SerializeField] private EnemyState _state = EnemyState.Active;

    // Numbers
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] public static float width;
    [SerializeField] public static float height;
    private float _lowPos;

    // Components
    private Player _player;

    private SpriteRenderer _spriteRenderer;
    private Sprite _sprite;
    private Animator _explosion;
    private BoxCollider2D _boxCollider2D;

    private void Start()
    {
        // Assignments & Null checkings

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Enemy: _player is NULL");
            enabled = false;
            return;

        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError("Enemy: _boxCollider2D is NULL");
            enabled = false;
            return;
        }

        _sprite = _spriteRenderer.sprite;
        if (_sprite == null)
        {
            Debug.LogError("Enemy: _sprite is NULL");
            enabled = false;
            return;
        }

        _explosion = GetComponent<Animator>();
        if (_explosion == null)
        {
            Debug.LogError("Enemy: _explosion is NULL");
            enabled = false;
            return;
        }

        _boxCollider2D = GetComponent<BoxCollider2D>();
        if (_boxCollider2D == null)
        {
            Debug.LogError("Enemy: _boxCollider2D is NULL");
            enabled = false;
            return;
        }

        width = _boxCollider2D.bounds.size.x;
        height = _boxCollider2D.bounds.size.y;
        _lowPos = GlobalValuesManager.Instance.BottomBoundary - height / 2;
    }

    private void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < _lowPos && _state == EnemyState.Active)
        {
            _state = EnemyState.OutOfVision;
            StartCoroutine(SpawnPositionManager.Instance.GetPosition(MoveToBeggining));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Damage the Player and destroy the Enemy
        if (other.CompareTag("Player"))
        {
            HandlePlayerCollision();
        }

        // Destroy Projectile and Enemy
        if (other.CompareTag("Projectile"))
        {
            HandleProjectileCollision(other);
        }
    }

    private void HandlePlayerCollision()
    {
        if (_player != null)
        {
            _player.Damage();
        }
        else
        {
            Debug.LogError("Enemy: _player is NULL");
        }

        DestroyEnemy();
    }

    private void HandleProjectileCollision(Collider2D projectile)
    {
        Destroy(projectile.gameObject);

        if (_player != null)
        {
            _player.AddScore();
        }
        else
        {
            Debug.LogError("Enemy: _player is NULL");
        }

        DestroyEnemy();
    }

    private void DestroyEnemy()
    {
        _state = EnemyState.Destroyed;

        SpawnManager.Instance.IncreaseEnemiesDestroyed();

        AudioSourcePool.Instance.PlayAudio("Explosion", transform.position);

        _boxCollider2D.enabled = false;
        _explosion.SetTrigger("OnObjectDeath");
    }

    public void ResetEnemy()
    {
        _state = EnemyState.Active;

        _boxCollider2D.enabled = true;
        _spriteRenderer.sprite = _sprite;
    }

    private void OnDeathAnimationEnd()
    {
        ResetEnemy();
        EnemyPool.Instance.ReturnEnemy(this);
    }

    private void MoveToBeggining(Vector2 position)
    {
        _state = EnemyState.Active;
        transform.position = position;
    }
}
