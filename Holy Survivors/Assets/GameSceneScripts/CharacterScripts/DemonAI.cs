using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAI : MonoBehaviour
{
    public Player targetPlayer; 
    private float navSpeed;
    private UnityEngine.AI.NavMeshAgent nav;
    private Animation anim;

    void Awake()
    {
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animation>();
    }

    void Update()
    {
        

        if(targetPlayer != null)
        {
            navSpeed = GetComponent<Demon>().getDemonSpeed();
            nav.speed = navSpeed;
			nav.SetDestination(targetPlayer.GetComponent<Transform>().position);

            // Play attack animation when player is close
            float distance = Vector3.Distance(transform.position, targetPlayer.GetComponent<Transform>().position);
            
            if(distance <= 3)
            {
                anim[anim.clip.name].speed = 1;
                anim.Play();
            }
            else
            {
                if(anim[anim.clip.name].clip.length > 0)
                {
                    anim[anim.clip.name].speed = -1;
                }
            }
        }
        else if(GameSceneEventHandler.instance.localPlayer != null)
        {
            targetPlayer = GameSceneEventHandler.instance.localPlayer;
        }
    }
}
