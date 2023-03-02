using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;
    Vector3 moveDirection;

    CharacterController characterCon;
    Animator amim;

    // Start is called before the first frame update
    void Start()
    {
        characterCon = GetComponent<CharacterController>();
        amim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        amim.SetBool("isRun", x != 0 || z != 0);
        moveDirection = new Vector3(x, 0, z);
        characterCon.Move(moveDirection * moveSpeed * Time.deltaTime);



        
    }
}
