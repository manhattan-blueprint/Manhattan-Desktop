using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Source used to play sounds.
    [SerializeField] public AudioSource soundSource;

    // Source used to play music.
    [SerializeField] public AudioSource musicSource;

    // Keeping track of player position for footsteps.
    [SerializeField] public GameObject player;
    private Vector3 playerLastPos;
    private Vector3 playerCurrentPos;
    private float playerTotalDistance;
    private float playerTotalTime;
    private float playerLastStep;

    // UI related sounds.
    private List<AudioClip> blueprintOpening = new List<AudioClip>();
    private List<AudioClip> bagOpening = new List<AudioClip>();
    private List<AudioClip> buttonPress = new List<AudioClip>();

    // Resource collection sounds.
    private List<AudioClip> chop = new List<AudioClip>();
    private List<AudioClip> shovel = new List<AudioClip>();
    private List<AudioClip> mine = new List<AudioClip>();
    private List<AudioClip> drip = new List<AudioClip>();

    // Background noise/music.
    private List<AudioClip> footsteps = new List<AudioClip>();
    private AudioClip birdsLooping;
    private AudioClip blueprintCollecting;
    private AudioClip mapView;

    private System.Random random;

    void Start()
    {
        random = new System.Random();
        musicSource.loop = true;

        playerTotalDistance = 0.0f;
        playerTotalTime = 0.0f;
        playerLastPos = player.transform.position;
        playerCurrentPos = player.transform.position;
        playerLastStep = 0.0f;

        blueprintOpening.Add(Resources.Load<AudioClip>("Sounds/BlueprintOpening/BlueprintOpening1"));
        blueprintOpening.Add(Resources.Load<AudioClip>("Sounds/BlueprintOpening/BlueprintOpening2"));

        bagOpening.Add(Resources.Load<AudioClip>("Sounds/BagOpening/BagOpening1"));
        bagOpening.Add(Resources.Load<AudioClip>("Sounds/BagOpening/BagOpening2"));
        bagOpening.Add(Resources.Load<AudioClip>("Sounds/BagOpening/BagOpening3"));
        bagOpening.Add(Resources.Load<AudioClip>("Sounds/BagOpening/BagOpening4"));

        buttonPress.Add(Resources.Load<AudioClip>("Sounds/buttonPress/buttonPress1"));
        buttonPress.Add(Resources.Load<AudioClip>("Sounds/buttonPress/buttonPress2"));
        buttonPress.Add(Resources.Load<AudioClip>("Sounds/buttonPress/buttonPress3"));
        buttonPress.Add(Resources.Load<AudioClip>("Sounds/buttonPress/buttonPress4"));
        buttonPress.Add(Resources.Load<AudioClip>("Sounds/buttonPress/buttonPress5"));

        chop.Add(Resources.Load<AudioClip>("Sounds/WoodChop/WoodChop1"));
        chop.Add(Resources.Load<AudioClip>("Sounds/WoodChop/WoodChop2"));
        chop.Add(Resources.Load<AudioClip>("Sounds/WoodChop/WoodChop3"));
        chop.Add(Resources.Load<AudioClip>("Sounds/WoodChop/WoodChop4"));
        chop.Add(Resources.Load<AudioClip>("Sounds/WoodChop/WoodChop5"));
        chop.Add(Resources.Load<AudioClip>("Sounds/WoodChop/WoodChop6"));
        chop.Add(Resources.Load<AudioClip>("Sounds/WoodChop/WoodChop7"));
        chop.Add(Resources.Load<AudioClip>("Sounds/WoodChop/WoodChop8"));
        chop.Add(Resources.Load<AudioClip>("Sounds/WoodChop/WoodChop9"));

        shovel.Add(Resources.Load<AudioClip>("Sounds/Shovel/Shovel1"));
        shovel.Add(Resources.Load<AudioClip>("Sounds/Shovel/Shovel2"));
        shovel.Add(Resources.Load<AudioClip>("Sounds/Shovel/Shovel3"));
        shovel.Add(Resources.Load<AudioClip>("Sounds/Shovel/Shovel4"));
        shovel.Add(Resources.Load<AudioClip>("Sounds/Shovel/Shovel5"));
        shovel.Add(Resources.Load<AudioClip>("Sounds/Shovel/Shovel6"));
        shovel.Add(Resources.Load<AudioClip>("Sounds/Shovel/Shovel7"));
        shovel.Add(Resources.Load<AudioClip>("Sounds/Shovel/Shovel8"));

        mine.Add(Resources.Load<AudioClip>("Sounds/Mining/Mining1"));
        mine.Add(Resources.Load<AudioClip>("Sounds/Mining/Mining2"));
        mine.Add(Resources.Load<AudioClip>("Sounds/Mining/Mining3"));
        mine.Add(Resources.Load<AudioClip>("Sounds/Mining/Mining4"));
        mine.Add(Resources.Load<AudioClip>("Sounds/Mining/Mining5"));
        mine.Add(Resources.Load<AudioClip>("Sounds/Mining/Mining6"));
        mine.Add(Resources.Load<AudioClip>("Sounds/Mining/Mining7"));

        drip.Add(Resources.Load<AudioClip>("Sounds/LiquidDripping/LiquidDripping1"));
        drip.Add(Resources.Load<AudioClip>("Sounds/LiquidDripping/LiquidDripping2"));
        drip.Add(Resources.Load<AudioClip>("Sounds/LiquidDripping/LiquidDripping3"));
        drip.Add(Resources.Load<AudioClip>("Sounds/LiquidDripping/LiquidDripping4"));
        drip.Add(Resources.Load<AudioClip>("Sounds/LiquidDripping/LiquidDripping5"));
        drip.Add(Resources.Load<AudioClip>("Sounds/LiquidDripping/LiquidDripping6"));
        drip.Add(Resources.Load<AudioClip>("Sounds/LiquidDripping/LiquidDripping7"));

        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps1"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps2"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps3"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps4"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps5"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps6"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps7"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps8"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps9"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps10"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps11"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps12"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps13"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps14"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps15"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps16"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps17"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps18"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps19"));
        footsteps.Add(Resources.Load<AudioClip>("Sounds/Steps/Steps20"));

        birdsLooping = Resources.Load<AudioClip>("Sounds/Ambient/BirdsLooping");

        blueprintCollecting = Resources.Load<AudioClip>("Sounds/Music/BlueprintCollecting");

        mapView = Resources.Load<AudioClip>("Sounds/Music/MapView");

        PlayBirdsSound();
    }
    
    void Update()
    {
        playerCurrentPos = player.transform.position;
        playerTotalDistance += Vector3.Distance(playerCurrentPos, playerLastPos);
        playerTotalTime += Time.deltaTime;
        playerLastPos = playerCurrentPos;
        if (playerTotalDistance > playerLastStep + 0.5f)
        {
            PlayStepsSound();
            playerLastStep = playerTotalDistance;
        }
    }
    
    public void PlayBlueprintOpeningSound()
    {
        soundSource.PlayOneShot(blueprintOpening[random.Next(blueprintOpening.Count)]);
    }

    public void PlayBagOpeningSound()
    {
        soundSource.PlayOneShot(bagOpening[random.Next(bagOpening.Count)]);
    }

    public void PlayButtonPressSound()
    {
        soundSource.PlayOneShot(buttonPress[random.Next(buttonPress.Count)]);
    }

    public void PlayChopSound()
    {
        soundSource.PlayOneShot(chop[random.Next(chop.Count)]);
    }

    public void PlayShovelSound()
    {
        soundSource.PlayOneShot(shovel[random.Next(shovel.Count)]);
    }

    public void PlayMineSound()
    {
        soundSource.PlayOneShot(mine[random.Next(mine.Count)]);
    }

    public void PlayDripSound()
    {
        soundSource.PlayOneShot(drip[random.Next(drip.Count)]);
    }

    public void PlayStepsSound()
    {
        soundSource.PlayOneShot(footsteps[random.Next(footsteps.Count)]);
    }

    public void PlayBlueprintCollecting()
    {
        musicSource.Stop();
        musicSource.clip = blueprintCollecting;
        musicSource.Play();
    }

    public void PlayBirdsSound()
    {
        musicSource.Stop();
        musicSource.clip = birdsLooping;
        musicSource.Play();
    }

    public void PlayAfterDelay(string functionName, float delay)
    {
        Invoke(functionName, delay);
    }
}
