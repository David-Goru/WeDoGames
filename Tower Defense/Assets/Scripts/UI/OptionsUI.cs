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
    [SerializeField] AudioMixerGroup musicSource;
    [SerializeField] GameObject postProcessingObject;
    [SerializeField] Toggle postProcessing;
    [SerializeField] Text fpsCounter;

    int frameCount = 0;
    float deltaTime = 0.0f;
    float fps = 0.0f;
    float updatesPerSecond = 5.0f;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float masterVolume = PlayerPrefs.GetFloat("MasterVolume");
            masterSlider.value = masterVolume;
        }
        masterSource.audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterSlider.value) * 20);

        if (PlayerPrefs.HasKey("SoundsVolume"))
        {
            float soundVolume = PlayerPrefs.GetFloat("SoundsVolume");
            soundsSlider.value = soundVolume;
        }
        soundsSource.audioMixer.SetFloat("SoundsVolume", Mathf.Log10(soundsSlider.value) * 20);

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            musicSlider.value = musicVolume;
        }
        musicSource.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicSlider.value) * 20);

        if (PlayerPrefs.HasKey("PostProcessing"))
        {
            bool postProcessingOn = PlayerPrefs.GetInt("PostProcessing") == 1;
            postProcessing.isOn = postProcessingOn;
        }

        gameObject.SetActive(false);
    }

    void Update()
    {
        frameCount++;
        deltaTime += Time.deltaTime;
        if (deltaTime > 1.0 / updatesPerSecond)
        {
            fps = frameCount / deltaTime;
            frameCount = 0;
            deltaTime -= 1.0f / updatesPerSecond;
            fpsCounter.text = fps.ToString("0.00 FPS").Replace(",", ".");
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

    public void ChangeMasterVolume()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        masterSource.audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterSlider.value) * 20);
    }

    public void ChangeSoundsVolume()
    {
        PlayerPrefs.SetFloat("SoundsVolume", soundsSlider.value);
        soundsSource.audioMixer.SetFloat("SoundsVolume", Mathf.Log10(soundsSlider.value) * 20);
    }

    public void ChangeMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        musicSource.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicSlider.value) * 20);
    }

    public void UpdatePostProcessing()
    {
        postProcessingObject.SetActive(postProcessing.isOn);
        PlayerPrefs.SetInt("PostProcessing", postProcessing.isOn ? 1 : 0);
    }
}