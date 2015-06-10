using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    /*
     * Fields
     */

    private Slider animationSlider, soundSlider;
    private Text animationText, soundText;

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
     * Methods
     */

    public void ReturnToMain()
    {
        Application.LoadLevel("Main");
    }

    public void UpdateAnimationSlider()
    {
        AnimationEffect option = (AnimationEffect)animationSlider.value;
        animationText.text = option.ToString();
    }

    public void UpdateSoundSlider()
    {
        SoundEffect option = (SoundEffect)soundSlider.value;
        soundText.text = option.ToString();
    }

    private void Start()
    {
        // Animation effects
        animationSlider = GameObject.Find("Animation/Slider").GetComponent<Slider>();
        animationText = GameObject.Find("Animation/Value").GetComponent<Text>();
        UpdateAnimationSlider();

        // Sound effects
        soundSlider = GameObject.Find("Sound/Slider").GetComponent<Slider>();
        soundText = GameObject.Find("Sound/Value").GetComponent<Text>();
        UpdateSoundSlider();
    }
}
