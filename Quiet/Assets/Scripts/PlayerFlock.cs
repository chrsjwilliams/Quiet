using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlock : Flock
{
    private PlayerControlledFlockAgent player;

    private void Start()
    {
        Flock flock = GetComponent<Flock>();
        FlockAgent newAgent = Instantiate(
                                flock.agentPrefab,
                                Random.insideUnitCircle * flock.startingCount * Flock.AGENT_DENSITY,
                                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                                transform);
        newAgent.gameObject.AddComponent<PlayerControlledFlockAgent>();
        player = newAgent.GetComponent<PlayerControlledFlockAgent>();
        player.name = "Player";
        Destroy(newAgent.GetComponent<FlockAgent>());
        player.Init(this);
    }

    private void Update()
    {
        List<Transform> context = GetNearbyObjects(player);
        Vector2 move = behavior.CalculateMove(player, context, this);
        move *= driveFactor;
        if (move.sqrMagnitude > m_squareMaxspeed)
        {
            move = move.normalized * maxSpeed;
        }
        player.Move(move);
    }
}
