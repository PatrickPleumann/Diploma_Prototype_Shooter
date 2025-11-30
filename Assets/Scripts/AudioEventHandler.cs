using UnityEngine;

public class AudioEventHandler : MonoBehaviour
{
    [SerializeField] private AudioClip gunshot;
    AudioSource source;
    private void OnEnable()
    {
        EventHandler.onWeaponShoot += PlayShootSound;
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void PlayShootSound()
    {
        source.clip = gunshot;
        source.Play();
    }
}
