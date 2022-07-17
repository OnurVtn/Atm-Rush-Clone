using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Atm : MonoBehaviour
{
    [SerializeField] private TextMeshPro atmNumberText;

    private int atmNumber;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stacked Money"))
        {
            atmNumber ++;
            atmNumberText.text = atmNumber.ToString();
            other.gameObject.SetActive(false);
            Stacker.Instance.moneys.Remove(other.gameObject);
        }

        if (other.CompareTag("Gold"))
        {
            atmNumber += 2;
            atmNumberText.text = atmNumber.ToString();
            other.gameObject.SetActive(false);
            Stacker.Instance.moneys.Remove(other.gameObject);
        }

        if (other.CompareTag("Diamond"))
        {
            atmNumber += 3;
            atmNumberText.text = atmNumber.ToString();
            other.gameObject.SetActive(false);
            Stacker.Instance.moneys.Remove(other.gameObject);
        }

        if (other.CompareTag("Character"))
        {
            playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z - 15f);
        }
    }
}
