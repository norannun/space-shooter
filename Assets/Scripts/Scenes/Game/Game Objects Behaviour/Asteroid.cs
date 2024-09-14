using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public delegate void AsteroidDestroyedHandle();
    public static event AsteroidDestroyedHandle OnAsteroidDeath;
    
    private Animator _explosion;
    private SpriteRenderer _spriteRenderer;
    private Sprite _sprite;

    [SerializeField] private float _rotationSpeed = 17f;

    void Start()
    {
        _explosion = GetComponent<Animator>();
        if (_explosion == null)
        {
            Debug.LogError("Asteroid: _explosion is NULL");
            enabled = false;
            return;
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError("Asteroid: _spriteRenderer is NULL");
        }

        _sprite = _spriteRenderer.sprite;
        if (_sprite == null)
        {
            Debug.LogError("Asteroid: _sprite is NULL");
            enabled = false;
            return;
        }

        SpawnManager.OnLevelEnd += Activate;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * _rotationSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destoy asteroid and projectile when they collide
        if (other.tag == "Projectile")
        {
            _explosion.SetTrigger("OnObjectDeath");

            AudioSourcePool.Instance.PlayAudio("Explosion", transform.position);
        }
    }

    private void Activate()
    {
        _spriteRenderer.sprite = _sprite;
        gameObject.SetActive(true);
    }

    private void OnDeathAnimationEnd()
    {
        gameObject.SetActive(false);
        OnAsteroidDeath?.Invoke();
    }
}
