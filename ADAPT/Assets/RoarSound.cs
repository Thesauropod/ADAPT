using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoarSound : MonoBehaviour
{

    FMOD.Studio.EventInstance Roar;

    private void Start()
    {
        Roar = FMODUnity.RuntimeManager.CreateInstance("event:/MainCharacter/Roar");
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {

            playSound();

            //FMODUnity.RuntimeManager.PlayOneShot("event:/MainCharacter/Roar", transform.position); }

        }

        void playSound()
        {
            FMOD.Studio.PLAYBACK_STATE fmodpbState;
            Roar.getPlaybackState(out fmodpbState);
            if (fmodpbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                Roar.start();
            }

        }


    }
}
