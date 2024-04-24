using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class StaticEventHandler
{
    public static event Action<PlayerAttackEventArgs> OnPlayerAttack;

    public static void CallPlayerAttackEvent(AttackDetailsSO _attack)
    {
        OnPlayerAttack?.Invoke(new PlayerAttackEventArgs() {attack = _attack});
    }

}

public class PlayerAttackEventArgs : EventArgs
{
    public AttackDetailsSO attack;
}
