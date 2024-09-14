using UnityEngine;

[System.Serializable]
public class AudioClipEntry
{
    public string clipName;
    public AudioClip audioClip;
}

[CreateAssetMenu(fileName = "AudioClipDataBase", menuName = "Audio/AudioClipDataBase")]
public class AudioClipDataBase : ScriptableObject
{
    public AudioClipEntry[] entries;
}
