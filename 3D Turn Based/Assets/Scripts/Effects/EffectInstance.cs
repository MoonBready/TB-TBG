using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectInstance
{
    public Effect effect;
    public int turnRemaining;

    public GameObject curActiveGameObject;
    public ParticleSystem curTickParticle;

    public EffectInstance (Effect effect)
    {
        this.effect = effect;
        turnRemaining = effect.durationOfTurns;
    }
}
