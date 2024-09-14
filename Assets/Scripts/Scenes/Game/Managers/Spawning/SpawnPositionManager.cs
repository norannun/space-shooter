using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositionManager : MonoBehaviour
{
    public static SpawnPositionManager Instance { get; private set; }

    private float _cellHeight;
    private float _cellWidth;
    private float _boundXLeft;
    private float _boundXRight;
    private float _startingPosY;
    private float _returnPosDelay = 3.0f;

    private HashSet<Vector2> _positions = new HashSet<Vector2>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _cellWidth = Enemy.width;
        _cellHeight = Enemy.height;
        _boundXLeft = GlobalValuesManager.Instance.LeftBoundary + _cellWidth / 2;
        _boundXRight = GlobalValuesManager.Instance.RightBoundary - _cellWidth / 2;
        _startingPosY = GlobalValuesManager.Instance.TopBoundary + _cellHeight / 2;

        FormGrid();
    }

    private void FormGrid()
    {
        float pointer1 = 0;
        float border1 = _boundXLeft;
        float pointer2 = _cellWidth;
        float border2 = _boundXRight;

        while (pointer1 > _boundXLeft)
        {
            _positions.Add(new Vector2(pointer1, _startingPosY));
            pointer1 -= _cellWidth;
        }
        while (pointer2 < _boundXRight)
        {
            _positions.Add(new Vector2(pointer2, _startingPosY));
            pointer2 += _cellWidth;
        }
    }

    public IEnumerator GetPosition(System.Action<Vector2> callback)
    {
        Vector2 position = Unclassified.GetRandomPosition(_positions);

        bool notDone = true;
        while (notDone)
        {
            if (position != GlobalValuesManager.Instance.BadPosiiton)
            {
                _positions.Remove(position);
                StartCoroutine(FreePosition(position));
                callback(position);
                notDone = false;
            }
            else
            {
                yield return new WaitForSeconds(_returnPosDelay);
                position = Unclassified.GetRandomPosition(_positions);
            }
        }
    }

    IEnumerator FreePosition(Vector2 position)
    {
        yield return new WaitForSeconds(_returnPosDelay);

        _positions.Add(position);
    }
}
