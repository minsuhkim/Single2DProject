using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }

    private Dictionary<ParticleType, GameObject> particlePrefabDict = new Dictionary<ParticleType, GameObject>();
    private Dictionary<ParticleType, Queue<GameObject>> particlePools = new Dictionary<ParticleType, Queue<GameObject>>();

    public GameObject playerAttackEffectPrefab;
    public GameObject playerDamageEffectPrefab;

    public int poolSize = 10;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        particlePrefabDict.Add(ParticleType.PlayerAttack, playerAttackEffectPrefab);
        particlePrefabDict.Add(ParticleType.PlayerDamage, playerDamageEffectPrefab);

        foreach(var type in particlePrefabDict.Keys)
        {
            Queue<GameObject> pool = new Queue<GameObject>();
            for(int i=0; i<poolSize; i++)
            {
                GameObject obj = Instantiate(particlePrefabDict[type]);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
            particlePools.Add(type, pool);
        }
    }

    public void ParticlePlay(ParticleType type, Vector3 position, Vector3 scale)
    {
        if (particlePools.ContainsKey(type))
        {
            GameObject particleObj = particlePools[type].Dequeue();

            if(particleObj != null)
            {
                particleObj.transform.position = position;
                particleObj.transform.localScale = scale;
                particleObj.SetActive(true);

                Animator animator = particleObj.GetComponent<Animator>();
                if(animator != null)
                {
                    animator.Play(0);
                    StartCoroutine(AnimationEndCoroutine(type, particleObj, animator));
                }
            }
        }
    }

    IEnumerator AnimationEndCoroutine(ParticleType type, GameObject obj, Animator animator)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        obj.SetActive(false);
        particlePools[type].Enqueue(obj);
    }
}

public enum ParticleType
{
    PlayerAttack,
    PlayerDamage,
}
