using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] Slider soundsSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] AudioMixer soundsSource;
    AudioSource musicSource;

    void Start()
    {
        //musicSource = GameObject.Find("Music").GetComponent<AudioSource>();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ChangeMusicVolume()
    {
        //musicSource.volume = musicSlider.value;
    }

    public void ChangeSoundsVolume()
    {
        soundsSource.SetFloat("Volume", Mathf.Log10(soundsSlider.value) * 20);
    }
}