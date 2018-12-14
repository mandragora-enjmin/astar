﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleEffect : Effect
{
    public Vector3 originalScale;// { get; private set; }

    public override bool ApplyEffect()
    {
        originalScale = transform.localScale;

        return true;
    }

    public override void RemoveEffect()
    {
        transform.localScale = originalScale;
    }
}