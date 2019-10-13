using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public int startingLevel = 1;
    private int currentLevel;
    public GameObject levelMarker;
    public Level[] levels;

    private GameObject levelGameobject;
    private LaserEmitter laserEmitter;
    public bool isHitting = false;

    void Start()
    {
        currentLevel = startingLevel;
        LoadLevel(startingLevel);
    }

    void Update()
    {
        if (isHitting)
        {   
            if (laserEmitter.reflectionsCount >= levels[currentLevel-1].minimumReflections)
            {
                Destroy(laserEmitter.myLaser);

                print("Level passed");

                if (currentLevel == 5) Win(); else NextLevel();
            }
            else
            {
                print("You need more reflections to pass this level.");
                isHitting = false;
            }
        }
    }

    void NextLevel ()
    {
        Destroy(levelGameobject);
        currentLevel++;
        isHitting = false;
        LoadLevel(currentLevel);
    }
    void LoadLevel (int level)
    {
        levelGameobject = Instantiate(levels[level-1].prefab, levelMarker.transform);
        levelGameobject.transform.position = new Vector3(levelGameobject.transform.position.x-1.5f,levelGameobject.transform.position.y,levelGameobject.transform.position.z-1.5f);
        laserEmitter = levelGameobject.GetComponentInChildren<LaserEmitter>();
        laserEmitter.maxReflectionCount = levels[level-1].maximumReflections;
    }

    void Win()
    {
        print("You won");
    }
}
