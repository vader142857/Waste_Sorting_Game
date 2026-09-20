using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private BagItemNode _head; // 链表头节点
    private int _itemCount;    // 当前物品数量
    private BagItemNode _currentNode; // 新增：当前选中节点
    private int _currentIndex = -1;   // 新增：当前游标位置
    private GameObject _currentDustBin; // 当前指定垃圾桶
    private float rate = 0.0f;
    private int score = 0;
    private int CorrectClassification;
    private int AllClassification;
    private int Pickup_0;
    private int Pickup_1;
    private GameObject[] pickableItems;
    private GameObject[] DustBins;

    private bool LastClassificationState = false;

    // UI相关
    public GameObject InventoryImage;
    public GameObject NullText;
    public GameObject DustBinText;
    public Image _Image;
    public Text _Text;
    public Text _DustBinText;
    public GameObject CorrectText;
    public GameObject IncorrectText;

    // CD
    private int ThrowCD = 0;

    // Start is called before the first frame update
    void Start()
    {
        score = RoundCounter.Instance.score_1;
        CorrectClassification = 0;
        AllClassification = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HandleScrollWheel();

        pickableItems = GameObject.FindGameObjectsWithTag("Waste");
        DustBins = GameObject.FindGameObjectsWithTag("DustBin");

        // 捡垃圾系统
        foreach (GameObject item in pickableItems)
        {
            // 距离检测
            if (Input.GetKeyDown(KeyCode.E) && (Vector3.Distance(this.transform.position, item.transform.position) < 1.5f * 1.5f) && (item.activeSelf))
            {
                if (item.GetComponent<ItemHandler>().Area == 0) Pickup_0 += 1;
                else if (item.GetComponent<ItemHandler>().Area == 1) Pickup_1 += 1;
                PickupItem(item);
            }
        }

        // 检测垃圾箱
        UpdateNearestDustBin();
        if (_currentDustBin != null)
        {
            DustBinText.SetActive(true);
            _DustBinText.text = "垃圾桶：" + _currentDustBin.name;
        }
        else
            DustBinText.SetActive(false);

        // 丢垃圾系统
        if (Input.GetKeyDown(KeyCode.Q) && (_currentDustBin != null) && (_currentIndex != -1) && (ThrowCD == 0))
        {
            if (GetCurrentItem().GetComponent<Wastes>().wasteType == _currentDustBin.GetComponent<Dustbin>().binType)
            {
                CorrectClassification += 1;
                Debug.Log($"对的对的");
                LastClassificationState = true;
                score += 10;
            }
            else
            {
                Debug.Log($"哦不对不对");
                LastClassificationState = false;
            }
            DestroyCurrentItem();
            AllClassification += 1;
            ThrowCD = 200;
            rate = (float)CorrectClassification / (float)AllClassification;
        }
        if (ThrowCD != 0)
        {
            if (NudgeController.Instance.Nudge)
                if (LastClassificationState) CorrectText.SetActive(true);
                else IncorrectText.SetActive(true);
            ThrowCD -= 1;
        }
        else if (ThrowCD == 0)
        {
            CorrectText.SetActive(false);
            IncorrectText.SetActive(false);
        }


        if (_itemCount == 0)
        {
            NullText.SetActive(true);
            InventoryImage.SetActive(false);
        }
        else
        {
            InventoryImage.SetActive(true);
            NullText.SetActive(false);
            _Image.sprite = GetCurrentItem().GetComponent<SpriteRenderer>().sprite;
            if (NudgeController.Instance.Nudge)
                _Text.text = GetCurrentItem().name;
        }
    }

    // 添加物品到链表末尾
    public void AddItem(GameObject prefab)
    {
        BagItemNode newNode = new BagItemNode(prefab);

        if (_head == null)
        {
            _head = newNode;
        }
        else
        {
            BagItemNode current = _head;
            while (current.next != null)
            {
                current = current.next;
            }
            current.next = newNode;
        }

        _itemCount++;
        // 新增：初始化游标
        if (_itemCount == 1)
        {
            _currentIndex = 0;
        }
        Debug.Log($"添加物品: {prefab.name}, 当前总数: {_itemCount}");
    }

    //捡拾物品
    private void PickupItem(GameObject target)
    {
        // 获取prefab引用
        ItemHandler itemHandler = target.GetComponent<ItemHandler>();

        // 添加到背包
        AddItem(itemHandler.linkedPrefab);

        // 销毁场景中的实例
        Destroy(target);
    }

    // 新增：滚轮控制逻辑
    private void HandleScrollWheel()
    {
        if (_itemCount == 0) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0) // 向上滚动
        {
            _currentIndex = (_currentIndex - 1 + _itemCount) % _itemCount;
        }
        else if (scroll < 0) // 向下滚动
        {
            _currentIndex = (_currentIndex + 1) % _itemCount;
        }
    }

    // 新增：获取当前物品
    public GameObject GetCurrentItem()
    {
        if (_itemCount == 0 || _currentIndex < 0) return null;

        BagItemNode current = GetNodeAtIndex(_currentIndex);
        return current?.prefab;
    }

    // 新增：销毁当前物品
    public void DestroyCurrentItem()
    {
        if (_itemCount == 0) return;

        BagItemNode toDelete = GetNodeAtIndex(_currentIndex);
        if (toDelete == null) return;

        // 1. 处理链表连接
        if (toDelete == _head)
        {
            _head = _head.next;
        }
        else
        {
            BagItemNode prev = GetNodeAtIndex(_currentIndex - 1);
            prev.next = toDelete.next;
        }

        // 2. 更新计数和游标
        _itemCount--;
        if (_itemCount == 0)
        {
            _currentIndex = -1;
        }
        else
        {
            _currentIndex = Mathf.Clamp(_currentIndex, 0, _itemCount - 1);
        }
    }

    // 辅助方法：获取指定索引节点
    private BagItemNode GetNodeAtIndex(int index)
    {
        if (index < 0 || index >= _itemCount) return null;

        BagItemNode current = _head;
        for (int i = 0; i < index; i++)
        {
            current = current.next;
        }
        return current;
    }

    // 检测目前垃圾桶
    private void UpdateNearestDustBin()
    {
        GameObject nearestBin = null;
        float minSqrDistance = float.MaxValue;
        Vector3 playerPosition = this.transform.position; // 假设管理器在玩家对象上

        foreach (GameObject dustBin in DustBins)
        {
            if (dustBin == null) continue; // 防止已销毁对象

            float sqrDistance = (dustBin.transform.position - playerPosition).sqrMagnitude;
            float checkDistance = 1.5f * 1.5f;

            if (sqrDistance <= checkDistance && sqrDistance < minSqrDistance)
            {
                minSqrDistance = sqrDistance;
                nearestBin = dustBin;
            }
        }

        // 更新当前指定垃圾桶（可能为null）
        _currentDustBin = nearestBin;
    }

    // 外部方法之获取积分
    public float GetRate()
    {
        return rate;
    }
    public int GetScore()
    {
        return score;
    }

    //统计数据
    public string Statistics()
    {
        string s = "Nudge=" + (NudgeController.Instance.Nudge).ToString() + "\n"
            + "捡起的垃圾：" + (Pickup_0 + Pickup_1).ToString() + "/20" + "\n"
            + "工作区域的垃圾：" + Pickup_0.ToString() + "/15" + "\n"
            + "非工作区域的垃圾：" + Pickup_1.ToString() + "/5" + "\n"
            + "丢弃的垃圾：" + AllClassification.ToString() + "/" + (Pickup_0 + Pickup_1).ToString() + "\n"
            + "分类正确数：" + CorrectClassification.ToString() + "/" + AllClassification.ToString() + "\n"
            + "分类正确率：" + rate.ToString();
        RoundCounter.Instance.RoundAdd(CorrectClassification, AllClassification, Pickup_0, Pickup_1);
        return s;
    }
}

public class BagItemNode
{
    public GameObject prefab;  // 关联的Prefab
    public BagItemNode next;   // 下一个节点
    public string type;

    public BagItemNode(GameObject prefab)
    {
        this.prefab = prefab;
        this.next = null;
        this.type = prefab.GetComponent<Wastes>().wasteType;
    }
}