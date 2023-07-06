using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using TMPro;

public class EpochSceneManager : MonoBehaviour
{
    public static EpochSceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(this.gameObject);
            // Your initialization code here...
            _scenes = new List<GameObject>();
            missionItems = new List<GameObject>();
            changeSceneFeedback = GetComponent<MMF_Player>();
            foreach (Transform child in transform)
            {
                _scenes.Add(child.gameObject);
            }
            ReadDescriptionFiles();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public List<GameObject> _scenes { get; set; }
    [SerializeField] GameObject skyCamera;

    public GameObject currentScene { get; set; }
    public GameObject nextScene
    {
        get
        {
            if (_scenes.IndexOf(currentScene) < _scenes.Count - 1)
                return _scenes[_scenes.IndexOf(currentScene) + 1];
            else
                return null;
        }
    }
    Dictionary<string, string> descriptions;
    List<GameObject> missionItems;
    MMF_Player changeSceneFeedback;
    [SerializeField] GameObject switchScenePortalPrefab;
    [SerializeField] TextMeshProUGUI epochText;
    public TextMeshProUGUI hintText;
    GameObject switchScenePortal;
    public GameObject player;
    public Portal currentPortal { get; set; }


    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Change the scene.
    /// </summary>
    public void NextScene()
    {

        if (!currentScene)
        {
            currentScene = _scenes[0];
            currentScene.SetActive(true);
            epochText.text = currentScene.name;
        }
        else
        {
            int index = _scenes.IndexOf(currentScene);
            if (index < _scenes.Count - 1)
            {
                currentScene.SetActive(false);
                currentScene = _scenes[index + 1];
                currentScene.SetActive(true);
                epochText.text = currentScene.name;
                if (index == _scenes.Count - 2) // if it's the epoch before the last epoch
                    TimeLineManager.Instance.End();
            }
        }
    }



    void ReadDescriptionFiles()
    {
        descriptions = new Dictionary<string, string>();
        foreach (GameObject scene in _scenes)
        {
            string path = "EpochDescription/" + scene.name;
            TextAsset description = Resources.Load<TextAsset>(path);
            if (description)
                descriptions.Add(description?.name, description?.text);
        }
    }

    /// <summary>
    /// Get the description of the current scene.
    /// </summary>
    /// <returns></returns>
    public string GetDescription()
    {
        return descriptions[nextScene.name];
    }


    public string GetDescription(string sceneName)
    {
        return descriptions[sceneName];
    }

    /// <summary>
    /// Collect the item.
    /// </summary>
    /// <param name="item"></param>
    public void CollectItem(GameObject item)
    {
        missionItems.Add(item);
    }

    void ChangeAnimation()
    {
        if (missionItems.Count != 0)
        {
            var position = player.transform.position + player.transform.forward * 40f + new Vector3(0, 5f, 0);
            switchScenePortal = Instantiate(switchScenePortalPrefab, position, Quaternion.identity);
            switchScenePortal.transform.LookAt(player.transform);
        }
        foreach (GameObject item in missionItems)
        {
            item.GetComponent<MMAutoRotate>().Orbit(false); // stop the rotation
            StartCoroutine(FlyToPortal(item)); // fly to the portal
        }
    }

    IEnumerator FlyToPortal(GameObject item)
    {
        float time = 0;
        Vector3 startPos = item.transform.position;
        Vector3 endPos = switchScenePortal.transform.position;
        while (time < 2.5f)
        {
            time += Time.deltaTime;
            item.transform.position = Vector3.Lerp(startPos, endPos, time * 1.5f);
            yield return null;
        }
        missionItems.Remove(item);
        Destroy(item);
        if (missionItems.Count == 0)
        {
            changeSceneFeedback.GetFeedbackOfType<MMF_Scale>().AnimateScaleTarget = switchScenePortal.transform;
            changeSceneFeedback.GetFeedbackOfType<MMF_Destroy>().TargetGameObject = switchScenePortal;
            changeSceneFeedback.PlayFeedbacks();
            int index = _scenes.IndexOf(currentScene);
            changeSceneFeedback.Events.OnComplete.AddListener(Next);
            if (index != _scenes.Count - 2) // if it's not epoch before the last epoch
                StartCoroutine(ActivateSkyCamera());
        }
    }

    void Next()
    {
        NextScene(); // show the portal to the next epoch
        currentPortal?.Reset();
        changeSceneFeedback.Events.OnComplete.RemoveAllListeners();
    }

    /// <summary>
    /// After collecting all the items, show the portal to the next epoch.
    /// </summary>
    public void ShowNextPortal()
    {
        ChangeAnimation();
    }

    IEnumerator ActivateSkyCamera()
    {
        yield return new WaitForSeconds(2f);
        skyCamera.SetActive(true);
    }


}
