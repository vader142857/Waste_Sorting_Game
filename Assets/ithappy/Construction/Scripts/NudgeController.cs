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

        string filePath = GetNudgeFilePath();

        try
        {
            // 文件不存在就创建一个默认的，方便外部修改
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "1");
                Debug.Log($"Nudge.txt 不存在，已创建默认文件: {filePath}");
            }

            string content = File.ReadAllText(filePath).Trim();

            // 核心判断逻辑：只要不是 "0" 都视为 true
            Nudge = (content != "0");
            Debug.Log($"成功读取 Nudge 值: {Nudge}，路径: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"读取 Nudge.txt 失败: {e.Message}");
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

    /// <summary>
    /// 编辑器：StreamingAssets 下（不污染项目根目录）
    /// 桌面发布版：exe 同级目录（游戏根目录，外部可改）
    /// 移动端/WebGL：退回 StreamingAssets（只读，无法外部改）
    /// </summary>
    private string GetNudgeFilePath()
    {
#if UNITY_EDITOR
        return Path.Combine(Application.streamingAssetsPath, "Nudge.txt");
#elif UNITY_STANDALONE
        // Application.dataPath 在 Windows 下是 xxx_Data 文件夹
        // 它的父目录就是游戏根目录（exe 所在处）
        string gameRoot = Directory.GetParent(Application.dataPath).FullName;
        return Path.Combine(gameRoot, "Nudge.txt");
#else
        return Path.Combine(Application.streamingAssetsPath, "Nudge.txt");
#endif
    }

    // Update is called once per frame
    void Update()
    {

    }
}
