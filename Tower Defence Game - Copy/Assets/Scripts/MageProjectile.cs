using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageProjectile : MonoBehaviour
{
    ProjectileUpgradeManager projectileUpgradeManager;

    public enum Dropdown { Freeze, Slow, Damage};

    public Dropdown projectileType;

    public GameObject freezehitBox;

    [Header("Ability 1")]
    float speed = 15f;
    public bool isFreeze;
    public float freezeLifeTime;

    [Header("Ability 2")]
    public float slowDuration;
    

    [Header("Ability 4")]
    public int dmgUseAmount;
    public float dmgDuration;
    public bool isDamage;
    // Make a game object w item inside, decrease there.
    public GameObject[] stacks;
    //public int stacksInt;


    private void Start()
    {
        projectileUpgradeManager = FindObjectOfType<ProjectileUpgradeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (projectileType)
        {
            case Dropdown.Slow:
                StartCoroutine(SlowDuration());
                break;
            case Dropdown.Freeze:
                isFreeze = true;
                StartCoroutine(FreezeAbility());
                break;
            case Dropdown.Damage:
                isDamage = true;
                print(stacks.Length);
                StartCoroutine(DamageDuration());
                break;
        }
    }

    // Do blinking here
    public IEnumerator SlowDuration()
    {
        yield return new WaitForSeconds(slowDuration);
        Destroy(gameObject);
    }

    IEnumerator FreezeAbility()
    {
         //Ability 1 (Freeze)
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        yield return new WaitForSeconds(freezeLifeTime);
        Destroy(gameObject);
    }
    IEnumerator DamageDuration()
    {
        yield return new WaitForSeconds(dmgDuration);
        Destroy(gameObject);
    }
    void DecreaseStacks()
    {
        for (int i = 0; i < stacks.Length; i++)
        {
            if (dmgUseAmount <= 0)
            {
                Destroy(gameObject);
            }
            stacks[dmgUseAmount].gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && isDamage)
        {
            dmgUseAmount--;
            DecreaseStacks();
        }

        if (other.CompareTag("Enemy") && isFreeze)
        {
            freezehitBox.transform.localScale = projectileUpgradeManager.freezeSize;
            Instantiate(freezehitBox, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }
}
