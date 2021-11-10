using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public static SoundManager GetInstance()
    {
        return instance;
    }

    private AudioSource m_AudioSource;

    private void Awake()
    {
        instance = this;
        m_AudioSource = GetComponent<AudioSource>();
    }

    public enum Sound
    {
        BirdJump,
        Score,
        Lose,
        ButtonOver,
        ButtonClick
    }

    public void PlaySound(Sound sound)
    {        
        m_AudioSource.PlayOneShot(GetAudioClip(sound));
    }

    private AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.GetInstance().soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }

        Debug.LogError("Sound " + sound + " not found!");

        return null;
    }
}