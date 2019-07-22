using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    public enum AgentStatus { NORMAL = 0, ANXIOUS }
    protected Flock m_agentFlock;
    public Flock AgentFlock { get { return m_agentFlock; } }

    protected Collider2D m_agentCollider;
    public Collider2D AgentCollider { get { return m_agentCollider; } }

    protected Rigidbody2D m_rigidBody2D;
    public Rigidbody2D Rigidbody2D {  get { return m_rigidBody2D; } }

    protected SpriteRenderer spriteRenderer;
    protected Color anxiousColor = new Color(1.0f, 103f/255f, 0f);
    protected Color naturalColor = new Color(0f, 78f/255f, 152f/255f);

    private AgentStatus m_status = AgentStatus.NORMAL;
    public AgentStatus Status { get { return m_status; } }

    [SerializeField]
    private float m_anxietyLevel;
    public float AnxietyLevel { get { return m_anxietyLevel; } }
    public const float ANXIETY_INCREMENT = 1.5f;
    public const float ANXIETY_DECREMENT = 0.5f;
    public const float MAX_ANXIETY_LEVEL = 15.0f;
    public const float MIN_ANXIETY_LEVEL = 0.5f;

    public bool moreAnxious = false;

    private bool m_contextContainsPlayer = false;
    public bool ContextContainsPlayer {  get { return m_contextContainsPlayer; } }
    

    // Start is called before the first frame update
    void Start()
    {
        m_agentCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        m_rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void SetContextContainsPlayer(bool containsPlayer)
    {
        m_contextContainsPlayer = containsPlayer;
    }

    public void SetFlock(Flock newFlock)
    {
        m_agentFlock = newFlock;
    }

    public virtual void Init(Flock flock)
    {
        m_agentFlock = flock;
        m_anxietyLevel = Random.Range(0.0f, 3.0f);
    }

    public virtual void Move(Vector2 velocity)
    {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    public void SetAnxietyLevel(float level)
    {
        m_anxietyLevel = level;
        if(m_anxietyLevel > MAX_ANXIETY_LEVEL * 0.75f)
        {
            m_status = AgentStatus.ANXIOUS;
        }
        else
        {
            m_status = AgentStatus.NORMAL;
        }
    }

    private void Update()
    {
        if (moreAnxious)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, anxiousColor, Time.deltaTime);
        }
        else
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, naturalColor, Time.deltaTime);
        }

    }

    public void LERPColor(bool moreAnxious)
    {
        // FIX: COLOR LERP DOES NOT WORK
        Debug.Log(Time.deltaTime);
        

    }
}
