using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Settlement : MonoBehaviour
{
    public Text Score;
    public Text Rate;
    public InventoryManager source;

    private string ScoreText = "";

    public class Ranks
    {
        public string Name;
        public int _Score;
        public Ranks(string n, int s)
        {
            Name = n;
            _Score = s;
        }
        public void RefreshScore(int s)
        {
            _Score = s;
        }
    }

    Ranks[] RanksArray = new Ranks[5];

    // Start is called before the first frame update
    void Start()
    {
        int[] Temp = new int[5] { 0, 0, 0, 0, 0 };
        Temp[0] = source.GetScore();
        RoundCounter.Instance.Output(out Temp[1], out Temp[2], out Temp[3], out Temp[4]);

        RanksArray[0] = new Ranks("一班组", Temp[0]);
        RanksArray[1] = new Ranks("二班组", Temp[1]);
        RanksArray[2] = new Ranks("三班组", Temp[2]);
        RanksArray[3] = new Ranks("四班组", Temp[3]);
        RanksArray[4] = new Ranks("五班组", Temp[4]);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 5; i += 1)
        {
            if (RanksArray[i].Name == "一班组")
                RanksArray[i].RefreshScore(source.GetScore());
        }
        RanksArray = RanksArray.OrderByDescending(x => x._Score).ToArray();
        ScoreText = "";
        for (int i = 0; i < 5; i += 1)
        {
            if (RanksArray[i].Name != "一班组")
                ScoreText = ScoreText + RanksArray[i].Name + "\t\t" + RanksArray[i]._Score.ToString() + "\n";
            else if (RanksArray[i].Name == "一班组")
                ScoreText = ScoreText + "<color=red>" + RanksArray[i].Name + "\t\t" + RanksArray[i]._Score.ToString() + "</color>" + "\n";
        }
        Score.text = ScoreText;
        Rate.text = "本日准确率：" + source.GetRate() * 100.0f + "%";
    }
}
