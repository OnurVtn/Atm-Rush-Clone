using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stacker : MonoBehaviour
{
    private static Stacker instance;

    public static Stacker Instance => instance ?? (instance = FindObjectOfType<Stacker>());

    [SerializeField] private Transform playerVisualTransform;
    public List<GameObject> moneys;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        MoveFirstMoney();
        CheckMoneyCount();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Money"))
        {
            StackFirstMoney(other);
        }
    }

    private void StackFirstMoney(Collider firstMoney)
    {
        firstMoney.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        firstMoney.transform.SetParent(playerVisualTransform);
        moneys.Add(firstMoney.gameObject);

        StartCoroutine(MakeFirstMoneyBigger(firstMoney.gameObject));
    
        firstMoney.tag = "Stacked Money";
        firstMoney.GetComponent<BoxCollider>().isTrigger = true;
        GetComponent<BoxCollider>().enabled = false;
    }

    private IEnumerator MakeFirstMoneyBigger(GameObject firstMoney)
    {
        Vector3 previousFirstMoneyScale = new Vector3(1f, 1f, 1f);
        Vector3 newFirstMoneyScale = previousFirstMoneyScale * 1.5f;
        firstMoney.transform.DOScale(newFirstMoneyScale, 0.1f)
            .OnComplete(() => firstMoney.transform.DOScale(previousFirstMoneyScale, 0.1f));
        yield break;
    }

    private void MoveFirstMoney()
    {
        if(moneys.Count > 0)
        {
            moneys[0].transform.DOMoveX(transform.position.x, 0.5f);
        }
    }

    private void CheckMoneyCount()
    {
        if (moneys.Count <= 0)
        {
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}
