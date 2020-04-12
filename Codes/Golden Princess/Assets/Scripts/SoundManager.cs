using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static AudioSource audioSrc;
    public static AudioClip ButtonBackSound, ButtonSelectedSound, ButtonChangeSound,
        ChickenDeadSound, DashSound, EggCollectedSound, HurtSound, ParrySound, ShieldSound;

    void Start ()
    {
        ButtonBackSound = Resources.Load<AudioClip>("Sound Effects/ButtonBack");
        ButtonChangeSound = Resources.Load<AudioClip>("Sound Effects/ButtonChange");
        ButtonSelectedSound = Resources.Load<AudioClip>("Sound Effects/ButtonSelected");
        ChickenDeadSound = Resources.Load<AudioClip>("Sound Effects/ChickenDead");
        DashSound = Resources.Load<AudioClip>("Sound Effects/Dash");
        EggCollectedSound = Resources.Load<AudioClip>("Sound Effects/EggCollected");
        HurtSound = Resources.Load<AudioClip>("Sound Effects/Hurt");
        ParrySound = Resources.Load<AudioClip>("Sound Effects/Parry");
        ShieldSound = Resources.Load<AudioClip>("Sound Effects/Shield");

        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip clip)
    {
        audioSrc.PlayOneShot(clip);
    }
}
