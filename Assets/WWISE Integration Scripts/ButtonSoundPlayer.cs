using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundPlayer : MonoBehaviour
{
    [SerializeField] AK.Wwise.Event eventToPlay;
    [SerializeField] GameObject gameObjectToPlayOn;


    public void OnClicked()
    {
        AkSoundEngine.PostEvent(eventToPlay.Name, gameObjectToPlayOn == null ? gameObject : gameObjectToPlayOn);
    }
}
