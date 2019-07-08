using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitMotion : MonoBehaviour
{
    public Transform orbitingObject;
    public Ellipse orbitPath;

    [Range(0, 1)]
    public float orbitProgress = 0;
    public float orbitPeriod = 3f;
    public bool orbitActive = true;

    private const float MIN_ORBIT_PERIOD = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        if(orbitingObject == null)
        {
            orbitActive = false;
            return;
        }
        SetOrbitingObjectPosition();
        StartCoroutine(AnimateOrbit());
    }

    void SetOrbitingObjectPosition()
    {
        Vector2 orbitPos = orbitPath.Evaluate(orbitProgress);
        orbitingObject.localPosition = new Vector2(orbitPos.x, orbitPos.y);
    }

    IEnumerator AnimateOrbit()
    {
        if (Mathf.Abs(orbitPeriod) < MIN_ORBIT_PERIOD) orbitPeriod = MIN_ORBIT_PERIOD;

        float orbitSpeed = 1f / orbitPeriod;
        while(orbitActive)
        {
            orbitProgress += Time.deltaTime * orbitSpeed;
            orbitProgress %= 1f;
            SetOrbitingObjectPosition();
            yield return null;
        }
    }
}
