﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShakeState : FSMState<Enemy>
{
    [SerializeField] private float shakingDuration = 3;
    [SerializeField] private float maxShakingForce = 5;
    [SerializeField] private float maxSteeringForce = 1000f;

    private bool isShaking;
    private float counter = 0;
    private SteeringManager steering;
    private Rigidbody rb;

    public override void Enter()
    {
        base.Enter();
        rb = GetComponent<Rigidbody>();
        steering = GetComponent<SteeringManager>();
        isShaking = true;

        steering.SetMaxSteeringForce(maxSteeringForce);
    }

    void FixedUpdate()
    {
        Shake();
        steering.FlockingSeparation(Enemy.allSteerables);
        steering.AvoidObstacles();
        steering.CompensateExternalForces();
        steering.LookWhereGoing();
    }

    private void Shake()
    {
        if (isShaking)
        {
            //rb.AddForce(new Vector3(Random.Range(-2, 2) * maxShakingForce, Random.Range(-2, 2) * maxShakingForce, Random.Range(-2, 2) * maxShakingForce), ForceMode.Impulse);
            rb.AddForce(Random.onUnitSphere * maxShakingForce, ForceMode.Impulse);
            counter += Time.fixedDeltaTime;
            if (counter >= shakingDuration)
            {
                isShaking = false;
                counter = 0;
            }
        }
        else
        {
            rb.useGravity = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
        rb.useGravity = false;

        steering.SetMaxSteeringForce(steering.GetInitalSteeringForce());
    }
}
