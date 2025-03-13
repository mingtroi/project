using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton để gọi từ bất kỳ đâu

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource; // Nhạc nền
    [SerializeField] private AudioSource sfxSource; // Hiệu ứng âm thanh

    [Header("Audio Clips")]
    [SerializeField] private AudioClip bgm; // Nhạc nền chính
    [SerializeField] private AudioClip arrowShoot; // Tiếng bắn cung
    [SerializeField] private AudioClip enemyHit; // Tiếng trúng đích
    [SerializeField] private AudioClip explosion; // Tiếng nổ khi quái chết

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        PlayBGM(); // Tự động phát nhạc nền khi game bắt đầu
    }

    public void PlayBGM()
    {
        if (bgmSource != null && bgm != null)
        {
            bgmSource.clip = bgm;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
