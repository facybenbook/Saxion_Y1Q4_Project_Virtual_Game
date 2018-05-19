﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

public class EnemyStateFollowPlayer : FSMState<Enemy>
{
    [SerializeField] private float maxDistanceToPlayer = 5f;
    [SerializeField] private float arriveSlowdownDistance = 5f;
    [SerializeField] private float lookAtPlayerDistance = 10f;
    [SerializeField] private GameObject toPlayerParticleGroup;

    private ParticleManager particleManager;
    private GameObject target;
    private Shooting shootingController;
    private SteeringManager steeringManager;

    void Start()
    {
        shootingController = GetComponent<Shooting>();
        shootingController.enabled = true;
        target = GameObject.FindGameObjectWithTag("Player");
        steeringManager = GetComponent<SteeringManager>();
        particleManager = GetComponentInChildren<ParticleManager>();
        particleManager.ChangeParticleGroup(toPlayerParticleGroup);

    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        Vector3 toPlayer = target.transform.position - transform.position;
        Vector3 desiredPos = target.transform.position - toPlayer.normalized * maxDistanceToPlayer;

        steeringManager.Seek(desiredPos, arriveSlowdownDistance);
        steeringManager.FlockingSeparation(Enemy.allSteerables);
        steeringManager.AvoidObstacles();
        steeringManager.Wander();

        if (toPlayer.magnitude > lookAtPlayerDistance)
        {
            steeringManager.LookWhereGoing();
        }
        else
        {
            steeringManager.LookAt(target.transform.position);
        }
    }
    public override void Exit()
    {
        base.Exit();
        shootingController.enabled = false;
    }
}

