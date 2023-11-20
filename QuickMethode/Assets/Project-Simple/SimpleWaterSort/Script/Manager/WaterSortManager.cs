using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class WaterSortManager : MonoBehaviour
{
    public static WaterSortManager Instance;

    [Space]
    [SerializeField] private WaterSortManagerConfig m_managerConfig;
    [SerializeField] private WaterSortBottleConfig m_bottleConfig;

    public WaterSortBottleConfig BottleConfig => m_bottleConfig;

    public WaterSortManagerConfig ManagerConfig => m_managerConfig;

    [Space]
    [SerializeField] private Transform m_groupBottle; //Got bottle gameobject
    [SerializeField] private Transform m_groupActive; //Got bottle gameobject in active

    private List<WaterSortBottle> m_bottle;
    private WaterSortBottle m_bottleA;
    private WaterSortBottle m_bottleB;

    public bool Finish
    {
        get
        {
            for (int i = 0; i < m_bottle.Count; i++)
            {
                if (!m_bottle[i].Finish)
                    return false;
            }
            //
            return true;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("[WaterSort] There are more than one instance of manager, so destroy newer instance!");
            Destroy(this.gameObject);
        }
        //
        Instance = this;
    }

    private void Start()
    {
        m_bottle = m_groupBottle.GetComponentsInChildren<WaterSortBottle>().ToList();
    }

    public void SetPress(WaterSortBottle Bottle)
    {
        if (Bottle == null)
        {
            SetBottleAUnHold();
            return;
        }
        else
        if (Bottle.Equals(m_bottleA) || !Bottle.BottleAvaible)
        {
            SetBottleAUnHold();
            return;
        }
        //
        if (m_bottleA == null)
        {
            m_bottleA = Bottle;
            m_bottleA.transform.position += Vector3.up * m_managerConfig.BottleChoiceOffsetY;
            m_bottleA.SetBottleValuePosYUpdate();
        }
        else
        {
            m_bottleB = Bottle;
            SetBottleFillStart();
        }
    }

    private void SetBottleAUnHold()
    {
        if (m_bottleA != null)
        {
            m_bottleA.transform.position += Vector3.down * m_managerConfig.BottleChoiceOffsetY;
            m_bottleA.SetBottleValuePosYUpdate();
            m_bottleA = null;
        }
    }

    private void SetBottleFillStart()
    {
        //Muti bottle can be active at the same time!
        //
        if (!m_bottleA.SetFillOut(m_bottleB, true))
        {
            m_bottleA.transform.position += Vector3.down * m_managerConfig.BottleChoiceOffsetY;
            m_bottleA.SetBottleValuePosYUpdate();
        }
        else
        {
            m_bottleA.transform.parent = m_groupActive;
            //
            GameObject BottleMover = new GameObject("object-bottle-mover");
            WaterSortBottleMover BottleAMover = BottleMover.AddComponent<WaterSortBottleMover>();
            BottleMover.transform.parent = m_groupActive;
            BottleAMover.SetInit(m_groupBottle, m_bottleA, m_bottleB);
        }
        //
        m_bottleA = null;
        m_bottleB = null;
    }
}