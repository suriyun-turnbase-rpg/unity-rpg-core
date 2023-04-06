using UnityEngine;

public class GameEffect : MonoBehaviour
{
    public bool isLoop;
    public float lifeTime;

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

    private ParticleSystem[] _particles;
    private AudioSource[] _audioSources;

    private void Awake()
    {
        _particles = GetComponentsInChildren<ParticleSystem>();
        foreach (var particle in _particles)
        {
            if (particle)
                particle.Play();
        }
        _audioSources = GetComponentsInChildren<AudioSource>();
        foreach (var audioSource in _audioSources)
        {
            if (audioSource)
                audioSource.Play();
        }
    }

    private void Start()
    {
        if (!isLoop)
            Destroy(gameObject, lifeTime);
    }

    public void DestroyEffect()
    {
        foreach (var particle in _particles)
        {
            if (particle)
            {
                var mainEmitter = particle.main;
                mainEmitter.loop = false;
            }
        }
        foreach (var audioSource in _audioSources)
        {
            if (audioSource)
                audioSource.loop = false;
        }
        Destroy(gameObject, lifeTime);
    }

    public GameEffect InstantiateTo(Transform parent, bool asChildren = true)
    {
        var effect = Instantiate(this, asChildren ? parent : null);
        effect.TempTransform.localPosition = Vector3.zero;
        effect.TempTransform.localEulerAngles = Vector3.zero;
        effect.TempTransform.localScale = Vector3.one;
        effect.gameObject.SetActive(true);
        return effect;
    }
}
