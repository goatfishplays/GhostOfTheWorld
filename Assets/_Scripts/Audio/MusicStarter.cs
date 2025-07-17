using System.Collections;
using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    AudioManager am => AudioManager.instance;
    public int curTrack = 0;
    public AudioClip[] tracks;


    private void Start()
    {
        StartCoroutine(CycleSong());
    }

    private IEnumerator CycleSong()
    {
        while (true)
        {
            am.FadeInMusic(tracks[curTrack]);
            yield return new WaitForSeconds(tracks[curTrack].length - 3f);
            curTrack = Random.Range(0, tracks.Length);
        }
    }
}
