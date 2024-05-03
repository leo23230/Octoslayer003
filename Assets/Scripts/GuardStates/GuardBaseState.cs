using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GuardBaseState : MonoBehaviour
{
    public abstract void EnterState(GuardStateManager guard);

    public abstract void UpdateState(GuardStateManager guard);

}
