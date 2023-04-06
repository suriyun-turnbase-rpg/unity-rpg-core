using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class BaseDamage : MonoBehaviour
{
    private Transform _tempTransform;
    public Transform TempTransform
    {
        get
        {
            if (_tempTransform == null)
                _tempTransform = GetComponent<Transform>();
            return _tempTransform;
        }
    }
    private Rigidbody _tempRigidbody;
    public Rigidbody TempRigidbody
    {
        get
        {
            if (_tempRigidbody == null)
                _tempRigidbody = GetComponent<Rigidbody>();
            return _tempRigidbody;
        }
    }

    public abstract void Setup(
        BaseCharacterEntity attacker,
        BaseCharacterEntity target,
        int seed,
        float pAtkRate,
        float mAtkRate,
        int hitCount,
        int fixDamage,
        bool fromCounter,
        bool useSkillHitChance,
        float skillHitChance);
}
