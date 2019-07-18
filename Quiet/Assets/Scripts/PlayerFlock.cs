using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlock : Flock
{
    private PlayerControlledFlockAgent m_player;
    public PlayerControlledFlockAgent Player { get { return m_player; } }

    public override void Init()
    {
        Flock flock = GetComponent<Flock>();
        FlockAgent newAgent = Instantiate(
                                flock.agentPrefab,
                                Random.insideUnitCircle * flock.startingCount * Flock.AGENT_DENSITY,
                                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                                transform);
        newAgent.gameObject.AddComponent<PlayerControlledFlockAgent>();
        m_player = newAgent.GetComponent<PlayerControlledFlockAgent>();
        m_player.name = "Player";
        Destroy(newAgent.GetComponent<FlockAgent>());
        m_player.Init(this);
    }

    private void Update()
    {
        List<Transform> context = GetNearbyObjects(m_player);
        Vector2 move = behavior.CalculateMove(m_player, context, this);
        move *= driveFactor;
        if (move.sqrMagnitude > m_squareMaxspeed)
        {
            move = move.normalized * maxSpeed;
        }
        m_player.Move(move);
    }
}
