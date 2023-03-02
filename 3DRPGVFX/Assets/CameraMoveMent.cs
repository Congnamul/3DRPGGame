using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveMent : MonoBehaviour
{

    public Transform objToFollow;
    public float followSpeed = 10f;
    public float sensitivity = 100f;
    public float clapAngle = 70f;

    float rotX;
    float rotY;

    public Transform realCamera;
    public Vector3 dirNomalized;
    public Vector3 finalDir;
    public float minDistance;
    public float maxDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
