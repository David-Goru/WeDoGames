using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] Slider soundsSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] AudioMixer soundsSource;
    AudioSource musicSource;

    void Start()
    {
        //musicSource = GameObject.Find("Music").GetComponent<AudioSource>();
                
        if (PlayerPrefs.HasKey("SoundsVolume"))
        {
            float soundVolume = PlayerPrefs.GetFloat("SoundsVolume");
            soundsSlider.value = soundVolume;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeMusicVolume()
    {
        //musicSource.volume = musicSlider.value;
    }

    public void ChangeSoundsVolume()
    {
        PlayerPrefs.SetFloat("SoundsVolume", soundsSlider.value);
        soundsSource.SetFloat("Volume", soundsSlider.value == 0 ? -100 : Mathf.Log10(soundsSlider.value) * 20);
    }
}