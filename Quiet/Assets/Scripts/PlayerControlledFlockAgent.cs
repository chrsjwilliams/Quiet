using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlledFlockAgent : FlockAgent
{
    private bool m_arrivedAtTarget;

    private int m_touchID;
    private bool m_touchInputActive;
    public bool TouchInputActive { get { return m_touchInputActive; } }

    private Vector3 m_targetPos;
    public Vector3 TargetPosition { get { return m_targetPos; } }

    private Vector3 m_initalTouchPos;
    public Vector3 InitalTouchPos { get { return m_initalTouchPos; } }

    private void Start()
    {
        m_agentCollider = GetComponent<Collider2D>();
    }

    public override void Init(Flock flock)
    {
        m_agentFlock = flock;
        gameObject.tag = "Player";
        gameObject.layer = 9;
        Unlock();
    }

    public override void Move(Vector2 velocity)
    {
        if (m_touchID == -1 && !m_touchInputActive)
        {
            transform.up = velocity;
            transform.position += (Vector3)velocity * Time.deltaTime;
        }
        else
        {   
            Vector3 posDifference = transform.localPosition - TargetPosition;
            Vector2 direction = posDifference.normalized;
            float initDistance = Vector3.Distance(transform.localPosition, InitalTouchPos);
            float distance = Vector3.Distance(transform.localPosition, TargetPosition);

            Vector3 newPos = Vector3.Lerp(transform.localPosition, TargetPosition, Time.deltaTime);
            transform.localPosition = new Vector3(newPos.x, newPos.y);
            
            float zRotation = Mathf.Atan2(direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0, 0, 270 - zRotation);
        }
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Flock flock = GameObject.Find("FlockBlue").GetComponent<Flock>();
        }
    }

    #region Touch Input
    public void Lock()
    {
        HideFromInput();
    }

    public void Unlock()
    {
        ListenForInput();
    }

    protected void ListenForInput()
    {

        Services.GameEventManager.Register<TouchDown>(OnTouchDown);
        Services.GameEventManager.Register<MouseDown>(OnMouseDownEvent);
        m_touchInputActive = false;
        m_touchID = -1;

    }

    protected void HideFromInput()
    {
        Services.GameEventManager.Unregister<TouchDown>(OnTouchDown);
        Services.GameEventManager.Unregister<MouseDown>(OnMouseDownEvent);
    }

    protected void OnTouchDown(TouchDown e)
    {
        Vector3 touchWorldPos = Services.GameManager.MainCamera.ScreenToWorldPoint(e.touch.position);
        if (m_touchID == -1)
        {
            m_touchID = e.touch.fingerId;
            OnInputDown();
        }
    }

    protected void OnMouseDownEvent(MouseDown e)
    {
        Vector3 mouseWorldPos = Services.GameManager.MainCamera.ScreenToWorldPoint(e.mousePos);
        m_targetPos = mouseWorldPos;
        m_initalTouchPos = transform.localPosition;
        OnInputDown();

    }

    protected void OnTouchUp(TouchUp e)
    {
        if (e.touch.fingerId == m_touchID)
        {
            OnInputUp();
            m_touchID = -1;
        }
    }

    protected void OnMouseUpEvent(MouseUp e)
    {
        OnInputUp();
    }

    public virtual void OnInputDown()
    {
        m_touchInputActive = true;
        Services.AudioManager.RegisterSoundEffect(Services.Clips.PiecePicked);


        Services.GameEventManager.Register<TouchUp>(OnTouchUp);
        Services.GameEventManager.Register<TouchMove>(OnTouchMove);
        Services.GameEventManager.Unregister<TouchDown>(OnTouchDown);

        Services.GameEventManager.Register<MouseUp>(OnMouseUpEvent);
        Services.GameEventManager.Register<MouseMove>(OnMouseMoveEvent);
        Services.GameEventManager.Unregister<MouseDown>(OnMouseDownEvent);

    }

    protected void OnMouseMoveEvent(MouseMove e)
    {
        OnInputDrag(Services.GameManager.MainCamera.ScreenToWorldPoint(e.mousePos));
    }

    protected void OnTouchMove(TouchMove e)
    {
        if (e.touch.fingerId == m_touchID)
        {
            OnInputDrag(Services.GameManager.MainCamera.ScreenToWorldPoint(e.touch.position));
        }
    }

    public virtual void OnInputUp(bool forceCancel = false)
    {
        m_touchInputActive = false;


        Services.GameEventManager.Unregister<TouchMove>(OnTouchMove);
        Services.GameEventManager.Unregister<TouchUp>(OnTouchUp);
        Services.GameEventManager.Unregister<MouseMove>(OnMouseMoveEvent);
        Services.GameEventManager.Unregister<MouseUp>(OnMouseUpEvent);

        ListenForInput();
    }

    public virtual void OnInputDrag(Vector3 inputPos)
    {
        m_targetPos = inputPos;   
    }

    public void CheckTouchStatus()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) return;
        for (int i = 0; i < Input.touches.Length; i++)
        {
            if (Input.touches[i].fingerId == m_touchID) return;
        }
        OnInputUp(true);
    }
    #endregion

}
