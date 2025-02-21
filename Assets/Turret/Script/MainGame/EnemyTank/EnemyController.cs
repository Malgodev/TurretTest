using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class EnemyController : TurretController
{
    private Rigidbody rb;

    [Header("UI Controller")]
    [SerializeField] private EnemyUIController enemyUIController;

    [Header("Stat")] 
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float avoidanceRadius;
    [SerializeField] private float avoidanceForce;
    [SerializeField] private LayerMask tankLayer;
    float MOVE_SPEED_SCALE = 10f;

    bool isMoving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            Vector3 targetPos = GameManager.GetRandomPosition(5f);

            StartCoroutine(MoveAndShotAtPosition(targetPos));
        }
    }

    IEnumerator MoveAndShotAtPosition(Vector3 newTankPosition)
    {
        isMoving = true;

        Vector3 targetDirection;

        while (Vector3.Distance(transform.position, newTankPosition) >= 0.5f)
        {
            targetDirection = (newTankPosition - transform.position).normalized;
            targetDirection.y = 0;

            Vector3 avoidanceDirection = Vector3.zero;
            int avoidanceCount = 0;

            Collider[] nearbyTanks = Physics.OverlapSphere(transform.position, avoidanceRadius, tankLayer);

            foreach (Collider tankCollider in nearbyTanks)
            {
                if (tankCollider.gameObject != gameObject)
                {
                    Vector3 awayFromTank = transform.position - tankCollider.transform.position;
                    float distance = awayFromTank.magnitude;

                    if (distance > 0.1f)
                    {
                        float strength = 1.0f - (distance / avoidanceRadius);
                        avoidanceDirection += awayFromTank.normalized * strength;
                        avoidanceCount++;
                    }
                }
            }

            if (avoidanceCount > 0)
            {
                avoidanceDirection /= avoidanceCount;
                avoidanceDirection.y = 0;
            }

            Vector3 finalDirection = (targetDirection + avoidanceDirection * avoidanceForce).normalized;
            finalDirection.y = 0;

            rb.velocity = finalDirection * movementSpeed;

            if (rb.velocity.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(rb.velocity);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Smooth turning
            }

            yield return null;
        }

        rb.velocity = Vector3.zero;

        // Rotation

        Vector3 playerPosition = GameManager.Instance.PlayerController.transform.position;
        playerPosition.y = 1.5f;

        Quaternion playerDirection = Quaternion.LookRotation(playerPosition - transform.position);
        Quaternion originDirection = turret.rotation;

        while (Quaternion.Angle(turret.rotation, playerDirection) >= 0.1f)
        {
            turret.rotation = Quaternion.RotateTowards(turret.rotation, playerDirection, rotationSpeed * Time.deltaTime);

            yield return null;
        }

        turret.rotation = playerDirection;

        // StartCoroutine(Fire());


        while (Quaternion.Angle(turret.rotation, originDirection) >= 0.1f)
        {
            turret.rotation = Quaternion.RotateTowards(turret.rotation, originDirection, rotationSpeed * Time.deltaTime);

            yield return null;
        }

        turret.rotation = originDirection;

        isMoving = false;
    }

    public override void GettingDamage(int damage)
    {
        base.GettingDamage(damage);

        enemyUIController.SetHealthBar(health);

        if (health <= 0)
        {
            GameManager.Instance.OnEnemyKilled?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
