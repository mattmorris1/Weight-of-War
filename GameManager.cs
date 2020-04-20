using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool isShortCampaign;
    private bool packedSocks = false;
    private bool packedGun = false;
    private bool packedMedKit = false;
    private bool packedGasMask = false;
    private float totalWeight;
    private readonly float maxWeight = 60f;
    private List<string> packedObjects;
    private float count;
    private readonly float packingTime = 60f;
    string currentScene;
    public Animator transition;
    public float transitionTime = 2f;
    private GameObject spawn;


    private void OnEnable()
    {
        //Setting up the scene manager code
        SceneManager.sceneLoaded += OnSceneLoaded;
        //Make the player game object persist across scenes
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Code to set up the timer in the packing scene for the short version
        if(currentScene == "Packing")
        {
            if(isShortCampaign == true)
            {
                count -= 1 * Time.deltaTime;
                if(count <= 0)
                {
                    StopPacking();
                }
            }
        }
    }

    //Loads the packing scene and sets the game to run the short campaign
    public void StartShortCampaign()
    {
        isShortCampaign = true;
        StartCoroutine(TransitionScene("Transition1"));
    }

    //Loads the packing scene and sets the game to run the long campaign
    public void StartLongCampaign()
    {
        isShortCampaign = false;
        StartCoroutine(TransitionScene("Transition1"));
    }

    //Runs everytime a new scene is loaded
    //Used to check which scene is loaded and determine the next scene to load
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded");
        currentScene = SceneManager.GetActiveScene().name;
        Debug.Log(currentScene);
        //Finds the spawn location in the new scene and moves the player there
        spawn = GameObject.Find("/Spawn");
        transform.position = spawn.transform.position;
        transform.rotation = spawn.transform.rotation;
        //Fades the new scene in
        //transition.SetTrigger("Stop_Crossfade");
        transition.SetBool("Crossfade", false);
        //Resets the counter each time you enter the packing scene
        if (currentScene == "Start Scene")
        {
            StartCoroutine(TransitionScene("Menu"));
        }
        else if (currentScene == "Menu")
        {
            if (packedObjects != null)
            {
                packedObjects.Clear();
            }
            totalWeight = 0f;
        }
        else if (currentScene == "Transition1")
        {
            StartCoroutine(WaitThenPlay(7f, "Packing"));
        }
        else if(currentScene == "Packing")
        {
            count = packingTime;
        }
        else if(currentScene == "Transition2")
        {
            if (isShortCampaign)
            {
                if (!packedSocks)
                {
                    StartCoroutine(WaitThenPlay(7f, "Trench1Lose"));
                }
                else if (!packedGasMask)
                {
                    StartCoroutine(WaitThenPlay(7f, "Trench2Lose"));
                }
                else if (!packedMedKit)
                {
                    StartCoroutine(WaitThenPlay(7f, "Hill1Lose"));
                }
                else if (!packedGun)
                {
                    StartCoroutine(WaitThenPlay(7f, "Hill2Lose"));
                }
                else
                {
                    StartCoroutine(WaitThenPlay(7f, "WinAll"));
                }
            }
            else
            {
                if (!packedSocks)
                {
                    StartCoroutine(WaitThenPlay(7f, "Trench1Lose"));
                }
                else
                {
                    StartCoroutine(WaitThenPlay(7f, "Trench1Win"));
                }
            }
        }
        else if (currentScene == "Trench1Lose")
        {
            StartCoroutine(WaitThenPlay(7f, "Menu"));
        }
        else if (currentScene == "Trench1Win")
        {
            if (!packedGasMask)
            {
                StartCoroutine(WaitThenPlay(7f, "Trench2Lose"));
            }
            else
            {
                StartCoroutine(WaitThenPlay(7f, "Trench2Win"));
            }
        }
        else if (currentScene == "Trench2Lose")
        {
            StartCoroutine(WaitThenPlay(7f, "Menu"));
        }
        else if (currentScene == "Trench2Win")
        {
            if (!packedMedKit)
            {
                StartCoroutine(WaitThenPlay(7f, "Hill1Lose"));
            }
            else
            {
                StartCoroutine(WaitThenPlay(7f, "Hill1Win"));
            }
        }
        else if (currentScene == "Hill1Lose")
        {
            StartCoroutine(WaitThenPlay(7f, "Menu"));
        }
        else if (currentScene == "Hill1Win")
        {
            if (!packedGun)
            {
                StartCoroutine(WaitThenPlay(7f, "Hill2Lose"));
            }
            else
            {
                StartCoroutine(WaitThenPlay(7f, "Hill2Win"));
            }
        }
        else if (currentScene == "Hill2Lose")
        {
            StartCoroutine(WaitThenPlay(7f, "Menu"));
        }
        else if (currentScene == "Hill2Win")
        {
            StartCoroutine(WaitThenPlay(7f, "Menu"));
        }
        else if (currentScene == "WinAll")
        {
            StartCoroutine(WaitThenPlay(7f, "Menu"));
        }

    }

    //Controls the scene fade out and loads the new scene
    IEnumerator TransitionScene(string sceneName)
    {
        //transition.SetTrigger("Start_Crossfade");
        transition.SetBool("Crossfade", true);
        StartCoroutine(LoadSceneBackground(sceneName));
        //SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        yield return new WaitForSeconds(transitionTime);
    }

    //Used to wait for specified number of seconds while scenes play and then loads the next scene
    IEnumerator WaitThenPlay(float waitTime, string sceneName)
    {
        Debug.Log("Waiting");
        yield return new WaitForSeconds(waitTime);
        //transition.SetTrigger("Start_Crossfade");
        transition.SetBool("Crossfade", true);
        StartCoroutine(LoadSceneBackground(sceneName));
        yield return new WaitForSeconds(transitionTime);
        //SceneManager.LoadScene(sceneName);
    }

    //Run at the end of the packing scene
    public void StopPacking()
    {
        //Accesses the bag object to get the weight and packed items
        packedObjects = GameObject.Find("/Bag/BagArea").GetComponent<Bag>().GetItems();
        totalWeight = GameObject.Find("/Bag/BagArea").GetComponent<Bag>().GetWeight();
        //Check through the packed items for each necessary game item
        foreach(string item in packedObjects){
            if(item == "Socks")
            {
                packedSocks = true;
            }else if(item == "Pistol")
            {
                packedGun = true;
            }
            else if (item == "Med Kit")
            {
                packedMedKit = true;
            }
            else if (item == "Gas Mask")
            {
                packedGasMask = true;
            }
        }
        StartCoroutine(TransitionScene("Transition2"));
    }

    //This will load a scene in the background once a transition starts
    IEnumerator LoadSceneBackground(string sceneToLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
