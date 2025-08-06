using UnityEngine;
using UnityEngine.Audio;

public class Sound_Manager : MonoBehaviour
{
    public static Sound_Manager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource; // 배경 음악을 위한 AudioSource
    public AudioSource effectsSource; // 효과음을 위한 AudioSource

    [Header("Audio Clips")]
    public AudioClip[] musicClips; // 여러 음악 클립을 저장
    public AudioClip[] soundEffects; // 여러 효과음 클립을 저장

    [Header("Mixer")]
    public AudioMixer audioMixer; // 오디오 믹서를 사용해 사운드의 볼륨을 조절할 수 있습니다.

    private void Awake()
    {
        // 싱글턴 인스턴스를 설정
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

    // 배경 음악을 시작하는 메서드
    public void PlayMusic(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < musicClips.Length)
        {
            musicSource.clip = musicClips[clipIndex];
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("음악 인덱스가 유효하지 않습니다.");
        }
    }

    // 배경 음악을 멈추는 메서드
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // 사운드 효과를 재생하는 메서드
    public void PlaySoundEffect(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < soundEffects.Length)
        {
            effectsSource.PlayOneShot(soundEffects[clipIndex]);
        }
        else
        {
            Debug.LogWarning("효과음 인덱스가 유효하지 않습니다.");
        }
    }

    // 배경 음악의 볼륨을 설정하는 메서드
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    // 효과음의 볼륨을 설정하는 메서드
    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("EffectsVolume", volume);
    }

    // 모든 사운드를 일시 정지하는 메서드
    public void PauseAllSounds()
    {
        musicSource.Pause();
        effectsSource.Pause();
    }

    // 모든 사운드를 다시 시작하는 메서드
    public void ResumeAllSounds()
    {
        musicSource.UnPause();
        effectsSource.UnPause();
    }
}
