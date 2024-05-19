using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class StaticEventHandler
{
    public static event Action<PlayerAttackEventArgs> OnPlayerAttack;
    public static event Action<PlayerSpottedEventArgs> OnPlayerSpotted;
    public static event Action<GrabCancelEventArgs> OnGrabCancel;
    public static event Action<BossFightStartedEventArgs> OnBossFightStarted;

    public static void CallPlayerAttackEvent(AttackDetailsSO _attack)
    {
        OnPlayerAttack?.Invoke(new PlayerAttackEventArgs() {attack = _attack});
    }
    public static void CallPlayerSpottedEvent(Vector3 _position)
    {
        OnPlayerSpotted?.Invoke(new PlayerSpottedEventArgs() {position = _position});
    }
    public static void CallGrabCancelEvent()
    {
        OnGrabCancel?.Invoke(new GrabCancelEventArgs() {});
    }
    public static void CallBossFightStartedEvent(AudioClip _music)
    {
        OnBossFightStarted?.Invoke(new BossFightStartedEventArgs() { music = _music });
    }
}

public class PlayerAttackEventArgs : EventArgs
{
    public AttackDetailsSO attack;
}
public class PlayerSpottedEventArgs : EventArgs
{
    public Vector3 position;
}
public class GrabCancelEventArgs : EventArgs
{
    
}
public class BossFightStartedEventArgs : EventArgs
{
    public AudioClip music;
}
