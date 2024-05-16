using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public void DoEffect(GameObject _effect, Vector3 _target, float _duration)
    {
        GameObject effect = Instantiate(_effect);
        effect.transform.position = _target;
        effect.transform.parent = transform.parent;
        StartCoroutine(DestroyEffect(_effect, _duration));
    }

    private IEnumerator DestroyEffect(GameObject _effectInstance, float _duration)
    {
        yield return new WaitForSeconds(_duration);
        if(_effectInstance != null) Destroy(_effectInstance);
    }
}
