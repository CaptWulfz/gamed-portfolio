using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class FMODAudioManager : Singleton<FMODAudioManager>, ISingleton
{
    public enum AudioType
    {
        MASTER,
        MUSIC,
        SFX
    }

    private Bus Master;
    private Bus Music;
    private Bus SFX;

    private bool isDone = false;
    public bool IsDone
    {
        get { return this.isDone; }
    }

    public void Initialize()
    {
        this.Master = RuntimeManager.GetBus("bus:/");
        //this.Music = RuntimeManager.GetBus("bus:/Master/Music");
        //this.SFX = RuntimeManager.GetBus("bus:/Master/SFX");

        this.isDone = true;
    }

    public void SetVolume(AudioType type, float volume)
    {
        Bus bus = GetAudioBus(type);
        bus.setVolume(volume);
    }

    public float GetVolume(AudioType type)
    {
        Bus bus = GetAudioBus(type);
        float volume = 1;
        bus.getVolume(out volume);

        return volume;
    }

    #region Utils
    private Bus GetAudioBus(AudioType type)
    {
        Bus bus = this.Master;

        switch (type)
        {
            case AudioType.MASTER:
                bus = this.Master;
                break;
            case AudioType.MUSIC:
                bus = this.Music;
                break;
            case AudioType.SFX:
                bus = this.SFX;
                break;
        }

        return bus;
    }
    #endregion
}
