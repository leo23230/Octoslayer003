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
    public List<AudioClip> clubTrackList = new List<AudioClip>();

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
            StartCoroutine(CrossFade(clubMusic, gameMusic0, 0.5f));
        }
        else if (gameMusic0.isPlaying)
        {
            SwitchAudioClip(clubMusic, _clip);
            StartCoroutine(CrossFade(gameMusic0, clubMusic, 0.5f));
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
        _sourceIn.volume = 0;
        _sourceIn.Play();
        while (_sourceIn.volume < 1)
        {
            _sourceOut.volume -= 0.01f * _durationScale;
            _sourceIn.volume += 0.01f * _durationScale;

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
