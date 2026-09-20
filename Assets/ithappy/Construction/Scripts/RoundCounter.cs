using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundCounter : MonoBehaviour
{
    public static RoundCounter Instance; // 单例实例
    public int Round = 1;

    //这一段用来做总统计，别问我为什么写在这，问就是方便
    private int CorrectClassification = 0;
    private int AllClassification = 0;
    private int Pickup_0 = 0;
    private int Pickup_1 = 0;
    private float rate = 0.0f;

    //排行榜用
    public int score_1;
    private int score_2;
    private int score_3;
    private int score_4;
    private int score_5;

    private void Awake()
    {
        //初始化
        score_1 = 0;
        score_2 = UnityEngine.Random.Range(17, 21) * 10;
        score_3 = UnityEngine.Random.Range(12, 15) * 10;
        score_4 = UnityEngine.Random.Range(5, 8) * 10;
        score_5 = UnityEngine.Random.Range(0, 4) * 10;
        // 确保单例唯一性
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 跨场景不销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //统计汇总，隔壁的InventoryManager每输出一次，就把总数叠加到这里一次
    public void RoundAdd(int n0, int n1, int n2, int n3)
    {
        CorrectClassification += n0;
        AllClassification += n1;
        Pickup_0 += n2;
        Pickup_1 += n3;
        rate = (float)CorrectClassification / (float)AllClassification;
    }

    //同样的生成统计数据
    public string Statistics()
    {
        string s = "Nudge=" + (NudgeController.Instance.Nudge).ToString() + "\n"
            + "捡起的垃圾：" + (Pickup_0 + Pickup_1).ToString() + "/60" + "\n"
            + "工作区域的垃圾：" + Pickup_0.ToString() + "/45" + "\n"
            + "非工作区域的垃圾：" + Pickup_1.ToString() + "/15" + "\n"
            + "丢弃的垃圾：" + AllClassification.ToString() + "/" + (Pickup_0 + Pickup_1).ToString() + "\n"
            + "分类正确数：" + CorrectClassification.ToString() + "/" + AllClassification.ToString() + "\n"
            + "分类正确率：" + rate.ToString();
        return s;
    }

    //每轮开始时改变分数
    public void Refresh()
    {
        score_2 += UnityEngine.Random.Range(5, 16) * 10;
        score_3 += UnityEngine.Random.Range(10, 16) * 10;
        score_4 += UnityEngine.Random.Range(0, 11) * 10;
        score_5 += UnityEngine.Random.Range(10, 21) * 10;
    }

    //输出
    public void Output(out int s2, out int s3, out int s4, out int s5)
    {
        s2 = score_2;
        s3 = score_3;
        s4 = score_4;
        s5 = score_5;
    }
}
