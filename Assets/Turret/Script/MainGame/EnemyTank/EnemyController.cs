using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : TurretController
{
    [Header("Config")]
    [SerializeField] private TurretManagerScriptableObject tankManagerValue;

    private Rigidbody rb;

    [SerializeField] private float movementSpeed = 1f;
    float MOVE_SPEED_SCALE = 10f;
    Vector3 targetPosition;

    bool isMoving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            Vector3 targetPos = GetRandomPosition();

            Debug.Log(targetPos);

            // StartCoroutine(MoveAndShotAtPosition(targetPosition));
        }

        // MoveToPosition(targetPosition);
    }

    IEnumerator MoveAndShotAtPosition(Vector3 newTankPosition)
    {
        isMoving = true;
        Vector3 direction;
        float initialDistance = Vector3.Distance(transform.position, newTankPosition);

        while (Vector3.Distance(transform.position, newTankPosition) >= 0.5f)
        {
            direction = newTankPosition - transform.position;
            float currentDistance = Vector3.Distance(transform.position, newTankPosition);

            float speedMultiplier = Mathf.Clamp(currentDistance / initialDistance, 0.5f, 1f);

            transform.rotation = Quaternion.LookRotation(direction);

            rb.AddForce(transform.forward * movementSpeed * MOVE_SPEED_SCALE * speedMultiplier, ForceMode.Acceleration);

            yield return null;
        }

        transform.position = newTankPosition;
        isMoving = false;

        Debug.Log("Done");
    }

    private Vector3 GetRandomPosition()
    {
        float randomX ;
        float randomY;

        do
        {
            randomX = Random.Range(-30f, 30f);
            randomY = Random.Range(-30f, 30f);
        }
        while (Mathf.Abs(randomX) <= 5f || Mathf.Abs(randomY) <= 5f);


        return new Vector3(randomX, 1.5f, randomY);
    }
}
