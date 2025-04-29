using UnityEngine;

public class StartMove : MonoBehaviour
{
    [SerializeField] private LinearMovement _target;
    [SerializeField] private ParticleSystem _eraseParticles;

    private void Start()
    {
        _target.StartMovement();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("stop_move"))
        {
            _target.StopMovement();
        }
     
    }

    private void PlayEraseParticles(Vector3 position)
    {
        if (_eraseParticles != null)
        {
            ParticleSystem particles = Instantiate(_eraseParticles, position, Quaternion.identity);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration);
        }
    }
}