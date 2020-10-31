using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace My {
  public class BGMManager : MonoBehaviour {
    [System.Serializable]
    public class AudioClipInfo {
      public string name;
      public AudioClip clip;
    }
    /*****singleton*****/
    public static BGMManager instance {
      get {
        if (m_instance != null)
          return m_instance;
        m_instance = FindObjectOfType<BGMManager>();
        if (m_instance != null)
          return m_instance;
        return null;
      }
    }
    protected static BGMManager m_instance;
    /**********/
    public int volume {
      get { return mVolume; }
      set {
        mVolume = Mathf.Clamp(value, 0, 100);
        m_audioSource.volume = mVolume / 100f;
        PlayerPrefs.SetInt("BGMVolume", mVolume);
      }
    }
    public float volumef {
      get { return mVolume / 100f; }
      set {
        mVolume = (int)(Mathf.Clamp(value, 0, 1f) * 100f);
        m_audioSource.volume = Mathf.Clamp(value, 0, 1f);
        PlayerPrefs.SetInt("BGMVolume", mVolume);
      }
    }
    protected int mVolume;
    public string　audioName{get{return m_audioName;}}    
    [SerializeField] protected AudioSource m_audioSource;
    [SerializeField] protected string m_audioName;
    [SerializeField] protected int m_listIndex = -1;
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
      //m_clipList.Clear();

      volume = PlayerPrefs.GetInt("BGMVolume", 40);
    }
    public void Play(AudioClip clip) {
      if (clip && clip != m_audioSource.clip) {
        m_audioSource.clip = clip;
        int i = 0;
        foreach (var info in m_clipList) {
          if (info.clip == clip) {
            m_audioName = info.name;
            m_listIndex = i;
            break;
          }
          i++;
        }
        if (i == m_clipList.Count()) {
          m_audioName = "NULL";
          m_listIndex = -1;
        }
        m_audioSource.Play();
      }
    }
    public void Play(string clipName) {
      if (m_clipDictionary.ContainsKey(clipName)) {
        Play(m_clipDictionary[clipName]);
      }
    }
    public void Replay(AudioClip clip) {
      if (clip) {
        m_audioSource.clip = clip;
        m_audioSource.Play();
      }

    }
    public void Replay(string clipName) {
      if (m_clipDictionary.ContainsKey(clipName))
        Replay(m_clipDictionary[clipName]);
    }

    public void PlayNext() {
      m_listIndex = Mathf.Clamp(m_listIndex + 1, 0, m_clipList.Count());
      Play(m_clipList[m_listIndex].clip);
    }
    public void PlayBack() {
      m_listIndex = Mathf.Clamp(m_listIndex - 1, 0, m_clipList.Count());
      Play(m_clipList[m_listIndex].clip);
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