using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmatusSoundControl : MonoBehaviour
{

    public GameObject Target;
    public bool Control;

    FMOD.Studio.EventInstance RockSound;

    // Start is called before the first frame update
    void Start()
    {
        RockSound = FMODUnity.RuntimeManager.CreateInstance("event:/Rocks/RockRock");
    }

    // Update is called once per frame
    void Update()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(RockSound, GetComponent<Transform>(), GetComponent<Rigidbody2D>());


        Control = Target.GetComponent<Vision>().IsOn;
        if (Control == false)
        {

           RockSound.start();
        }
    }
}
