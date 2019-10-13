﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VolumetricLines;
public class LaserEmitter : MonoBehaviour
{
    public Ray ray;
    public RaycastHit hit;
    public int maxReflectionCount = 5;
    public float maxStepDistance = 200;
    public GameObject volumetricLinePrefab;
    public bool showReflectionsInEditor;

    //public GameObject particlesPrefab;
    //private List<GameObject> collisionParticlesList;

    public List<Vector3> positionTestList;
    public GameObject myLaser;
    private VolumetricMultiLineBehavior myLaserBehavior;

    private void Start ()
    {
        positionTestList = new List<Vector3>();
    }
    private void FixedUpdate ()
    {
        DrawPredictedReflection( this.transform.position + this.transform.forward * 0.75f, this.transform.forward, maxReflectionCount );
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

                direction = Vector3.Reflect( direction, hit.normal );
                position = hit.point;
                positionTestList.Add( position );

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