using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private Material conveyorMaterial;
    [SerializeField] private float offsetSpeed;

    private PlayerController playerController;

    private float offsetY = 0;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        offsetY -= Time.deltaTime * offsetSpeed;
        conveyorMaterial.mainTextureOffset = new Vector2(0f, offsetY);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            playerController.ResetPlayerSpeed();
            GameManager.Instance.OnGameFinish();
        }
    }
}
