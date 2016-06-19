using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class unitSounds : MonoBehaviour {
	
	public AudioClip[] clipsFire;
	public AudioClip[] clipsEmpty;
	public AudioClip[] clipsSelected;
	public AudioClip[] clipsWin;
	public AudioClip[] clipsLose;
	public AudioClip[] clipsDead;
	
	private Dictionary<string, AudioSource[]> sounds;
	
	public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol) { 
		AudioSource newAudio = gameObject.AddComponent<AudioSource>();
		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol; 
		return newAudio; 
	}
	
	public void AddClips(string name, ref AudioClip[] clips) {
		AudioSource[] sources = new AudioSource[clips.Length];
		for (int i = 0; i < clips.Length; ++i) {
			sources [i] = AddAudio (clips [i], false, false, 1.0f);
		}
		try {
			sounds[name] = sources;
		} catch(UnityException) {
			Debug.Log ("Key already in sounds: "+name);
		}
	}

	void Awake () {
		sounds = new Dictionary<string, AudioSource[]> ();
	}

	// Use this for initialization
	void Start () {
		AddClips ("Fire", ref clipsFire);
		AddClips ("Empty", ref clipsEmpty);
		AddClips ("Selected", ref clipsSelected);
		AddClips ("Win", ref clipsWin);
		AddClips ("Lose", ref clipsLose);
		AddClips ("Dead", ref clipsDead);
	}
	
	public int Play(string name) {
		int length = 0;
		try {
			AudioSource[] array = sounds[name];
			if (array != null && array.Length > 0) {
				//				Debug.Log ("Playing sound \""+name+"\"");
				int i = Random.Range(0, array.Length);
				array[i].Play();
				length = (int)array[i].clip.length;
			}
		} catch(KeyNotFoundException) {
			Debug.Log ("Key not found in sounds: "+name);
		}
		return length;
	}
}
