using UnityEngine;

public class Preferences : MonoBehaviour
{
    /*
     * Fields
     */

    private const int AnimationEffectDefault = 2;
    private const int SoundEffectDefault = 0;

    private const string AnimationEffectKey = "AnimationEffect";
    private const string SoundEffectKey = "SoundEffect";

    /*
     * Enums
     */

    public enum AnimationEffect
    {
        None,
        Simple,
        Complete
    }

    public enum SoundEffect
    {
        Off,
        On
    }

    /*
     * Properties
     */

    public AnimationEffect Animation { get; set; }

    public SoundEffect Sound { get; set; }

    /*
     * Methods
     */

    public void Start()
    {
        // Read values from player preferences
        this.Animation = (AnimationEffect)PlayerPrefs.GetInt(AnimationEffectKey, AnimationEffectDefault);
        this.Sound = (SoundEffect)PlayerPrefs.GetInt(SoundEffectKey, SoundEffectDefault);
    }
}
