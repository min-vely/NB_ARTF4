using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Utility;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public Sound[] bgm;
    private Sound[] sfx = null;
    private AudioSource bgmPlay { get; set; }
    private readonly List<AudioSource> _sfxPlays = new List<AudioSource>();

    public void InitializedSound()
    {
        GameObject soundManager = Main.Resource.InstantiatePrefab("SoundManager.prefab");
        soundManager.name = "@SoundManager";
        SoundManager sm = SceneUtility.GetAddComponent<SoundManager>(soundManager);
        Main.Sound = sm;
        AudioClip clip = Main.Resource.Load<AudioClip>("LoadBGM1.clip");
        Main.Sound.StartBGM();
        Main.Sound.PlayBGM(clip.name);
    }

    public void StartBGM()
    {
        bgmPlay = gameObject.GetComponent<AudioSource>();
        AudioSource sfxPlayer = gameObject.AddComponent<AudioSource>();
        _sfxPlays.Add(sfxPlayer);
    }

    // 배경음악 재생
    public void PlayBGM(string bgmName)
    {
        foreach (var t in bgm)
        {
            Debug.Log(bgmName + bgm[0].name);
            if (bgmName != t.name) continue;
            bgmPlay.clip = t.clip;
            bgmPlay.Play();
            return;
        }
    }

    public void StopBGM()
    {
        bgmPlay.Stop();
    }

    // 효과음 재생
    public void PlaySFX(string sfxName, float volume = 0.5f)
    {
        foreach (var t in sfx)
        {
            if (sfxName != t.name) continue;
            AudioSource sfxPlay = AddSFXPlayer();
            sfxPlay.clip = t.clip;
            sfxPlay.volume = volume;
            sfxPlay.Play();
            return;
        }
    }

    private AudioSource AddSFXPlayer()
    {
        foreach (var t in _sfxPlays.Where(t => !t.isPlaying)) return t;

        // 모든 SFX 플레이어가 사용 중일 경우 새 플레이어 생성
        AudioSource newSFXPlay = gameObject.AddComponent<AudioSource>();
        _sfxPlays.Add(newSFXPlay);
        return newSFXPlay;
    }
}