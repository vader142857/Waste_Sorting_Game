using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NudgeController : MonoBehaviour
{
    public static NudgeController Instance; // 单例实例
    public bool Nudge = false;

    public GameObject BillBoard1;
    public GameObject BillBoard2;
    public GameObject BillBoard3;
    public GameObject BillBoardOthers;
    public GameObject classificationareabillboard;
    public GameObject Arrows;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        string filePath = Path.Combine(Application.streamingAssetsPath, "Nudge.txt");

        try
        {
            // 安全读取文件
            if (File.Exists(filePath))
            {
                // 读取并清理内容
                string content = File.ReadAllText(filePath).Trim();

                // 核心判断逻辑
                Nudge = (content != "0"); // 只要不是0都视为true
                Debug.Log($"成功读取Nudge值: {Nudge}");
            }
            else
            {
                Debug.LogWarning("Nudge.txt文件不存在，使用默认值false");
                Nudge = false;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"读取文件失败: {e.Message}");
            Nudge = false;
        }


        if (Nudge)
        {
            BillBoard1.SetActive(true);
            BillBoard2.SetActive(true);
            BillBoard3.SetActive(true);
            BillBoardOthers.SetActive(true);
            classificationareabillboard.SetActive(true);
            Arrows.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
