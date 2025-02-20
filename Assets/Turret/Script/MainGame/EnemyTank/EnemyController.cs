using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : TurretController
{
    private Rigidbody rb;

    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float rotationSpeed = 0.1f;
    float MOVE_SPEED_SCALE = 10f;

    [SerializeField] private Slider healthBar;
    [SerializeField] private Canvas canvas;


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

        // MoveToPosition(targetPosition);
    }

    private void LateUpdate()
    {
        canvas.transform.LookAt(canvas.transform.position + Camera.main.transform.forward);
    }

    IEnumerator MoveAndShotAtPosition(Vector3 newTankPosition)
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, newTankPosition) >= 0.5f)
        {
            Vector3 direction = newTankPosition - transform.position;

            transform.rotation = Quaternion.LookRotation(direction);

            rb.AddForce(transform.forward * movementSpeed * MOVE_SPEED_SCALE, ForceMode.Acceleration);

            yield return null;
        }

        transform.position = newTankPosition;

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

        // Shot
        StartCoroutine(Fire());


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
        if (health - damage <= 0)
        {
            GameManager.Instance.IncreasePoint();
        }

        base.GettingDamage(damage);
        healthBar.value = health;
    }
}
