using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager singleton;
    
    [Header("Audio source reference")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    [Header("SFX List")]
    [SerializeField] private AudioClip[] sfxClips;

    [Header("Slider volume")]
    [SerializeField] private Transform sliderParent;
    [SerializeField] private Slider[] sliders;
    private bool isSliderUIActive = false;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    private void Update()
    {
        UpdateSliderVolume();
    }

    public void PlaySfx(int _clipIndex) => sfxSource.PlayOneShot(sfxClips[_clipIndex]);

    private void UpdateSliderVolume()
    {
        sfxSource.volume = sliders[0].value;
        bgmSource.volume = sliders[1].value;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isSliderUIActive = !isSliderUIActive;
            sliderParent.gameObject.SetActive(isSliderUIActive);

            if (isSliderUIActive)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
    }

}
