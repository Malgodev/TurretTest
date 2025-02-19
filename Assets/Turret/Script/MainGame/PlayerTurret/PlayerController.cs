using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] private Transform turret;

    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        
    }

    void Update()
    {
        TurnTurret();

        if (Input.GetMouseButton(0))
        {
            GetMouseWorldPosition();
        }
    }

    private void TurnTurret()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        if (mousePosition == Vector3.zero)
        {
            return;
        }

        Vector3 direction = mousePosition - transform.position;
        turret.rotation = Quaternion.LookRotation(direction);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPosition = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            worldPosition = hit.point;
        }

        return worldPosition;
    }
}
