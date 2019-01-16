﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetEffect : Effect
{
    public Rigidbody m_rb;
    public Rigidbody sunRigidbody;

    private MTK_JointType m_joint;

    public float m_maxRadius;
    public bool m_radiusDecided = false;

    public override bool ApplyEffect()
    {
        m_rb = GetComponent<Rigidbody>();
        AkSoundEngine.PostEvent("Planet_Rotation_Play", gameObject);

        m_maxRadius = float.MaxValue;
        
        if(m_lastVel != Vector3.zero)
            m_rb.velocity = m_lastVel;

        if (m_rb && !m_rb.isKinematic)
        {
            ManageEffectsCollisions();
            m_rb.useGravity = false;
            m_joint = GetComponent<MTK_JointType>();

            return true;
        }
        else
        {
            Destroy(this);
            return false;
        }
    }

    void ManageEffectsCollisions()
    {
        LevitationEffect levEffect = GetComponent<LevitationEffect>();
        if (levEffect)
            Destroy(levEffect);
    }

    public override void RemoveEffect()
    {
        if (m_rb)
        {
            m_lastVel = m_rb.velocity;
            m_rb.useGravity = true;
        }

        AkSoundEngine.PostEvent("Planet_Rotation_Stop", gameObject);
    }

    Vector3 axis;
    float baseVel;
    Vector3 m_lastVel;
    private void FixedUpdate()
    {
        Vector3 dir = transform.position - sunRigidbody.transform.position;
        float radius = dir.magnitude;
        dir.Normalize();
        if (!m_joint.Used())
        {
            if (radius < m_maxRadius)
            {
                m_maxRadius = radius;
                axis = Vector3.Cross(m_rb.velocity, dir).normalized;
                baseVel = m_rb.velocity.magnitude;
            }
            else
            {
                transform.position = sunRigidbody.transform.position +  m_maxRadius * (transform.position - sunRigidbody.transform.position).normalized;
                Vector3 dir2 = (transform.position - sunRigidbody.transform.position).normalized;
                m_rb.velocity = baseVel * Vector3.Cross(dir2, axis);
            }
        }
        else
        {
            m_maxRadius = float.MaxValue;
        }
    }
}
