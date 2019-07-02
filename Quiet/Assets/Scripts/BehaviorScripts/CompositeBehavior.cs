using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Composite")]
public class CompositeBehavior : FlockBehaviour
{
    public FlockBehaviour[] behaviours;
    public float[] weights;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // Handle data mismatch
        if(weights.Length != behaviours.Length)
        {
            Debug.LogError("Data mismatch in " + name, this);
            return Vector2.zero;
        }

        // Set up move
        Vector2 move = Vector2.zero;

        // Iterate through behaviors
        for(int i = 0; i < behaviours.Length; i++)
        {
            Vector2 partialMove = behaviours[i].CalculateMove(agent, context, flock) * weights[i];

            // IF we get some movement from the behaviors
            if(partialMove != Vector2.zero)
            {
                // Check to see if movement exceeds the weight
                if(partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    // Set partialMOve to that weight
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }
                // Combine moves from all behaviors
                move += partialMove;
            }
        }
        return move;
    }
}
