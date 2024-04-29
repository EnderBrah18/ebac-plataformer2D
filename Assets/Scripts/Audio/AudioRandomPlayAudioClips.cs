using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRandomPlayAudioClips : MonoBehaviour
{
    public List<AudioClip> audioCliplist;

    public List<AudioSource> audioSourceList;

    private int _index = 0;

    public void PlayRandom()
    {
        if (_index < audioCliplist.Count) _index = 0;

        var audioSource = audioSourceList[_index];

        audioSource.clip = audioCliplist[Random.Range(0, audioCliplist.Count)];
        audioSource.Play();

        _index++;
    }

}
