using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My {
  public class SEManager : MonoBehaviour {
    [System.Serializable]
    public class AudioClipInfo {
      public string name;
      public AudioClip clip;
    }
    /*****singleton*****/
    public static SEManager instance {
      get {
        if (m_instance != null)
          return m_instance;
        m_instance = FindObjectOfType<SEManager>();
        if (m_instance != null)
          return m_instance;
        return null;
      }
    }
    protected static SEManager m_instance;
    /**********/
    public int volume{
      get { return mVolume; }
      set{
        mVolume = Mathf.Clamp(value, 0, 100);
        m_audioSource.volume = mVolume / 100f;
        PlayerPrefs.SetInt("SEVolume", mVolume);
      }
    }
    public float volumef {
      get { return mVolume/100f; }
      set {
        mVolume = (int)(Mathf.Clamp(value, 0, 1f) * 100f);
        m_audioSource.volume = Mathf.Clamp(value, 0, 1f);
        PlayerPrefs.SetInt("SEVolume", mVolume);
      }
    }
    protected int mVolume;
    [SerializeField] protected AudioSource m_audioSource;
    [SerializeField] protected List<AudioClipInfo> m_clipList;
    protected Dictionary<string, AudioClip> m_clipDictionary = new Dictionary<string, AudioClip>();
    
    void Awake() {
      if (instance != this) {
        Destroy(gameObject);
        return;
      }
      DontDestroyOnLoad(gameObject);

      foreach (var i in m_clipList)
        m_clipDictionary[i.name] = i.clip;
      m_clipList.Clear();
      volume = PlayerPrefs.GetInt("SEVolume", 40);
    }
    
    public void Play(AudioClip clip) {
      if (clip)
        m_audioSource.PlayOneShot(clip);
    }
    public void Play(string clipName) {
      if (m_clipDictionary.ContainsKey(clipName))
        Play(m_clipDictionary[clipName]);
    }
    public void Stop() {
      m_audioSource.Stop();
    }
    public void Pause() {
      m_audioSource.Pause();
    }
    public void Resume() {
      m_audioSource.UnPause();
    }
    public void Mute() {
      m_audioSource.mute = true;
    }
    public void UnMute() {
      m_audioSource.mute = false;
    }
  }
}