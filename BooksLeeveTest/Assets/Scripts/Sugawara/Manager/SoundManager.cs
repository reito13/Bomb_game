using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    private static SoundManager instance; //SoundManagerのインスタンス	

	//----------------------------------------------------------------------------------
	[System.Serializable]
	class SEClass
	{
		public AudioClip seClip = null;
		public string seKey = null;
	}
	[SerializeField]
	SEClass[] seClass = new SEClass[0];
	public Dictionary<string, AudioClip> seDate = new Dictionary<string, AudioClip>();

	//----------------------------------------------------------------------------------
	//----------------------------------------------------------------------------------
	[System.Serializable]
	class BGMClass
	{
		public AudioClip bgmClip = null;
		public string bgmKey = null;
	}
	[SerializeField]
	BGMClass[] bgmClass = new BGMClass[0];
    public Dictionary<string, AudioClip> bgmDate = new Dictionary<string, AudioClip>();

    [System.NonSerialized] public AudioSource seAudiosource,bgmAudiosource;
    AudioClip nullClip;
	//----------------------------------------------------------------------------------

	public static SoundManager Instance
    {
        get
        {
            if (null == instance)
            {
                instance = (SoundManager)FindObjectOfType(typeof(SoundManager));
                if (null == instance)
                {
                    Debug.Log(" SoundManager Instance Error ");
                }
            }
            return instance;
        }
    }

    void Awake()
    {
		if (transform.Find("SEManager") && transform.Find("BGMManager"))
		{
			seAudiosource = transform.Find("SEManager").gameObject.GetComponent<AudioSource>();
			bgmAudiosource = transform.Find("BGMManager").gameObject.GetComponent<AudioSource>();
		}
		else
		{
			Debug.LogError("[SEManager]、もしくは[BGMManager]がありません。");
		}

		if (tag != "SoundManager")
		{
			Debug.Log("タグ”SoundManager”がありません。");
			return;

		}
		GameObject[] obj = GameObject.FindGameObjectsWithTag("SoundManager");
        if (1 < obj.Length)
        {
            // 既に存在しているなら削除
            Destroy(gameObject);
        }
        else
        {
            // シーン遷移では破棄させない
            DontDestroyOnLoad(gameObject);
        }

        for (int i = 0; i < seClass.Length; i++)
        {
            seDate.Add(seClass[i].seKey, seClass[i].seClip);
            //Debug.Log(seDate[seKey[i]].name);
        }

		for (int i = 0; i < bgmClass.Length; i++)
		{
			bgmDate.Add(bgmClass[i].bgmKey, bgmClass[i].bgmClip);
			//Debug.Log(bgmDate[bgmKey[i]].name);
		}

	}
    void Start()
    {

    }

    public void PlaySE(string seKey)
    {
        if (seDate.TryGetValue(seKey, out nullClip))
        {
            seAudiosource.PlayOneShot(seDate[seKey]);
        } else
        {
            //Debug.Log(seKey+"に対応したSEはありません");
        }
    }

    public void PlayBGM(string bgmKey)
    {
        if (bgmDate.TryGetValue(bgmKey, out nullClip))
        {
			bgmAudiosource.clip = nullClip;
            bgmAudiosource.Play();
        }
        else
        {
           // Debug.Log("に対応したBGMはありません");
        }
    }

    //辻野追加
    public void StopBGM(string bgmKey)
    {
        if (bgmDate.TryGetValue(bgmKey, out nullClip))
        {
            bgmAudiosource.clip = nullClip;
            bgmAudiosource.Stop();
        }
        else
        {
            // Debug.Log("に対応したBGMはありません");
        }
    }

}

