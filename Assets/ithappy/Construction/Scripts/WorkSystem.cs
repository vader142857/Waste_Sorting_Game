using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WorkSystem : MonoBehaviour
{
    public GameObject PlayerCamera;
    public GameObject WorkPlace;
    public GameController GameController;
    public TMP_Text PText;
    int progress;
    // Start is called before the first frame update
    void Start()
    {
        progress = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && ((WorkPlace.transform.position - PlayerCamera.transform.position).sqrMagnitude < 2f * 2f) && (GameController.State == 1))
        {
            if (progress < 99)
            {
                progress += 5;
                PText.text = "进度：" + progress + "%";
            }
        }
        if ((progress == 100) && (GameController.State == 1))
            GameController.State = 2;
    }
}
