using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{

    protected Flock m_agentFlock;
    public Flock AgentFlock { get { return m_agentFlock; } }

    protected Collider2D m_agentCollider;
    public Collider2D AgentCollider { get { return m_agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
        m_agentCollider = GetComponent<Collider2D>();   
    }

    public void SetFlock(Flock newFlock)
    {
        m_agentFlock = newFlock;
    }

    public virtual void Init(Flock flock)
    {
        m_agentFlock = flock;
    }

    public virtual void Move(Vector2 velocity)
    {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
}
