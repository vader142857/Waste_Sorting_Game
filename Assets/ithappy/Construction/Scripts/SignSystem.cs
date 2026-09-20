using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignSystem : MonoBehaviour
{
    public GameObject PlayerCamera;
    public GameObject SignPlace;
    public GameController GameController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && ((SignPlace.transform.position - PlayerCamera.transform.position).sqrMagnitude < 4f * 4f) && (GameController.State == 0))
            GameController.State = 1;

        if (Input.GetKeyDown(KeyCode.E) && ((SignPlace.transform.position - PlayerCamera.transform.position).sqrMagnitude < 4f * 4f) && (GameController.State == 2))
            GameController.State = 3;
    }
}
