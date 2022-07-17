using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Money : MonoBehaviour
{  
    [SerializeField] private Mesh goldMesh, diamondMesh;
    [SerializeField] private Material goldMaterial, diamondMaterial;
    [SerializeField] private GameObject moneyParticle, goldParticle;

    private Transform playerVisualTransform, stackerTransform, lastAtmTransform;

    void Start()
    {
        playerVisualTransform = GameObject.FindGameObjectWithTag("Player Visual").transform;
        stackerTransform = GameObject.FindGameObjectWithTag("Stacker").transform;
        lastAtmTransform = GameObject.FindGameObjectWithTag("LastAtm").transform;
    }

    void Update()
    {
        MoveMoneys();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Money"))
        {
            if (!Stacker.Instance.moneys.Contains(other.gameObject))
            {
                StackMoneys(other);
            }     
        }

        if (other.CompareTag("Obstacle"))
        {
            Stacker.Instance.moneys.Remove(this.gameObject);
            transform.parent = null;
            this.gameObject.SetActive(false);

            if(this.CompareTag("Stacked Money"))
            {
                GameObject moneyEffect = Instantiate(moneyParticle, transform.position, Quaternion.identity);
                Destroy(moneyEffect, 5f);
            }

            if (this.CompareTag("Gold") || this.CompareTag("Diamond"))
            {
                GameObject goldEffect = Instantiate(goldParticle, transform.position, Quaternion.identity);
                Destroy(goldEffect, 5f);
            }
        }

        if (other.CompareTag("Conveyor"))
        {
            Stacker.Instance.moneys.Remove(this.gameObject);
            transform.parent = null;
            transform.DOMoveX(lastAtmTransform.position.x, 1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            if (this.CompareTag("Stacked Money"))
            {
                GetComponent<MeshFilter>().mesh = goldMesh;
                GetComponent<MeshRenderer>().material = goldMaterial;
                transform.localScale = new Vector3(2f, 4f, 4f);
                Destroy(GetComponent<BoxCollider>());
                this.gameObject.AddComponent<BoxCollider>().isTrigger = true;

                StartCoroutine(Delay());
            }

            else if (this.CompareTag("Gold"))
            {
                GetComponent<MeshFilter>().mesh = diamondMesh;
                GetComponent<MeshRenderer>().material = diamondMaterial;
                transform.localScale = new Vector3(2f, 2.25f, 2f);
                Destroy(GetComponent<BoxCollider>());
                this.gameObject.AddComponent<BoxCollider>().isTrigger = true;
                this.tag = "Diamond";
            }
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.2f);
        this.tag = "Gold";
    }

    private void StackMoneys(Collider money)
    {
        float currentStackedMoneyNumber = Stacker.Instance.moneys.Count;
        money.transform.position = new Vector3(stackerTransform.position.x, stackerTransform.position.y, stackerTransform.position.z + currentStackedMoneyNumber);
        money.transform.SetParent(playerVisualTransform);
        Stacker.Instance.moneys.Add(money.gameObject);

        StartCoroutine(MakeMoneysBigger());

        money.tag = "Stacked Money";
        money.GetComponent<BoxCollider>().isTrigger = true;
    }

    private IEnumerator MakeMoneysBigger()
    {
        for(int i = Stacker.Instance.moneys.Count - 1; i >= 0; i--)
        {
            int index = i;
            
            if (Stacker.Instance.moneys[index].CompareTag("Diamond"))
            {
                Vector3 previousMoneyScale = new Vector3(2f, 2.25f, 2f);
                Vector3 newMoneyScale = previousMoneyScale * 1.5f;
                Stacker.Instance.moneys[index].transform.DOScale(newMoneyScale, 0.1f)
                    .OnComplete(() => Stacker.Instance.moneys[index].transform.DOScale(previousMoneyScale, 0.1f));
                yield return new WaitForSeconds(0.05f);
            }

            if (Stacker.Instance.moneys[index].CompareTag("Gold"))
            {
                Vector3 previousMoneyScale = new Vector3(2f, 4f, 4f);
                Vector3 newMoneyScale = previousMoneyScale * 1.5f;
                Stacker.Instance.moneys[index].transform.DOScale(newMoneyScale, 0.1f)
                    .OnComplete(() => Stacker.Instance.moneys[index].transform.DOScale(previousMoneyScale, 0.1f));
                yield return new WaitForSeconds(0.05f);
            }

            if (Stacker.Instance.moneys[index].CompareTag("Stacked Money"))
            {
                Vector3 previousMoneyScale = new Vector3(1f, 1f, 1f);
                Vector3 newMoneyScale = previousMoneyScale * 1.5f;
                Stacker.Instance.moneys[index].transform.DOScale(newMoneyScale, 0.1f)
                    .OnComplete(() => Stacker.Instance.moneys[index].transform.DOScale(previousMoneyScale, 0.1f));
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    private void MoveMoneys()
    {
        if (Stacker.Instance.moneys.Count > 0)
        {
            for (int i = Stacker.Instance.moneys.Count - 1; i > 0; i--)
            {
                Stacker.Instance.moneys[i].transform.DOMoveX(Stacker.Instance.moneys[i - 1].transform.position.x, 0.5f);
            }
        }
    }
}
