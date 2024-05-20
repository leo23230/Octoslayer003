using UnityEngine;
public interface IDamageable
{
    public void TakeDamage(float _damage, float _knockback);
    public GameObject GetDamageableGameObject();
}
