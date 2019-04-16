using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeOrbBehaviour : SoulBehaviour {

    private bool stopped;
    public Gradient gradient;

	
	// Update is called once per frame
	public override void Update () {
        base.Update();

        if(!stopped && (targetPoint - transform.position).magnitude < 0.1f)
        {
            StopMoving();
            StartCoroutine("WaitThenDie");
        }
    }

    public void StopMoving()
    {
        forwardSpeed = 0;
        turnSpeed = 0;
        stopped = true;

        var ps = GetComponent<ParticleSystem>();
        ps.Stop();
        var main = ps.main;
        main.stopAction = ParticleSystemStopAction.Destroy;
    }

    public void SetNewGradient(float percent)
    {
        var ps = GetComponent<ParticleSystem>();
        var col = ps.colorOverLifetime;
        col.enabled = true;

        GradientColorKey Ckey1 = new GradientColorKey(new Color(0.59f, 0.59f, 0.59f, 1), 0.1f);
        GradientColorKey Ckey2 = new GradientColorKey(gradient.Evaluate(percent), 0.39f);

        GradientAlphaKey AKey1 = new GradientAlphaKey(0, 0);
        GradientAlphaKey AKey2 = new GradientAlphaKey(1, 0.47f);
        GradientAlphaKey AKey3 = new GradientAlphaKey(1, 0.68f);
        GradientAlphaKey AKey4 = new GradientAlphaKey(0, 1);


        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { Ckey1, Ckey2 }, new GradientAlphaKey[] { AKey1, AKey2, AKey3, AKey4 });

        col.color = grad;
    }
}
