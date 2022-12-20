using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRangeAttributer : MonoBehaviour
{
    [Tooltip("Only put in the Particle Systems that have a variable range.")]
    public ParticleSystem[] particleSystems;
    [Tooltip("If ticked/true, the radius will stay at 1 but the particles will travel throughout the AoE.")]
    public bool disperseArea;
    public float defaultRange;
    private void Start()
    {
        AttributeRange(defaultRange);
    }
    public int AttributeRange(float _range)
    {
        foreach (ParticleSystem system in particleSystems)
        {
            if(!disperseArea)
            {
                var shape = system.shape;
                shape.enabled = true;
                shape.shapeType = ParticleSystemShapeType.Donut;
                shape.radius = _range;
            } else
            {
                var shape = system.shape;
                shape.enabled = true;
                shape.shapeType = ParticleSystemShapeType.Donut;
                shape.radius = 1;
                var velocityOverLifetime = system.velocityOverLifetime;
                velocityOverLifetime.enabled = true;
                velocityOverLifetime.speedModifier = -_range / system.main.startLifetime.constant;
            }
        }
        return particleSystems.Length;
    }
}
