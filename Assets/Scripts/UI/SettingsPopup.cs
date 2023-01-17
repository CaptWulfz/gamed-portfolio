using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : Popup
{
    [Header("Volume Sliders")]
    [SerializeField] UISlider musicSlider;
    [SerializeField] UISlider sfxSlider;

    [Header("Toggles")]
    [SerializeField] Toggle muteToggle;

    private void OnEnable()
    {
        // This Function should contain initializiations of the popup values based on the Audio Manager used
        //this.musicSlider.Value = AudioManager.Instance.GetAudioGroupVolume(AudioKeys.MUSIC, true);
        //this.sfxSlider.Value = AudioManager.Instance.GetAudioGroupVolume(AudioKeys.SFX, true);

        //this.muteToggle.isOn = AudioManager.Instance.IsMute;
        ToggleSliderActiveStates();
    }

    private void ToggleSliderActiveStates()
    {
        this.musicSlider.ToggleActiveState(!this.muteToggle.isOn);
        this.sfxSlider.ToggleActiveState(!this.muteToggle.isOn);
    }

    #region Slider On Value Changed
    public void OnMusicVolumeSliderValueChanged()
    {
        //AudioManager.Instance.SetAudioGroupVolume(AudioKeys.MUSIC, this.musicSlider.Value);
    }

    public void OnSFXVolumeSliderValueChanged()
    {
        //AudioManager.Instance.SetAudioGroupVolume(AudioKeys.SFX, this.sfxSlider.Value);
    }
    #endregion

    #region
    public void OnMuteToggleValueChanged()
    {
        //AudioManager.Instance.SetGlobalMute(this.muteToggle.isOn);
        ToggleSliderActiveStates();
    }
    #endregion
}
