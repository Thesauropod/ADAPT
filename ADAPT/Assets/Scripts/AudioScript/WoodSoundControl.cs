using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSoundControl : MonoBehaviour
{
    public GameObject Target;
    public bool Control;

    int Timer;

    FMOD.Studio.EventInstance WoodSound;

    // Start is called before the first frame update
    void Start()
    {

        //WoodSound = FMODUnity.RuntimeManager.CreateInstance("event:/MainCharacter/WoodBreak");


    }

    // Update is called once per frame
    void Update()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(WoodSound, GetComponent<Transform>(), GetComponent<Rigidbody2D>());

        Control = Target.GetComponent<BreakableWall>().isDestroyed;
        if (Control == true && Timer==0)
        {
            
            FMODUnity.RuntimeManager.PlayOneShot("event:/MainCharacter/WoodBreak", GetComponent<Transform>().position);
            WoodSound.start();
            Timer++;


        }


    }
}
