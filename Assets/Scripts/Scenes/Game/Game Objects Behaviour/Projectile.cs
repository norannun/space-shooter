using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed;

    private BoxCollider2D _boxCollider2D;

    private float _topBoundary;

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        if (_boxCollider2D == null)
        {
            Debug.LogError("Projectile: _boxCollider2D is NULL");
        }

        _topBoundary = GlobalValuesManager.Instance.TopBoundary;
    }

    void Update()
    {
        Vector3 positon = transform.position;

        transform.Translate(Vector3.up * Time.deltaTime * _speed);

        if (positon.y > _topBoundary + _boxCollider2D.bounds.size.y / 2) 
        {
            ProjectilePool.Instance.ReturnProjectile(this);
        }
    }
}
