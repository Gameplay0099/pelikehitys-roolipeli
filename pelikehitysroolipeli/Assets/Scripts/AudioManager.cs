using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public enum SoundEffects
    {
        PlayerHitWall,
        OpenDoor,
        FindMerchant,
        BuyItem,
        InvalidAction,
        Select,
        Walk
    }
    public enum Music
    {
        moonspire,
        windsOfValor,
        darkwoodPath,
        frostbound,
        emberlight,
        silverbrook,
        mysticGrove,
        throneOfStorms,
        sorrowsEdge,
        elvenDawn
    }
    [SerializeField] AudioClip music;

    [Header("Sound/Music audio sources")]
    [SerializeField] AudioSource soundEffectPlayer;
    [SerializeField] AudioSource musicPlayer;

    // Käytettävät ääniefektit
    [Header("AudioClips for Sound effect AudioSource")]
    [SerializeField] AudioClip hitWall;
    [SerializeField] AudioClip invalidAction;
    [SerializeField] AudioClip walk;
    [SerializeField] AudioClip buyItem;
    [SerializeField] AudioClip findMerchant;
    [SerializeField] AudioClip select;
    [SerializeField] AudioClip openDoor;

    [Header("AudioClips for Music player AudioSource")]
    [SerializeField] AudioClip moonspire;
    [SerializeField] AudioClip windsOfValor;
    [SerializeField] AudioClip darkwoodPath;
    [SerializeField] AudioClip frostbound;
    [SerializeField] AudioClip emberlight;
    [SerializeField] AudioClip silverbrook;
    [SerializeField] AudioClip mysticGrove;
    [SerializeField] AudioClip throneOfStorms;
    [SerializeField] AudioClip sorrowsEdge;
    [SerializeField] AudioClip elvenDawn;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Pelissä on liikaa AudioManager objekteja...!!!!!!!!!!!!!!!!!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public void PlaySound(SoundEffects sound)
    {
        TurnOffLoop();
        switch (sound)
        {
            case SoundEffects.PlayerHitWall:
                soundEffectPlayer.PlayOneShot(hitWall);
                break;
            case SoundEffects.OpenDoor:
                soundEffectPlayer.PlayOneShot(openDoor);
                break;
            case SoundEffects.FindMerchant:
                soundEffectPlayer.PlayOneShot(findMerchant);
                break;
            case SoundEffects.BuyItem:
                soundEffectPlayer.PlayOneShot(buyItem);
                break;
            case SoundEffects.Select:
                soundEffectPlayer.PlayOneShot(select);
                break;
            case SoundEffects.Walk:
                soundEffectPlayer.loop = true;
                soundEffectPlayer.clip = walk;
                soundEffectPlayer.Play();
                break;
            case SoundEffects.InvalidAction:
                soundEffectPlayer.PlayOneShot(invalidAction);
                break;
        }
    }

    public void TurnOffLoop()
    {
        soundEffectPlayer.loop = false;
    }

    public bool GetSoundPlayerLoopStatus()
    {
        return soundEffectPlayer.loop;
    }

    public float GetWalkSoundLength()
    {
        return walk.length;
    }

    public void ToggleMusic(Music music)
    {
        switch (music)
        {
            case Music.moonspire:
                musicPlayer.clip = moonspire;
                break;
            case Music.windsOfValor:
                musicPlayer.clip = windsOfValor;
                break;
            case Music.darkwoodPath:
                musicPlayer.clip = darkwoodPath;
                break;
            case Music.frostbound:
                musicPlayer.clip = frostbound;
                break;
            case Music.emberlight:
                musicPlayer.clip = emberlight;
                break;
            case Music.silverbrook:
                musicPlayer.clip = silverbrook;
                break;
            case Music.mysticGrove:
                musicPlayer.clip = mysticGrove;
                break;
            case Music.throneOfStorms:
                musicPlayer.clip = throneOfStorms;
                break;
            case Music.sorrowsEdge:
                musicPlayer.clip = sorrowsEdge;
                break;
            case Music.elvenDawn:
                musicPlayer.clip = elvenDawn;
                break;
        }
        if (musicPlayer.isPlaying)
        {
            musicPlayer.Pause();
        }
        else
        {
            musicPlayer.Play();
        }
    }

    public void StopMusic()
    {
        if (musicPlayer.isPlaying)
        {
            musicPlayer.Pause();
        }
    }
}
