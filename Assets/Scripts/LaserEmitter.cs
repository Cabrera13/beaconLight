using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VolumetricLines;
public class LaserEmitter : MonoBehaviour
{
    public Ray ray;
    private RaycastHit hit;
    public int maxReflectionCount = 5;
    public GameObject gameLogic;
    public float maxStepDistance = 200;
    public GameObject volumetricLinePrefab;
    public bool showReflectionsInEditor = true;

    //public GameObject particlesPrefab;
    //private List<GameObject> collisionParticlesList;

    public List<Vector3> positionTestList;
    [HideInInspector] public int reflectionsCount;
    public GameObject myLaser;
    private VolumetricMultiLineBehavior myLaserBehavior;

    private void Start ()
    {
        positionTestList = new List<Vector3>();
    }
    private void FixedUpdate ()
    {
        DrawPredictedReflection( this.transform.position + this.transform.forward * 0.1f, this.transform.forward, maxReflectionCount );
    }
    private void DrawPredictedReflection ( Vector3 position, Vector3 direction, int reflectionsRemaining )
    {
        positionTestList = new List<Vector3>();
        positionTestList.Add( transform.position );

        //foreach (GameObject part in collisionParticlesList)
        //{
        //    Destroy(part);
        //}
        //collisionParticlesList = new List<GameObject>();

        for ( int i = reflectionsRemaining; i > 0; i-- )
        {
            Vector3 startingPosition = position;
            ray = new Ray( position, direction );
            
            if ( Physics.Raycast( ray, out hit, maxStepDistance ) )
            {
                if (hit.transform.tag == "exit") {
                    GameObject.Find("Game Controller").GetComponent<GameLogic>().isHitting = true;
                }
                else if (hit.transform.tag == "mirror") {
                    direction = Vector3.Reflect( direction, hit.normal );
                    position = hit.point;
                    positionTestList.Add( position );
                    reflectionsCount++;
                }
                else {
                    position = hit.point;
                    positionTestList.Add( position );
                    reflectionsCount = 0;
                }

                //collisionParticlesList.Add(Instantiate(particlesPrefab, position,Quaternion.identity));
            }
            else
            {
                position += direction * maxStepDistance;
                positionTestList.Add( position );
            }
            //pointsArray[maxReflectionCount - reflectionsRemaining +1] = position;
        }
        Destroy( myLaser );
        myLaser = Instantiate( volumetricLinePrefab );
        myLaserBehavior = myLaser.GetComponent<VolumetricMultiLineBehavior>();
        myLaserBehavior.m_lineVertices = positionTestList.ToArray();
    }
    
    public string GetHitObjectTag()
    {
        try {
            return hit.transform.tag;
        } catch{return "";}
    }

    void OnDrawGizmos ()
    {
        if ( showReflectionsInEditor )
        {
            DrawPredictedReflectionPattern( this.transform.position + this.transform.forward * 0.75f, this.transform.forward, maxReflectionCount );
        }
    }

    private void DrawPredictedReflectionPattern ( Vector3 position, Vector3 direction, int reflectionsRemaining )
    {
        if ( showReflectionsInEditor )
        {
            if ( reflectionsRemaining == 0 )
            {
                return;
            }
            Vector3 startingPosition = position;
            Ray ray = new Ray( position, direction );
            RaycastHit hit;
            if ( Physics.Raycast( ray, out hit, maxStepDistance ) )
            {
                direction = Vector3.Reflect( direction, hit.normal );
                position = hit.point;
            }
            else
            {
                position += direction * maxStepDistance;
            }
            Gizmos.DrawLine( startingPosition, position );
            DrawPredictedReflectionPattern( position, direction, reflectionsRemaining - 1 );
        }
    }
}