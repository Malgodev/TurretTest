using System.Collections;
using System.Collections.Generic;
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
            Vector2 randomPoint = Random.insideUnitCircle * 15f;

            targetPosition = new Vector3(randomPoint.x, 1.5f, randomPoint.y);

            StartCoroutine(MoveAndShotAtPosition(targetPosition));
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

            float speedMultiplier = Mathf.Clamp(currentDistance / initialDistance, 0.3f, 1f);

            Debug.Log($"Position: {transform.position}, Target: {newTankPosition}, Speed Multiplier: {speedMultiplier}");

            transform.rotation = Quaternion.LookRotation(direction);
            rb.AddForce(transform.forward * movementSpeed * MOVE_SPEED_SCALE * speedMultiplier, ForceMode.Acceleration);
            yield return null;
        }

        transform.position = newTankPosition;
        isMoving = false;
    }
}
