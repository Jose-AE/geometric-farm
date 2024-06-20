using UnityEngine.Audio;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public static class AudioManager
{

    private static GameObject audioManager;

    private static AudioMixerGroup musicGroup;
    private static AudioMixerGroup sfxGroup;

    private static AudioMixer mixer;


    private static void Init()
    {
        if (audioManager == null)
        {
            mixer = Resources.Load<AudioMixer>("Audio/MainAudioMixer");
            musicGroup = mixer.FindMatchingGroups("Music")[0];
            sfxGroup = mixer.FindMatchingGroups("SFX")[0];


            audioManager = new GameObject("AudioManager");
            var au = audioManager.AddComponent<AudioSource>();
            au.loop = true;
            au.outputAudioMixerGroup = musicGroup;
            au.playOnAwake = false;

        }
    }



    public static AudioSource PlaySFX(AudioClip sfx, Transform location = null, float volume = 1f, bool loop = false)
    {
        Init();


        GameObject sfxSource = new GameObject("SFX");
        sfxSource.transform.SetParent(audioManager.transform);
        sfxSource.transform.position = location == null ? Vector3.zero : location.position;


        AudioSource audioSource = sfxSource.AddComponent<AudioSource>();
        audioSource.clip = sfx;
        audioSource.loop = loop;
        audioSource.spatialBlend = location == null ? 0f : 1f;
        audioSource.volume = volume;
        audioSource.outputAudioMixerGroup = sfxGroup;
        audioSource.Play();

        if (!loop)
            Object.Destroy(sfxSource, sfx.length);

        return audioSource;
    }





    public static void PlayMusic(AudioClip music)
    {
        Init();

        audioManager.GetComponent<AudioSource>().clip = music;
        audioManager.GetComponent<AudioSource>().Play();
    }



    public static void SetSfxGroupVolume(float vol)
    {
        Init();

        mixer.SetFloat("SfxVolume", vol);
    }


    public static void SetMusicGroupVolume(float vol)
    {
        Init();

        mixer.SetFloat("MusicVolume", vol);
    }


}