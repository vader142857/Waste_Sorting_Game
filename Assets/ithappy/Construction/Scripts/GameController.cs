using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public int State;  //代表目前所处的状态，相当于一个全局变量
    public Text Information;
    public Text EndTipInfo;
    public GameObject EndText;
    public GameObject EndTip;
    public GameObject[] WastePrefabs;          // 所有Prefab（Inspector中拖入）
    public GameObject[] WasteGameObjects0; // 所有目标GameObject（Inspector中拖入）
    public GameObject[] WasteGameObjects1;
    public InventoryManager IM;
    public GameObject Player;
    public GameObject Inventory;

    int flag = 0;

    // Start is called before the first frame update
    void Start()
    {
        State = 0;
        Debug.Log($"当前轮数：{RoundCounter.Instance.Round}");
        List<GameObject> shuffledPrefabs = new List<GameObject>(WastePrefabs);
        //生成室内垃圾
        Shuffle(shuffledPrefabs);
        for (int i = 0; i < WasteGameObjects0.Length; i++)
        {
            // 确保有ItemHandler组件
            ItemHandler itemHandler0 = WasteGameObjects0[i].GetComponent<ItemHandler>();
            if (itemHandler0 == null)
            {
                itemHandler0 = WasteGameObjects0[i].AddComponent<ItemHandler>();
            }

            // 绑定Prefab
            itemHandler0.linkedPrefab = shuffledPrefabs[i];
        }
        //生成室外垃圾
        Shuffle(shuffledPrefabs);
        for (int i = 0; i < WasteGameObjects1.Length; i++)
        {
            // 确保有ItemHandler组件
            ItemHandler itemHandler1 = WasteGameObjects1[i].GetComponent<ItemHandler>();
            if (itemHandler1 == null)
            {
                itemHandler1 = WasteGameObjects1[i].AddComponent<ItemHandler>();
            }

            // 绑定Prefab
            itemHandler1.linkedPrefab = shuffledPrefabs[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case 0:
                Info("当前状态：等待打卡（请前往打卡处签到）");
                break;
            case 1:
                Info("当前状态：开始工作（请前往工作地点）");
                break;
            case 2:
                if (NudgeController.Instance.Nudge)
                    Info("当前状态：工作完成（签退前注意地上指示）");
                else
                    Info("当前状态：工作完成（请返回打卡处签退）");
                if (flag == 0)
                {
                    //显示室内垃圾共15件
                    List<GameObject> activationList0 = new List<GameObject>(WasteGameObjects0);
                    Shuffle(activationList0);
                    for (int i = 0; i < 15; i++)
                    {
                        activationList0[i].SetActive(true);
                    }
                    for (int i = 15; i < 20; i++)
                    {
                        Destroy(activationList0[i]);
                    }
                    //显示室外垃圾共5件
                    List<GameObject> activationList1 = new List<GameObject>(WasteGameObjects1);
                    Shuffle(activationList1);
                    for (int i = 0; i < 5; i++)
                    {
                        activationList1[i].SetActive(true);
                    }
                    for (int i = 5; i < 20; i++)
                    {
                        Destroy(activationList1[i]);
                    }
                    //结束
                    flag = 1;
                }
                break;
            case 3:
                Info("当前状态：游戏结束");
                if (IM.GetScore() != 0 && NudgeController.Instance.Nudge)
                    EndText.SetActive(true);
                Player.SetActive(false);
                Inventory.SetActive(false);
                EndTip.SetActive(true);

                if (RoundCounter.Instance.Round < 3)
                {
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        SaveStringToFile(IM.Statistics(), "Round" + (RoundCounter.Instance.Round).ToString());
                        RoundCounter.Instance.Round += 1;
                        RoundCounter.Instance.score_1 = IM.GetScore();
                        RoundCounter.Instance.Refresh();
                        SceneManager.LoadScene("Demonstration_Day");
                    }
                }
                else
                {
                    EndTipInfo.text = "R：结束游戏";
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        SaveStringToFile(IM.Statistics(), "Round" + (RoundCounter.Instance.Round).ToString());
                        SaveStringToFile(RoundCounter.Instance.Statistics(), "Total");
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                    }
                }
                break;
            default: break;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
        }
    }

    void Info(string s)
    {
        Information.text = s;
    }

    // Fisher-Yates洗牌算法
    private void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void SaveStringToFile(string content, string round)
    {
#if UNITY_EDITOR
        // 编辑器模式下保存到项目根目录的 OutputFiles 文件夹
        string projectPath = Application.dataPath.Replace("/Assets", "");
        string saveFolder = Path.Combine(projectPath, "OutputFiles");
#else
        // 发布后仍然保存到持久化路径
        string saveFolder = Application.persistentDataPath+"/OutputFiles/";
#endif

        // 生成文件名（示例：20231023_153045.txt）
        string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + round + ".txt";
        string safeFileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
        string savePath = Path.Combine(saveFolder, safeFileName);

        try
        {
            // 创建目录（如果不存在）
            Directory.CreateDirectory(saveFolder);

            // 写入文件
            File.WriteAllText(savePath, content);

#if UNITY_EDITOR
            // 在编辑器中刷新资源窗口
            UnityEditor.AssetDatabase.Refresh();
            Debug.Log($"文件已保存到项目文件夹：{savePath}");
#else
            Debug.Log($"文件已保存：{savePath}");
#endif
        }
        catch (Exception e)
        {
            Debug.LogError($"保存失败：{e.Message}");
        }
    }
}
