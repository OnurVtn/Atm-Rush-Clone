using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance => instance ?? (instance = FindObjectOfType<GameManager>());

    [SerializeField] private GameObject successImage, nextButton;

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

    public void OnGameFinish()
    {
        StartCoroutine(FinishUI());
    }

    private IEnumerator FinishUI()
    {
        successImage.transform.DOMoveY(successImage.transform.position.y - 200f, 1.5f)
            .SetEase(Ease.OutBack);

        yield return new WaitForSeconds(2f);

        nextButton.transform.DOScale(Vector3.one, 1.5f)
            .SetEase(Ease.OutBack);
    }
}
