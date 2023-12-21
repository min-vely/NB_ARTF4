using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public static SoundManager Instance;

    private void Awake()
    {
        Debug.Log("사운드매니저 초기화");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Sound[] bgm;
    private Sound[] sfx = null;
    public AudioSource bgmPlay { get; set; }
    private List<AudioSource> sfxPlays = new List<AudioSource>();



    public void StartBGM()
    {
        bgmPlay = gameObject.GetComponent<AudioSource>();
        AudioSource sfxPlayer = gameObject.AddComponent<AudioSource>();
        sfxPlays.Add(sfxPlayer);
    }

    // 배경음악 재생
    public void PlayBGM(string bgmName)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            Debug.Log(bgmName + bgm[0].name);
            if (bgmName == bgm[i].name)
            {
                bgmPlay.clip = bgm[i].clip;
                bgmPlay.Play();
                return;
            }
        }
    }

    public void StopBGM()
    {
        bgmPlay.Stop();
    }

    // 효과음 재생
    public void PlaySFX(string sfxName, float volume = 0.5f)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (sfxName == sfx[i].name)
            {
                AudioSource sfxPlay = AddSFXPlayer();
                sfxPlay.clip = sfx[i].clip;
                sfxPlay.volume = volume;
                sfxPlay.Play();
                return;
            }
        }
    }

    private AudioSource AddSFXPlayer()
    {
        for (int i = 0; i < sfxPlays.Count; i++)
        {
            if (!sfxPlays[i].isPlaying)
            {
                return sfxPlays[i];
            }
        }

        // 모든 SFX 플레이어가 사용 중일 경우 새 플레이어 생성
        AudioSource newSFXPlay = gameObject.AddComponent<AudioSource>();
        sfxPlays.Add(newSFXPlay);
        return newSFXPlay;
    }
}