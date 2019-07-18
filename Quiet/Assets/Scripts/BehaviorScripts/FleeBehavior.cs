using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Flee")]
public class FleeBehavior : AvoidanceBehavior
{
    public LayerMask mask;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (!(Services.Scenes.CurrentScene is GameSceneScript)) return Vector2.zero;
        // If no neighbors, return no adjustment
        if (context.Count == 0) return Vector2.zero;

        // Average all points
        Vector2 avoidancenMove = Vector2.zero;
        int nAvoid = 0;
        List<Transform> filterContext = (filter == null) ? context : filter.Filter(agent, context);

        float avoidanceRadius = flock ? flock.SquareAvoidanceRadius : m_defaultSquareAvoidanceRadius;
        avoidanceRadius *= agent.AnxietyLevel;

        if(!ContextContains(((GameSceneScript)Services.Scenes.CurrentScene).PlayerFlock.Player.transform, context))
        {
            agent.moreAnxious = false;
            agent.SetAnxietyLevel(Mathf.Lerp(agent.AnxietyLevel, FlockAgent.MIN_ANXIETY_LEVEL, Easing.BackEaseOut(Time.deltaTime)));

        }

        foreach (Transform item in filterContext)
        {

            // If an item is within the avoidanceRadius
            if (Vector2.SqrMagnitude(item.position - agent.transform.position) <
                avoidanceRadius && mask == (mask | (1 << item.gameObject.layer)))
            {
                agent.SetAnxietyLevel(Mathf.Lerp(agent.AnxietyLevel, FlockAgent.MAX_ANXIETY_LEVEL, Easing.BackEaseOut(Time.deltaTime)));
                //Add to items to avoid and calculate the direction to move to avoid
                //agent.LERPColor(true);
                agent.moreAnxious = true;
                nAvoid++;
                avoidancenMove += (Vector2)(agent.transform.position - item.position);
            }

        }

        if (nAvoid > 0)
        {
            avoidancenMove /= nAvoid;
        }

        return avoidancenMove;
    }

    public bool ContextContains(Transform obj, List<Transform> context)
    {
        return context.Contains(obj);
    }
}
