using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource gameMusic0;
    public AudioSource gameMusic1;
    public AudioSource clubMusic;

    [Header("Clips")]
    public AudioClip currentClubTrack;
    public AudioClip playerSpottedTrack0;
    public AudioClip bossFightMusic;
    public List<AudioClip> clubTrackList = new List<AudioClip>();

    float maxVolume;

    private void OnEnable()
    {
        StaticEventHandler.OnPlayerSpotted += PlayerSpottedMusic;
    }
    private void OnDisable()
    {
        StaticEventHandler.OnPlayerSpotted -= PlayerSpottedMusic;
    }

    public void SwitchMusic(AudioClip _clip)
    {
        //which ever source is playing will be the source that is faded out
        if (clubMusic.isPlaying)
        {
            SwitchAudioClip(gameMusic0, _clip);
            maxVolume = gameMusic0.volume;
            gameMusic0.volume = 0;
            gameMusic0.Play();
            StartCoroutine(CrossFade(clubMusic, gameMusic0, 0.5f));
        }
        else if (gameMusic0.isPlaying || gameMusic1.isPlaying)
        {
            SwitchSoundtrack(_clip);
        }
    }
    public void SwitchSoundtrack(AudioClip _clip)
    {
        if (gameMusic0.isPlaying)
        {
            SwitchAudioClip(gameMusic1, _clip);
            maxVolume = gameMusic1.volume;
            gameMusic1.volume = 0;
            gameMusic1.Play();
            StartCoroutine(CrossFade(gameMusic0, gameMusic1, 0.5f));
        }
        else if (gameMusic1.isPlaying)
        {
            SwitchAudioClip(gameMusic0, _clip);
            maxVolume = gameMusic0.volume;
            gameMusic0.volume = 0;
            gameMusic0.Play();
            StartCoroutine(CrossFade(gameMusic1, gameMusic0, 0.5f));
        }
    }

    private void SwitchAudioClip(AudioSource _source, AudioClip _clip)
    {
        if(_source.clip != _clip)
        {
            _source.clip = _clip;
        }
    }
    
    private IEnumerator CrossFade (AudioSource _sourceOut, AudioSource _sourceIn, float _durationScale)
    {

        while (_sourceOut.volume > 0)
        {
            //Debug.Log(maxVolume);
            _sourceOut.volume -= 0.01f * _durationScale;
            //only want to raise source volume to the volume set in editor
            if(_sourceIn.volume < 0.85f)_sourceIn.volume += 0.01f * _durationScale;

            yield return null;
        }
        _sourceOut.Stop();
        yield return null;
    }

    public void PlayerSpottedMusic(PlayerSpottedEventArgs eventArgs)
    {
        SwitchMusic(playerSpottedTrack0);
    }
    public void PlayClubMusic()
    {
        SwitchMusic(currentClubTrack);
    }
}
