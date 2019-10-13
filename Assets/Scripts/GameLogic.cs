using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public LaserEmitter laserEmitter;
    public GameObject[] arrayPrefabs;
    public GameObject[] arrayFather;
    public int level = 1;

    void Update()
    {
        if (laserEmitter.hit.transform.tag == "exit")
        {
            if (laserEmitter.positionTestList.Count == laserEmitter.maxReflectionCount-1)
            {
                Destroy(laserEmitter.myLaser);
                print("S'ha guanyat el nivell");

                if (level == 5)
                {
                    print("S'ha guanyat el joc");
                }
                else
                {
                    Instantiate(arrayPrefabs[level], arrayFather[level].transform);
                    Destroy(arrayPrefabs[level-1]);
                    level++;
                }
            }
            else if (laserEmitter.positionTestList.Count < laserEmitter.maxReflectionCount-1)
            {
                print("S'esta tocant el final pero no hi han suficients miralls a la partida");
            }
        }
    }
}
