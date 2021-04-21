using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSoundControl : MonoBehaviour
{
    public GameObject LaserTarget;
    public bool LaserControl;

    FMOD.Studio.EventInstance LaserSound;

    private void Awake()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        LaserSound = FMODUnity.RuntimeManager.CreateInstance("event:/Environment/Laser");
    }

    // Update is called once per frame
    void Update()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(LaserSound, GetComponent<Transform>(), GetComponent<Rigidbody2D>());


        LaserControl = LaserTarget.GetComponent<Laser>().IsOn;
        if (LaserControl == false) {
            
            LaserSound.start();
        }

    }
}
