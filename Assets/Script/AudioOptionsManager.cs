using UnityEngine;
using TMPro;

public class AudioOptionsManager : MonoBehaviour {

    public static float generalVolume {get; private set; }
    public static float musicVolume {get; private set; }
    public static float soundEffectsVolume{get; private set; }

    [SerializeField] private TextMeshProUGUI generalSliderText;
    [SerializeField] private TextMeshProUGUI musicSliderText;
    [SerializeField] private TextMeshProUGUI soundEffectscSliderText;

    public void OnGeneralSliderValuechange(float value) {
        generalVolume = value;
        generalSliderText.text = ((int)(value *100)).ToString();
        AudioManager.Instance.UpdateMixerVolume();
    }
    
    public void OnMusicSliderValuechange(float value) {
        musicVolume = value;
        musicSliderText.text = ((int)(value *100)).ToString();
        AudioManager.Instance.UpdateMixerVolume();
    }

    public void OnSoundEffectsSliderValuechange(float value) {
        soundEffectsVolume = value;
        soundEffectscSliderText.text = ((int)(value *100)).ToString();
        AudioManager.Instance.UpdateMixerVolume();
    }

}
