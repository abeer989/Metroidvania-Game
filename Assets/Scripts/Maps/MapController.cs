using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    public static MapController instance;

    [SerializeField] GameObject minimapCam;
    [SerializeField] GameObject fullscreenMapCam;

    [Space]
    [SerializeField] List<GameObject> maps;

    PlayerController player;

    private void Awake()
    {
        // Singleton:
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);
    }

    private void Start()
    {
        maps.ForEach(map =>
        {
            // the maps are stored at indexes from 0-... 
            int indexInList = maps.IndexOf(map) + 1;
            if (PlayerPrefs.GetInt("level_map_" + indexInList) == 1)
                map.SetActive(true);
        });

        player = PlayerHealthController.instance.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!UIController.instance.isGamePaused)
            {
                if (!UIController.instance.FullscreenMap.activeSelf)
                {
                    Debug.Log("M");
                    minimapCam.SetActive(false);
                    UIController.instance.MiniMap.SetActive(false);

                    fullscreenMapCam.SetActive(true);
                    UIController.instance.FullscreenMap.SetActive(true);

                    Time.timeScale = 0;
                }

                else
                {
                    minimapCam.SetActive(true);
                    UIController.instance.MiniMap.SetActive(true);

                    fullscreenMapCam.SetActive(false);
                    UIController.instance.FullscreenMap.SetActive(false);

                    Time.timeScale = 1;
                } 
            }
        }
    }

    public void ActivateMap(int mapIndex)
    {
        // entering values in the inspector, starting from one, so the value is being corrected here
        // by subtracting 1 to avoid out of range exceptions:
        mapIndex -= 1;

        if (mapIndex < 0)
            mapIndex = 0;

        else if (mapIndex >= maps.Count)
            mapIndex = maps.Count - 1;

        //maps.ForEach(a => a.SetActive(false));
        maps[mapIndex].SetActive(true);

        // if a map is active, a key (e.g.: map_2) will get saved in PP:
        PlayerPrefs.SetInt("level_map_" + (mapIndex + 1), 1);
    }
}
