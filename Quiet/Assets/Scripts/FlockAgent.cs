using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{

    Flock m_agentFlock;
    public Flock AgentFlock { get { return m_agentFlock; } }

    Collider2D m_agentCollider;
    public Collider2D AgentCollider { get { return m_agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
        m_agentCollider = GetComponent<Collider2D>();   
    }

    public void Init(Flock flock)
    {
        m_agentFlock = flock;
    }

    public void Move(Vector2 velocity)
    {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
}
