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
    public float CurrentSpeedY { get; private set; }
    public float CurrentMass { get; private set; }

    // Constants
    public Vector2 Size { get; protected set; }

    // Complonents
    private BoxCollider2D _boxCollider2D;

    // Collisions
    private SpawnableObject upperCollision;
    private SpawnableObject lowerCollision;

    public SpawnableObject UpperCollision
    {
        get { return upperCollision; }
        set
        {
            // Add collision
            if (upperCollision == null && value != null)
            {
                upperCollision = value;

                if (value.CurrentMass >= CurrentMass)
                {
                    CurrentMass = value.CurrentMass;
                    CurrentSpeedY = value.CurrentSpeedY;
                }

                if (lowerCollision != null)
                {
                    lowerCollision.UpperCollision = this;
                }
            }

            // Remove collision
            if (upperCollision != null && value == null)
            {
                upperCollision = null;

                if (lowerCollision != null)
                {
                    if (lowerCollision.CurrentMass >= CurrentMass)
                    {
                        CurrentMass = lowerCollision.CurrentMass;
                        CurrentSpeedY = lowerCollision.CurrentSpeedY;
                    }
                    else
                    {
                        CurrentMass = Mass;
                        CurrentSpeedY = SpeedY;
                    }

                    lowerCollision.UpperCollision = this;
                }
                else
                {
                    CurrentMass = Mass;
                    CurrentSpeedY = SpeedY;
                }
            }
        }
    }
    
    public SpawnableObject LowerCollision
    {
        get { return lowerCollision; }
        set
        {
            // Add collision
            if (lowerCollision == null && value != null)
            {
                lowerCollision = value;

                if (value.CurrentMass >= CurrentMass)
                {
                    CurrentMass = value.CurrentMass;
                    CurrentSpeedY = value.CurrentSpeedY;
                }

                if (upperCollision != null)
                {
                    upperCollision.LowerCollision = this;
                }
            }

            // Remove collision
            if (lowerCollision != null && value == null)
            {
                lowerCollision = null;

                if (upperCollision != null)
                {
                    if (upperCollision.CurrentMass >= CurrentMass)
                    {
                        CurrentMass = upperCollision.CurrentMass;
                        CurrentSpeedY = upperCollision.CurrentSpeedY;
                    }
                    else
                    {
                        CurrentMass = Mass;
                        CurrentSpeedY = SpeedY;
                    }

                    upperCollision.LowerCollision = this;
                }
                else
                {
                    CurrentMass = Mass;
                    CurrentSpeedY = SpeedY;
                }
            }
        }
    }

    protected virtual void Initialize<T>(T config) where T : SpawnableObjectConfig
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        Unclassified.NullCheckComponent(_boxCollider2D);
        _boxCollider2D.isTrigger = true;

        Name = config.objectName;
        SpeedY = config.speedY;
        Mass = config.mass;

        CurrentSpeedY = SpeedY;
        CurrentMass = Mass;

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
        transform.Translate(Vector2.down * Time.deltaTime * CurrentSpeedY);
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
        Vector2 direction = other.transform.position - transform.position;  // Collison direction calculation method is sus
        direction.Normalize();

        bool isUpperCollision = direction.y > 0;

        if (isUpperCollision)
        {
            LowerCollision = other;
        }
        else
        {
            UpperCollision = other;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        SpawnableObject otherObject = other.GetComponent<SpawnableObject>();
        if (otherObject != null)
        {
            HandleCollisionExit(otherObject);
        }
    }

    private void HandleCollisionExit(SpawnableObject other)  // Probably, should combine with HandleCollision
    {
        Vector2 direction = other.transform.position - transform.position;
        direction.Normalize();

        bool isUpperCollision = direction.y > 0;

        if (isUpperCollision)
        {
            LowerCollision = null;
        }
        else
        {
            UpperCollision = null;
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