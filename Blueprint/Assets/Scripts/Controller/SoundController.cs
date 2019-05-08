using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Controller {
    public class SoundController : MonoBehaviour {
        [SerializeField] public AudioSource soundSource;
        [SerializeField] public AudioSource ambientSource;

        [SerializeField] public AudioSource musicSource;

        // Keeping track of player position for footsteps.
        [SerializeField] public GameObject player;

        [SerializeField]public bool isMenu;

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
        private AudioClip buildingEnvironment;
        private AudioClip introMusic;
        private AudioClip outroMusic;

        private System.Random random;


        void Start() {
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

            buttonPress.Add(Resources.Load<AudioClip>("Sounds/ButtonPress/ButtonPress1"));
            buttonPress.Add(Resources.Load<AudioClip>("Sounds/ButtonPress/ButtonPress2"));
            buttonPress.Add(Resources.Load<AudioClip>("Sounds/ButtonPress/ButtonPress3"));
            buttonPress.Add(Resources.Load<AudioClip>("Sounds/ButtonPress/ButtonPress4"));
            buttonPress.Add(Resources.Load<AudioClip>("Sounds/ButtonPress/ButtonPress5"));

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

            buildingEnvironment = Resources.Load<AudioClip>("Sounds/Music/BuildingEnvironment");

            introMusic = Resources.Load<AudioClip>("Sounds/Music/IntroMusic");

            outroMusic = Resources.Load<AudioClip>("Sounds/Music/OutroMusic");

            if (!isMenu)
                PlayBirdsSound();
            else
                PlayMenuSound();
        }
        
        void Update() {
            if (isMenu)
                return;

            // Don't want to include walking sounds while jumping.
            if (!player.GetComponent<PlayerMoveController>().isJumping) {
                playerCurrentPos = player.transform.position;
                playerTotalDistance += Vector3.Distance(playerCurrentPos, playerLastPos);
                playerTotalTime += Time.deltaTime;
                playerLastPos = playerCurrentPos;
                if (playerTotalDistance > playerLastStep + 2.4f) {
                    PlayStepsSound();
                    playerLastStep = playerTotalDistance;
                }
            }

            // One in 10,000 chance per frame for... reasons.
            if (!musicSource.isPlaying && UnityEngine.Random.Range(0.0f, 1.0f) < 0.0001f)  {
                PlayBuildingEnvironmentMusic();
                ambientSource.Stop();
            }

            // Play ambient noise of no music is playing.
            if (!musicSource.isPlaying && !ambientSource.isPlaying) {
                PlayBirdsSound();
            }
        }
        
        public void PlayBlueprintOpeningSound() {
            soundSource.PlayOneShot(blueprintOpening[random.Next(blueprintOpening.Count)]);
        }

        public void PlayBagOpeningSound() {
            soundSource.PlayOneShot(bagOpening[random.Next(bagOpening.Count)]);
        }

        public void PlayButtonPressSound() {
            soundSource.PlayOneShot(buttonPress[random.Next(buttonPress.Count)]);
        }

        public void PlayChopSound() {
            soundSource.PlayOneShot(chop[random.Next(chop.Count)]);
        }

        public void PlayShovelSound() {
            soundSource.PlayOneShot(shovel[random.Next(shovel.Count)]);
        }

        public void PlayMineSound() {
            soundSource.PlayOneShot(mine[random.Next(mine.Count)]);
        }

        public void PlayDripSound() {
            soundSource.PlayOneShot(drip[random.Next(drip.Count)]);
        }

        public void PlayMachinePlacementSound() {
            soundSource.PlayOneShot(drip[random.Next(drip.Count)]);
        }

        public void PlayStepsSound() {
            soundSource.PlayOneShot(footsteps[random.Next(footsteps.Count)]);
        }

        public void PlayJumpSound() {
            playerCurrentPos = player.transform.position;
            playerTotalDistance += Vector3.Distance(playerCurrentPos, playerLastPos);
            playerTotalTime += Time.deltaTime;
            playerLastPos = playerCurrentPos;
            playerLastStep = playerTotalDistance;
            soundSource.PlayOneShot(footsteps[random.Next(footsteps.Count)]);

            // Stopping a step sound halfway through makes it sound more like a jump.
            Invoke("StopJumpSound", 0.25f);

            // Want landing sound to play slightly before end of jump as loading the clip is slightly delayed.
            Invoke("PlayLandSound", 0.62f);
        }

        private void StopJumpSound() {
            soundSource.Stop();
        }

        private void PlayLandSound() {
            playerCurrentPos = player.transform.position;
            playerTotalDistance += Vector3.Distance(playerCurrentPos, playerLastPos);
            playerTotalTime += Time.deltaTime;
            playerLastPos = playerCurrentPos;
            playerLastStep = playerTotalDistance;
            soundSource.PlayOneShot(footsteps[random.Next(footsteps.Count)]);
        }

        public void PlayBlueprintCollectingMusic() {
            musicSource.Stop();
            musicSource.clip = blueprintCollecting;
            musicSource.Play();
        }

        public void PlayBuildingEnvironmentMusic() {
            musicSource.Stop();
            musicSource.clip = buildingEnvironment;
            musicSource.Play();
        }

        public void PlayIntroMusic() {
            musicSource.Stop();
            musicSource.clip = introMusic;
            musicSource.Play();
        }

        public void PlayOutroMusic() {
            musicSource.Stop();
            musicSource.clip = outroMusic;
            musicSource.Play();
        }
        
        public void PlayPlacementSound(int inpID) {
            switch (inpID) {
                case 1: PlayChopSound(); break;
                case 2: PlayMineSound(); break;
                case 3: PlayShovelSound(); break;
                case 4: PlayMineSound(); break;
                case 5: PlayMineSound(); break;
                case 6: PlayDripSound(); break;
                case 7: PlayMineSound(); break;
                case 8: PlayShovelSound(); break;
                case 9: PlayMineSound(); break;
                case 10: PlayMineSound(); break;
                case 11: PlayMachinePlacementSound(); break;
                case 12: PlayShovelSound(); break;
                case 13: PlayMineSound(); break;
                case 14: PlayMineSound(); break;
                case 15: PlayMineSound(); break;
                case 16: PlayMineSound(); break;
                case 17: PlayMachinePlacementSound(); break;
                case 18: PlayMachinePlacementSound(); break;
                case 19: PlayMachinePlacementSound(); break;
                case 20: PlayMachinePlacementSound(); break;
                case 21: PlayMachinePlacementSound(); break;
                case 22: PlayMachinePlacementSound(); break;
                case 23: PlayMachinePlacementSound(); break;
                case 24: PlayMachinePlacementSound(); break;
                case 25: PlayMachinePlacementSound(); break;
                case 26: PlayMachinePlacementSound(); break;
                case 27: PlayMachinePlacementSound(); break;
                case 28: PlayMachinePlacementSound(); break;
                case 29: PlayMachinePlacementSound(); break;
                case 30: PlayMachinePlacementSound(); break;
                case 31: PlayMachinePlacementSound(); break;
                default: PlayMineSound(); break;
            }
        }

        public void PlayBirdsSound() {
            // ambientSource.Stop();
            // ambientSource.clip = birdsLooping;
            // ambientSource.Play();
        }

        public void PlayMenuSound() {
            musicSource.Stop();
            musicSource.clip = birdsLooping;
            musicSource.Play();
        }

        public void PlayAfterDelay(string functionName, float delay) {
            Invoke(functionName, delay);
        }
    }
}