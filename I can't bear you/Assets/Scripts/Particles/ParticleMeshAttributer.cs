using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMeshAttributer : MonoBehaviour
{
    private MeshRenderer meshToAttribute;
    [Tooltip("Only put in the Particle Systems that need a mesh as emissive shape.")]
    public ParticleSystem[] particleSystems;
    public int AttributeMesh(MeshRenderer _mesh)
    {
        foreach (ParticleSystem system in particleSystems)
        {
            var shape = system.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.MeshRenderer;
            shape.meshRenderer = meshToAttribute;
        }
        return particleSystems.Length;
    }
}
