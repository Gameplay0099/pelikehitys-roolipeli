using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public enum SoundEffects
    {
        PlayerHitWall,
        DoorOpen
    }

    bool isMusicPlaying = false;

    [SerializeField]
    AudioClip hitWall;
    [SerializeField]
    AudioClip invalidAction;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Pelissä on liikaa AudioManager objekteka...!!!!!!!!!!!!!!!!!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        
    }

    public void PlaySound(SoundEffects sound)
    {

    }

    public void PlayMusic(AudioClip music)
    {
        
    }

    public void StopMusic()
    {

    }
}
