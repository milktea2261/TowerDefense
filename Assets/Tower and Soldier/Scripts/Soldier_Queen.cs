using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//激勵隊友
public class Soldier_Queen : Soldier
{
    [SerializeField] ParticleSystem particle;

    public float powerTimeInterval = 15f;
    private float powerTimer = 0;

    protected override void Update() {
        base.Update();

        if(powerTimer <= 0) {
            Debug.Log("power up");
            PowerUp();
            powerTimer = powerTimeInterval;
        }
        powerTimer -= Time.deltaTime;
    }

    private void PowerUp() {
        foreach(Unit unit in unitsInRange) {
            if(unit.tag == tag) {
                UseSpeicalAblity(unit);
            }
        }
        particle.Play();
    }

}
