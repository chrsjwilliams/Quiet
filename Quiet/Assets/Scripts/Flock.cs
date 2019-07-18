using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    /*
     *  TODO:
     *          Color change based on number of neighbors?
     *          Random Walk behabior
     *          Surround behavior
     * 
     */ 

    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour behavior;

    [Range(1, 500)]
    public int startingCount = 250;
    public const float AGENT_DENSITY = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5;
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    protected float m_squareMaxspeed;
    protected float m_squareNeightborRadius;
    protected float m_squareAvoidanceRadius;

    public float SquareAvoidanceRadius { get { return m_squareAvoidanceRadius; } }

    void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    public virtual void Init()
    {
        m_squareMaxspeed = maxSpeed * maxSpeed;
        m_squareNeightborRadius = neighborRadius * neighborRadius;
        m_squareAvoidanceRadius = m_squareNeightborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;



        for(int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle* startingCount * AGENT_DENSITY,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );

            newAgent.name = "Agent " + i;
            newAgent.Init(this);
            agents.Add(newAgent);
        }
    }

    protected void RemoveAgentFromFlock(FlockAgent agent)
    {
        if(agents.Contains(agent))
        {
            agents.Remove(agent);
            agent.SetFlock(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);
            Vector2 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if(move.sqrMagnitude > m_squareMaxspeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    protected List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach(Collider2D c in contextColliders)
        {
            if(c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
}
