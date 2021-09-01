using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundsSlider;
    AudioSource musicSource;
    AudioSource soundsSource;

    void Start()
    {
        //musicSource = GameObject.Find("Music").GetComponent<AudioSource>();
        soundsSource = GameObject.Find("Camera").GetComponent<AudioSource>();
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
        soundsSource.volume = soundsSlider.value;
    }
}