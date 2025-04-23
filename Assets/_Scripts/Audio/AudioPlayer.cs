using System.Collections;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public Transform tracker;
    public Coroutine co_audioPlaying = null;

    private void Update()
    {
        if (tracker != null)
        {
            transform.SetPositionAndRotation(tracker.position, tracker.rotation);
        }
    }

    public void PlayAudioAtTracker(AudioClip clip, Transform tracker, float volume = 1f, float pitch = 1f, bool loop = false)
    {
        this.tracker = tracker;
        transform.SetPositionAndRotation(tracker.position, tracker.rotation);
        PlayAudio(clip, volume, pitch, loop);
    }

    public void PlayAudioAtPoint(AudioClip clip, Vector3 pos, Quaternion rot, float volume = 1f, float pitch = 1f, bool loop = false)
    {
        transform.position = pos;
        transform.rotation = rot;
        PlayAudio(clip, volume, pitch, loop);
    }

    public void PlayAudio(AudioClip clip, float volume = 1f, float pitch = 1f, bool loop = false)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;

        if (co_audioPlaying != null)
        {
            StopCoroutine(co_audioPlaying);
        }

        audioSource.Play();
        if (!loop)
        {
            co_audioPlaying = StartCoroutine(Playing(clip.length));
        }
    }


    public void Pause()
    {
        if (co_audioPlaying != null)
        {
            StopCoroutine(co_audioPlaying);
        }
        audioSource.Pause();
    }

    public void UnPause()
    {
        audioSource.UnPause();
        if (!audioSource.loop)
        {
            co_audioPlaying = StartCoroutine(Playing(audioSource.clip.length - audioSource.time));
        }
    }

    private IEnumerator Playing(float time)
    {
        yield return new WaitForSeconds(time + .1f);
        // reset and return 
        AudioManager.instance.FreeSFXPlayer(this);
        tracker = null;
        transform.parent = AudioManager.instance.transform;
    }


    private void OnDestroy()
    {
        // prevent mem leak 
        AudioManager.instance.RemoveSFXPlayer(this);
    }


}
