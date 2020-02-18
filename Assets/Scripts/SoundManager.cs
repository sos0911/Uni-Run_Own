using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
	public AudioSource efxSource; // jump and die
	public AudioSource musicSource;
	public static SoundManager instance = null; // singleton

	private float volval = 1f; // 처음엔 볼륨 max

	// Use this for initialization
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		// scene이 넘어가도 이 사운드매니저는 제거되지 않음
		DontDestroyOnLoad(gameObject);
	}

	/// <summary>
	/// 이벤트 발생시 efxsource 재생
	/// </summary>
	/// <param name="clip"></param>
	public void PlaySingle(AudioClip clip)
	{
		efxSource.clip = clip;
		efxSource.Play();
	}
}
