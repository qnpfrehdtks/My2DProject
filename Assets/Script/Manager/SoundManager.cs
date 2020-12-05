
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    Dictionary<E_SFX, AudioClip> m_dicSound = new Dictionary<E_SFX, AudioClip>();
    Dictionary<E_BGM, AudioClip> m_dicBGM = new Dictionary<E_BGM, AudioClip>();

    AudioSource m_AudioSource;

    E_BGM m_currentPlayBGM = E_BGM.NONE;


    public override void InitializeManager()
    {
        if(m_AudioSource == null)
        {
            m_AudioSource =gameObject.AddComponent<AudioSource>();
        }

        AudioClip[] bgms = Resources.LoadAll<AudioClip>("Sounds/BGM");

        for(int i = 0; i < bgms.Length; i++)
        {
            string soundName = bgms[i].name;
            E_BGM bgm = (E_BGM)Enum.Parse(typeof(E_BGM), soundName);
#if UNITY_EDITOR
            Debug.Log(bgms[i].name + "Load Success");
#endif
            m_dicBGM.Add(bgm, bgms[i]);
        }

        AudioClip[] sounds = Resources.LoadAll<AudioClip>("Sounds/SFX");

        for (int i = 0; i < sounds.Length; i++)
        {
            string soundName = sounds[i].name;
            E_SFX sfx = (E_SFX)Enum.Parse(typeof(E_SFX), soundName);
#if UNITY_EDITOR
            Debug.Log(sounds[i].name + "Load Success");
#endif
            m_dicSound.Add(sfx, sounds[i]);
        }

        return;
    }

    public void StopBGM()
    {
        m_AudioSource.Stop();
        m_currentPlayBGM = E_BGM.NONE;
    }

    public void PlayBGM(E_BGM _bgm)
    {
        m_AudioSource.volume = 0.2f;

        if (m_currentPlayBGM == _bgm)
            return;

        AudioClip clip;
        if (m_dicBGM.TryGetValue(_bgm, out clip))
        {
            m_AudioSource.loop = true;
            m_AudioSource.clip = clip;
            m_AudioSource.Play();
            m_currentPlayBGM = _bgm;
        }
        else
        {
            StopBGM();
        }
    }

    public void PlaySFX(E_SFX _sfx)
    {
        AudioClip clip;
        if(m_dicSound.TryGetValue(_sfx, out clip))
        {
            m_AudioSource.PlayOneShot(clip);
        }
    }
}
