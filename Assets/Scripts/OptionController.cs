using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    /*
     * Fields
     */

    private const int AnimationEffectDefault = 2;
    private const int SoundEffectDefault = 0;

    private const string AnimationEffectKey = "AnimationEffect";
    private const string SoundEffectKey = "SoundEffect";

    private Slider animationSlider, soundSlider;
    private Text animationText, soundText;

    /*
     * Methods
     */

    /// <summary>
    /// Apply button (save changes)
    /// </summary>
    public void ApplyButton()
    {
        SaveSettings();
    }

    /// <summary>
    /// Cancel button (don't save any changes)
    /// </summary>
    public void CancelButton()
    {
        ReturnToMain();
    }

    /// <summary>
    /// Update text value on Animation Slider
    /// </summary>
    public void UpdateAnimationSlider()
    {
        Preferences.AnimationEffect option = (Preferences.AnimationEffect)animationSlider.value;
        animationText.text = option.ToString();
    }

    /// <summary>
    /// Update text value on Sound Slider
    /// </summary>
    public void UpdateSoundSlider()
    {
        Preferences.SoundEffect option = (Preferences.SoundEffect)soundSlider.value;
        soundText.text = option.ToString();
    }

    private void Start()
    {
        // Animation effects
        animationSlider = GameObject.Find("Animation/Slider").GetComponent<Slider>();
        animationText = GameObject.Find("Animation/Value").GetComponent<Text>();

        // Sound effects
        soundSlider = GameObject.Find("Sound/Slider").GetComponent<Slider>();
        soundText = GameObject.Find("Sound/Value").GetComponent<Text>();

        // Read settings from player preferences
        ReadSettings();
    }

    /// <summary>
    /// Read settings from PlayerPrefs and update UI
    /// </summary>
    private void ReadSettings()
    {
        // Read values from player preferences
        int animation = PlayerPrefs.GetInt(AnimationEffectKey, AnimationEffectDefault);
        int sound = PlayerPrefs.GetInt(SoundEffectKey, SoundEffectDefault);

        // Update slider value
        animationSlider.value = animation;
        UpdateAnimationSlider();

        // Update slider value
        soundSlider.value = sound;
        UpdateSoundSlider();
    }

    /// <summary>
    /// Save settings to PlayerPrefs
    /// </summary>
    private void SaveSettings()
    {
        // Read values from sliders
        int animation = (int)animationSlider.value;
        int sound = (int)soundSlider.value;

        // Save settings on player preferences
        PlayerPrefs.SetInt(AnimationEffectKey, animation);
        PlayerPrefs.SetInt(SoundEffectKey, sound);
        PlayerPrefs.Save();

        // Go back to main menu
        ReturnToMain();
    }

    /// <summary>
    /// Go to Main Scene
    /// </summary>
    private void ReturnToMain()
    {
        Application.LoadLevel("Main");
    }
}
