using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public Transform CameraTarget; 
    public AnimationCurve LerpCurve;
    public float MinDistance = 2f;
    public float MaxDistance = 10f; 
    public float FollowSpeedMultiplier = 10f; 
    
    void Update() 
    {
        Vector3 cameraPos = transform.position; 
        Vector3 delta = (CameraTarget.position - cameraPos); 
        float deltaMagnitude = delta.magnitude; 

        if (deltaMagnitude > MaxDistance) 
        { 
            cameraPos = CameraTarget.position + delta.normalized * MaxDistance; 
            deltaMagnitude = MaxDistance; 
        }
        
        float lerpPercent = LerpCurve.Evaluate((delta.magnitude - MinDistance) / (MaxDistance - MinDistance)); 
        cameraPos = Vector3.MoveTowards(cameraPos, CameraTarget.position, FollowSpeedMultiplier * lerpPercent * Time.deltaTime); 
        transform.position = cameraPos; 
    }






}
