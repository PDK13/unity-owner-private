using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using QuickMethode;

public class SimpleSlotMachine : MonoBehaviour
{
    //Data!!
    [SerializeField] private List<string> m_ItemQueue = new List<string>() { "0", "1", "2", "3", "4", "5", "6", };

    //Memory!!
    private class Item
    {
        public RectTransform ItemContent;
        public int Index;

        public Item(RectTransform ItemContent, int Index)
        {
            this.ItemContent = ItemContent;
            this.Index = Index;
        }
    }
    private List<Item> m_Item = new List<Item>();
    private int m_ItemSet = 0;
    private int m_ItemFindQueue = -1; //Item Find in Queue!!
    private int m_ItemFindContent = -1; //Item Find in Content!!

    //Stage!!
    private enum Stage { Wait = 0, Loop = 1, Find = 2, Final = 3, End = 4, }
    private Stage m_Stage = Stage.Wait;

    [SerializeField] [Min(0)] private float m_Speed = 3f;

    private UIScrollViewSingle m_ScrollViewSingle;

    private void Start()
    {
        m_ScrollViewSingle = GetComponent<UIScrollViewSingle>();

        List<RectTransform> ItemContentGet = m_ScrollViewSingle.GetContentItem();
        for (int i = 0; i < ItemContentGet.Count; i++)
            m_Item.Add(new Item(ItemContentGet[i], i));

        SetUpdateInc();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (m_Stage)
            {
                case Stage.Wait:
                    m_Stage = Stage.Loop;
                    break;
                case Stage.Loop:
                    m_Stage = Stage.Find;
                    //Random!!
                    m_ItemFindQueue = Random.Range(0, (m_ItemQueue.Count - 1) * 10) / 10;
                    break;
                case Stage.Find:
                    //Do nothing!!
                    break;
                case Stage.Final:
                    //Do nothing!!
                    break;
                case Stage.End:
                    m_Stage = Stage.Wait;
                    //Reset!!
                    m_ItemSet = 0;
                    m_ItemFindQueue = -1;
                    m_ItemFindContent = -1;
                    m_ScrollViewSingle.SetContent(0);
                    SetUpdateInc();
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        SetUpdateActive();
    }

    private void SetUpdateActive()
    {
        if (m_Stage == Stage.Wait) return;

        if (m_Speed <= 0) return;

        if (m_Stage == Stage.Loop || m_Stage == Stage.Find)
        {
            //Move Content to Last Item!!
            QTransform.SetMoveToward(m_ScrollViewSingle.Content, m_ScrollViewSingle.ContentLast, m_Speed);
        }
        else
        if (m_Stage == Stage.Final)
        {
            //Move Content to Last Item!!
            QTransform.SetMoveToward(m_ScrollViewSingle.Content, m_ScrollViewSingle.GetContent(m_ItemFindContent), m_Speed);
        }

        if (m_Stage == Stage.Loop || m_Stage == Stage.Find)
        {
            //Content Reach the Last Item!!
            if (m_ScrollViewSingle.Content.anchoredPosition.y >= m_ScrollViewSingle.ContentLast.y)
            {
                m_ScrollViewSingle.SetContent(0);

                SetUpdateInc();
            }
        }
        else
        if (m_Stage == Stage.Final)
        {
            //Content Reach the Random Item!!
            if (m_ScrollViewSingle.Content.anchoredPosition.y >= m_ScrollViewSingle.GetContent(m_ItemFindContent).y)
            {
                m_Stage = Stage.End;
            }
        }
    }

    private void SetUpdateInc()
    {
        for (int i = 0; i < m_Item.Count; i++)
        {
            m_Item[i].ItemContent.Find("Tmp").GetComponent<TextMeshProUGUI>().text = m_ItemQueue[m_ItemSet];
            m_Item[i].Index = m_ItemSet;

            if (m_Stage == Stage.Find)
            {
                if (m_ItemSet == m_ItemFindQueue)
                {
                    m_Stage = Stage.Final;
                    m_ItemFindContent = i;
                }
            }

            if (i < m_Item.Count - 1)
                m_ItemSet++;

            if (m_ItemSet > m_ItemQueue.Count - 1)
                m_ItemSet = 0;
        }
    }
}