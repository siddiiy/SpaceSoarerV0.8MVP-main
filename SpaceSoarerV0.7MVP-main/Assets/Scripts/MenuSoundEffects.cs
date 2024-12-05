using UnityEngine;

public class MenuSoundEffects : MonoBehaviour
{
    public AudioClip hoverSound;  // Sound for hovering over buttons
    public AudioClip clickSound;  // Sound for clicking buttons
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHoverSound()
    {
        audioSource.PlayOneShot(hoverSound);
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
}
