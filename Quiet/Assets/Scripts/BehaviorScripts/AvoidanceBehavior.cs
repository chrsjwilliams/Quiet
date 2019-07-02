using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class AvoidanceBehavior : FlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // If no neighbors, return no adjustment
        if (context.Count == 0) return Vector2.zero;

        // Average all points
        Vector2 avoidancenMove = Vector2.zero;
        int nAvoid = 0;
        foreach (Transform item in context)
        {
            // If an item is within the avoidanceRadius
            if (Vector2.SqrMagnitude(item.position - agent.transform.position) <
                flock.SquareAvoidanceRadius)
            {
                //Add to items to avoid and calculate the direction to move to avoid
                nAvoid++;
                avoidancenMove += (Vector2)(agent.transform.position - item.position);
            }
        }

        if(nAvoid > 0)
        {
            avoidancenMove /= nAvoid;
        }

        return avoidancenMove;
    }
}
