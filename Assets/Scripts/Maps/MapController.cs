using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public static MapController instance;

    [SerializeField] List<GameObject> maps;

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
    }

    public void ActivateMap(int mapIndex)
    {
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
