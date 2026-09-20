using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TipsController : MonoBehaviour
{
    public GameObject Tip1;
    public GameObject Tip2;

    public GameObject Route1;
    public GameObject Route2;
    public GameObject Route3;

    public GameController GameController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(GameController.State) 
        {
            case 0:
                Tip1.SetActive(true);
                Route1.SetActive(true);
                break;
            case 1:
                Tip1.SetActive(false);
                Route1.SetActive(false);
                Tip2.SetActive(true);
                Route2.SetActive(true);
                break;
            case 2:
                Tip2.SetActive(false);
                Route2.SetActive(false);
                if (NudgeController.Instance.Nudge)
                    Route3.SetActive(true);
                break;
            case 3:
                Route3.SetActive(false);
                break;
            default:
                break;
        }
    }
}
