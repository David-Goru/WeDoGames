using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider soundsSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] AudioMixerGroup masterSource;
    [SerializeField] AudioMixerGroup soundsSource;
    AudioSource musicSource;

    void Start()
    {
        //musicSource = GameObject.Find("Music").GetComponent<AudioSource>();
                
        if (PlayerPrefs.HasKey("SoundsVolume"))
        {
            float soundVolume = PlayerPrefs.GetFloat("SoundsVolume");
            soundsSlider.value = soundVolume;
            soundsSource.audioMixer.SetFloat("Volume", Mathf.Log10(soundsSlider.value) * 20);
        }

        gameObject.SetActive(false);
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

    public void ChangeMasterVolume()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        masterSource.audioMixer.SetFloat("Volume", Mathf.Log10(masterSlider.value) * 20);
    }

    public void ChangeMusicVolume()
    {
        //musicSource.volume = musicSlider.value;
    }

    public void ChangeSoundsVolume()
    {
        PlayerPrefs.SetFloat("SoundsVolume", soundsSlider.value);
        soundsSource.audioMixer.SetFloat("Volume", Mathf.Log10(soundsSlider.value) * 20);
    }
}