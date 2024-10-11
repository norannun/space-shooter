using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SpawnableObject : MonoBehaviour
{
    // Configuration
    public string Name { get; protected set; }
    public float SpeedY { get; protected set; }
    public float Mass { get; protected set; }

    // Current state
    private float _currentSpeedY;
    private float _currentMass;

    // Constants
    public Vector2 Size { get; protected set; }

    // Complonents
    private BoxCollider2D _boxCollider2D;

    // Current collisions list
    private List<SpawnableObject> upperCollisions = new List<SpawnableObject>();
    private List<SpawnableObject> lowerCollisions = new List<SpawnableObject>();

    protected virtual void Initialize<T>(T config) where T : SpawnableObjectConfig
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        Unclassified.NullCheckComponent(_boxCollider2D);
        _boxCollider2D.isTrigger = true;

        Name = config.objectName;
        SpeedY = config.speedY;
        Mass = config.mass;

        _currentSpeedY = SpeedY;
        _currentMass = Mass;

        Size = _boxCollider2D.bounds.size;

        Deactivate();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        _boxCollider2D.enabled = true;
    }

    public void Deactivate()
    {
        _boxCollider2D.enabled = false;
        gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector2.down * Time.deltaTime * _currentSpeedY);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SpawnableObject otherObject = other.GetComponent<SpawnableObject>();  // Could it be fixed?
        if (otherObject != null)
        {
            HandleCollision(otherObject);
        }
    }

    private void HandleCollision(SpawnableObject other)
    {
        Vector2 direction = other.transform.position - transform.position;
        direction.Normalize();

        bool isUpperCollision = direction.y > 0;

        if (isUpperCollision)
        {
            if (lowerCollisions.Count == 0 && !lowerCollisions.Contains(other))
            {
                lowerCollisions.Add(other);
                UpdateCollisionState();
            }
        }
        else
        {
            if (upperCollisions.Count == 0 && !upperCollisions.Contains(other))
            {
                upperCollisions.Add(other);
                UpdateCollisionState();
            }
        }
    }

    private void UpdateCollisionState()
    {
        // Перерасчет масс и скоростей
        // Собираем все связанные объекты
        List<SpawnableObject> collisionChain = GetCollisionChain();

        // Находим объект с наибольшей массой
        SpawnableObject heaviestObject = collisionChain.OrderByDescending(o => o._currentMass).First();

        // Обновляем массы и скорости
        foreach (var obj in collisionChain)
        {
            obj._currentMass = heaviestObject._currentMass;
            obj._currentSpeedY = heaviestObject._currentSpeedY;
        }
    }

    private List<SpawnableObject> GetCollisionChain(List<SpawnableObject> visitedObjects = null)
    {
        if (visitedObjects == null)
        {
            visitedObjects = new List<SpawnableObject>();
        }

        // Skip this object, if there is a one in the list
        if (visitedObjects.Contains(this))
        {
            return visitedObjects;
        }

        // If this object is not in the list yet, add it
        visitedObjects.Add(this);

        // Add linked objects recursively
        foreach (var obj in upperCollisions)
        {
            obj.GetCollisionChain(visitedObjects);
        }

        foreach (var obj in lowerCollisions)
        {
            obj.GetCollisionChain(visitedObjects);
        }

        return visitedObjects;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        SpawnableObject otherObject = other.GetComponent<SpawnableObject>();
        if (otherObject != null)
        {
            HandleCollisionExit(otherObject);
        }
    }

    private void HandleCollisionExit(SpawnableObject other)
    {
        if (upperCollisions.Contains(other))
        {
            upperCollisions.Remove(other);
            UpdateCollisionState();
        }
        else if (lowerCollisions.Contains(other))
        {
            lowerCollisions.Remove(other);
            UpdateCollisionState();
        }
    }

}


public abstract class SpawnableObjectConfig : ScriptableObject
{
    public static string tag = "spawnable_object";
    public string objectName;
    public float speedY;
    public int mass;
}