using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    public static AudioSourcePool Instance { get; private set; }

    [SerializeField] private AudioClipDataBase _audioClipDataBase;
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    private Dictionary<GameObject, AudioSource> _audioSources = new Dictionary<GameObject, AudioSource>();
    private Queue<GameObject> _pool = new Queue<GameObject>();
    private const int _initialSize = 10;
    private const int _expansionSize = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            Expand(_initialSize);

            foreach (var clip in _audioClipDataBase.entries)
            {
                _audioClips.Add(clip.clipName, clip.audioClip);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private GameObject Expand(int size)
    {
        GameObject temp = null;

        for (int i = 0; i < size; i++)
        {
            GameObject obj = new GameObject("Audio Agent");
            AudioSource source = obj.AddComponent<AudioSource>();
            source.volume = 0.45f;

            _audioSources.Add(obj, source);
            obj.transform.parent = transform;
            obj.SetActive(false);
            _pool.Enqueue(obj);

            if (i == 0)
            {
                temp = obj;
            }
        }

        return temp;
    }

    private GameObject Get()
    {
        GameObject obj;

        if (_pool.Count > 0)
        {
            obj = _pool.Dequeue();
        }
        else
        {
            obj = Expand(_expansionSize);
        }

        obj.SetActive(true);
        return obj;
    }

    private IEnumerator Return(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        obj.SetActive(false);
        _pool.Enqueue(obj);
    }

    public void PlayAudio(string audioClipName, Vector2 position)
    {
        GameObject agent = Get();
        AudioSource source = _audioSources[agent];

        agent.transform.position = position;
        source.clip = _audioClips[audioClipName];
        source.Play();

        StartCoroutine(Return(agent, source.clip.length));
    }
}


[System.Serializable]
public class AudioClipEntry
{
    public string clipName;
    public AudioClip audioClip;
}

[CreateAssetMenu(fileName = "AudioClip Database", menuName = "Configs/Audio/AudioClip Database")]
public class AudioClipDataBase : ScriptableObject
{
    public AudioClipEntry[] entries;
}
