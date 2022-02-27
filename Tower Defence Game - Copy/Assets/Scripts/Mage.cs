using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Mage : MonoBehaviour
{
    // Etc
    UIcontroller uiController;
    public Player playerCS;

    public GameObject rangeIndicator;
    public GameObject aim;
    public LayerMask whatIsGround;

    public Image selection;

    public GameObject ability1;
    public GameObject ability2;
    public GameObject ability3;
    public GameObject ability4;

    enum Abilities { Freeze, Slow, AtkSpeed, Damage };
    Abilities ability;

    [Header("Cooldown UI")]
    public Image freezeCooldownUI;
    public Image slowCooldownUI;
    public Image boostCooldownUI;
    public Image dmageCooldownUI;

    [Header("Ability 1 Freeze")]
    // Ability 1
    // For Ability UI
    public Vector3 position;
    public Canvas ability1Canvas;
    public Image skillshot;
    public Transform player;
    public GameObject Freeze;
    bool freezeUiShown = false;

    // For cooldown
    public float freezeCooldown = 8;
    public static bool freezeCooldownBool;
    public static int freezeLevel;

    [Header("Ability 2 Slow")]
    // Ability 2
    // For Ability UI
    public Image targetCircle;
    public Image indicatorRangeCircle;
    public Canvas ability2Canvas;
    public GameObject projectileM;
    bool slowUiShown = false;

    // Range
    public float maxAbility2Range;
    public float range;

    // For Cooldown
    public static bool slowCooldownBool;
    public float slowCooldown;
    public static int slowLevel;


    [Header("Ability 3 AttackSpeed")]
    public GameObject coffeeRadius;
    public Image targetCircleCoffee;
    public Image indicatorRangeCircleCoffee;
    public Canvas ability2CanvasCoffee;
    public float maxAbility2RangeCoffee;
    bool CoffeeUiShown = false;

    // For Cooldown
    public static bool atkspeedCooldownBool;
    public float atkCooldown;
    public static int atkSpeedLevel;


    [Header("Ability 4 Damage")]
    // For UI
    public Image targetCircleDmg;
    public Image indicatorRangeCircleDmg;
    public Canvas ability2CanvasDmg;
    public float maxAbility2RangeDmg;
    bool damageUiShown = false;

    // For projectile
    public GameObject damageObj;

    // For Cooldown
    public static bool dmgCooldownBool;
    public float dmgCooldown;
    public static int dmgLevel;

    public void OnEnable()
    {
        // Sets all UI to false
        skillshot.GetComponent<Image>().enabled = false;
        targetCircle.GetComponent<Image>().enabled = false;
        indicatorRangeCircle.GetComponent<Image>().enabled = false;
        targetCircleDmg.GetComponent<Image>().enabled = false;
        indicatorRangeCircleDmg.GetComponent<Image>().enabled = false;
        targetCircleCoffee.GetComponent<Image>().enabled = false;
        indicatorRangeCircleCoffee.GetComponent<Image>().enabled = false;

        // Reference
        uiController = FindObjectOfType<UIcontroller>();

        selection.rectTransform.position = ability1.transform.position;
    }

    public void Update()
    {
        if (!playerCS.isMage)
            return;
        else if (!UIcontroller.isPause)
        {
            // Updates skill UI
            UiUpdate();
            SwitchAbility();
        }
    }
    public void UiUpdate()
    {
        RaycastHit hit;

        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Ability 1 Input
        if (Physics.Raycast(r, out hit, Mathf.Infinity, whatIsGround))
        {
            position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }

        // Ability 2 && Ability 4 Input
        if (Physics.Raycast(r, out hit, Mathf.Infinity, whatIsGround))
        {
            if (hit.collider.gameObject != this.gameObject)
            {
                position = new Vector3(hit.point.x, 10f, hit.point.z);
                position = hit.point;
            }
        }

        #region UI stuff
        // Ability 1 Canvas Input
        Quaternion transRot = Quaternion.LookRotation(position - player.transform.position);
        transRot.eulerAngles = new Vector3(0f, transRot.eulerAngles.y, transRot.eulerAngles.z);
        ability1Canvas.transform.rotation = Quaternion.Lerp(transRot, ability1Canvas.transform.rotation, 0f);

        // Ability 2 Canvas Input
        var hitPosDir = (hit.point - transform.position).normalized;
        float distance = Vector3.Distance(hit.point, transform.position);
        distance = Mathf.Min(distance, maxAbility2Range);
        range = distance;

        var newHitPos = transform.position + hitPosDir * distance;
        ability2Canvas.transform.position = (newHitPos);
        targetCircle.transform.localScale = new Vector3(maxAbility2Range / 5f, maxAbility2Range / 5f, maxAbility2Range / 5f);
        
        // Ability 3 Canvas Input
        var hitPosDirCoffee = (hit.point - transform.position).normalized;
        float distanceCoffee = Vector3.Distance(hit.point, transform.position);
        distanceCoffee = Mathf.Min(distanceCoffee, maxAbility2RangeCoffee);

        var newHitPosCoffee = transform.position + hitPosDirCoffee * distanceCoffee;
        ability2CanvasCoffee.transform.position = (newHitPosCoffee);
        targetCircleCoffee.transform.localScale = new Vector3(maxAbility2RangeCoffee / 5f, maxAbility2RangeCoffee / 5f, maxAbility2RangeCoffee / 5f);

        // Ability 4 Canvas Input
        var hitPosDirDmg = (hit.point - transform.position).normalized;
        float distanceDmg = Vector3.Distance(hit.point, transform.position);
        distanceDmg = Mathf.Min(distanceDmg, maxAbility2RangeDmg);

        var newHitPosDmg = transform.position + hitPosDirDmg * distanceDmg;
        ability2CanvasDmg.transform.position = (newHitPosDmg);
        targetCircleDmg.transform.localScale = new Vector3(maxAbility2RangeDmg / 2.5f, maxAbility2RangeDmg / 2.5f, maxAbility2RangeDmg / 2.5f);
        #endregion
    }

    void SwitchAbility()
    {
        // Ability 1
        if (Input.GetKeyDown(KeyCode.Alpha1) && !freezeCooldownBool)
        {
            selection.rectTransform.position = ability1.transform.position;

            if (!freezeUiShown)
            {
                ability = Abilities.Freeze;
                ShowAbility();
            }
            else if (freezeUiShown)
            {
                EndAbility();
            }
        }
        if (skillshot.GetComponent<Image>().enabled == true && Input.GetMouseButton(0) && freezeUiShown)
        {
            UseAbility();
        }

        // Ability 2
        if (Input.GetKeyDown(KeyCode.Alpha2) && !slowCooldownBool)
        {
            selection.rectTransform.position = ability2.transform.position;
            //EndAbility();

            if (!slowUiShown)
            {
                ability = Abilities.Slow;
                ShowAbility();
            }
            else if (slowUiShown)
            {
                EndAbility();
            }
        }
        if (indicatorRangeCircle.GetComponent<Image>().enabled == true && Input.GetMouseButton(0) && slowUiShown)
        {
            UseAbility();
        }

        // Ability 3
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !atkspeedCooldownBool)
        {
            selection.rectTransform.position = ability3.transform.position;

            if (!CoffeeUiShown)
            {
                ability = Abilities.AtkSpeed;
                ShowAbility();
            }
            else if (CoffeeUiShown)
            {
                EndAbility();
            }

        }
        if (indicatorRangeCircleCoffee.GetComponent<Image>().enabled == true && Input.GetMouseButton(0) && CoffeeUiShown)
        {
            UseAbility();
        } 


        // Ability 4
        else if (Input.GetKeyDown(KeyCode.Alpha4) && !dmgCooldownBool)
        {
            selection.rectTransform.position = ability4.transform.position;

            if (!damageUiShown)
            {
                ability = Abilities.Damage;
                ShowAbility();
            }
            else if (damageUiShown)
            {
                EndAbility();
            }
        }
        if (indicatorRangeCircleDmg.GetComponent<Image>().enabled == true && Input.GetMouseButton(0) && damageUiShown)
        {
            UseAbility();
        }
    }

    // ------------------------- Use ability -------------------------
    #region Use Ability
    public void UseAbility()
    {
        switch (ability)
        {
            case Abilities.Freeze:
                if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null)
                {
                    Debug.Log("Over");
                    return;
                }
                StartCoroutine(FreezeAbility());
                break;
            case Abilities.Slow:
                if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null)
                {
                    Debug.Log("Over");
                    return;
                }
                StartCoroutine(SlowAbility());
                break;
            case Abilities.AtkSpeed:
                if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null)
                {
                    Debug.Log("Over");
                    return;
                }
                StartCoroutine(AtkSpeedAbility());
                break;
            case Abilities.Damage:
                if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null)
                {
                    Debug.Log("Over");
                    return;
                }
                StartCoroutine(DamageAbility());
                break;
        }
    }
    public IEnumerator FreezeAbility()
    {
        // Click input
        skillshot.GetComponent<Image>().enabled = false;
        Instantiate(Freeze, ability1Canvas.transform.position, ability1Canvas.transform.rotation);
        Mage.freezeCooldownBool = true;
        freezeUiShown = false;

        // Cooldown
        float i = 0;
        while (i < freezeCooldown)
        {
            i += Time.deltaTime;
            freezeCooldownUI.fillAmount = 1 - i / freezeCooldown;
            yield return null;
        }
        Mage.freezeCooldownBool = false;
    }
    public IEnumerator SlowAbility()
    {
        Instantiate(projectileM, ability2Canvas.transform.position, transform.rotation);
        indicatorRangeCircle.GetComponent<Image>().enabled = false;
        targetCircle.GetComponent<Image>().enabled = false;
        Mage.slowCooldownBool = true;
        slowUiShown = false;

        float i = 0;
        while (i < slowCooldown)
        {
            i += Time.deltaTime;
            slowCooldownUI.fillAmount = 1 - i / slowCooldown;
            yield return null;
        }
        Mage.slowCooldownBool = false;
    }
    public IEnumerator AtkSpeedAbility()
    {
        Instantiate(coffeeRadius, ability2CanvasCoffee.transform.position, ability2CanvasCoffee.transform.rotation);
        indicatorRangeCircleCoffee.GetComponent<Image>().enabled = false;
        targetCircleCoffee.GetComponent<Image>().enabled = false;
        Mage.atkspeedCooldownBool = true;
        CoffeeUiShown = false;

        float i = 0;
        while (i < atkCooldown)
        {
            i += Time.deltaTime;
            boostCooldownUI.fillAmount = 1 - i / atkCooldown;
            yield return null;
        }
        Mage.atkspeedCooldownBool = false;
    }
    public IEnumerator DamageAbility()
    {
        Mage.dmgCooldownBool = true;
        Instantiate(damageObj, ability2CanvasDmg.transform.position, transform.rotation);
        indicatorRangeCircleDmg.GetComponent<Image>().enabled = false;
        targetCircleDmg.GetComponent<Image>().enabled = false;
        damageUiShown = false;

        float i = 0;
        while (i < dmgCooldown)
        {
            i += Time.deltaTime;
            dmageCooldownUI.fillAmount = 1 - i / dmgCooldown;
            yield return null;
        }
        Mage.dmgCooldownBool = false;

    }
    #endregion 

    // ------------------------- Show ability -------------------------
    #region Show Ability
    void ShowAbility()
    {
        switch (ability)
        {
            case Abilities.Freeze:
                FreezeUI();
                    break;
            case Abilities.Slow:
                SlowUI();
                break;
            case Abilities.AtkSpeed:
                CoffeeUI();
                break;
            case Abilities.Damage:
                DamageUI();
                break;
        }
    }

    public void FreezeUI()
    {
        selection.rectTransform.position = ability1.transform.position;
        ability = Abilities.Freeze;
        skillshot.GetComponent<Image>().enabled = true;
        freezeUiShown = true;

        // Disable other UI
        targetCircle.GetComponent<Image>().enabled = false;
        indicatorRangeCircle.GetComponent<Image>().enabled = false;
        targetCircleDmg.GetComponent<Image>().enabled = false;
        indicatorRangeCircleDmg.GetComponent<Image>().enabled = false;
        targetCircleCoffee.GetComponent<Image>().enabled = false;
        indicatorRangeCircleCoffee.GetComponent<Image>().enabled = false;

        slowUiShown = false;
        CoffeeUiShown = false;
        damageUiShown = false;
    }

    public void SlowUI()
    {
        selection.rectTransform.position = ability2.transform.position;
        ability = Abilities.Slow;
        
        targetCircle.GetComponent<Image>().enabled = true;
        indicatorRangeCircle.GetComponent<Image>().enabled = true;
        slowUiShown = true;

        // Disable other UI
        skillshot.GetComponent<Image>().enabled = false;
        
        targetCircleDmg.GetComponent<Image>().enabled = false;
        indicatorRangeCircleDmg.GetComponent<Image>().enabled = false;

        targetCircleCoffee.GetComponent<Image>().enabled = false;
        indicatorRangeCircleCoffee.GetComponent<Image>().enabled = false;
        freezeUiShown = false;
        damageUiShown = false;
        CoffeeUiShown = false;
    }
    public void CoffeeUI()
    {
        selection.rectTransform.position = ability3.transform.position;
        ability = Abilities.AtkSpeed;
        targetCircleCoffee.GetComponent<Image>().enabled = true;
        indicatorRangeCircleCoffee.GetComponent<Image>().enabled = true;
        CoffeeUiShown = true;

        // Disable other UI
        skillshot.GetComponent<Image>().enabled = false;
        
        targetCircleDmg.GetComponent<Image>().enabled = false;
        indicatorRangeCircleDmg.GetComponent<Image>().enabled = false;
        targetCircle.GetComponent<Image>().enabled = false;
        
        indicatorRangeCircle.GetComponent<Image>().enabled = false;
        
        freezeUiShown = false;
        slowUiShown = false;
        damageUiShown = false;
    }
    public void DamageUI()
    {
        selection.rectTransform.position = ability4.transform.position;
        ability = Abilities.Damage;
        targetCircleDmg.GetComponent<Image>().enabled = true;
        indicatorRangeCircleDmg.GetComponent<Image>().enabled = true;
        damageUiShown = true;

        // Disable other UI
        skillshot.GetComponent<Image>().enabled = false;
        targetCircle.GetComponent<Image>().enabled = false;
        indicatorRangeCircle.GetComponent<Image>().enabled = false;
        targetCircleCoffee.GetComponent<Image>().enabled = false;
        indicatorRangeCircleCoffee.GetComponent<Image>().enabled = false;
        freezeUiShown = false;
        slowUiShown = false;
        CoffeeUiShown = false;
    }
    #endregion

    // ------------------------- Stops ability -------------------------
    #region EndAbility
    void EndAbility()
    {
        switch (ability)
        {
            case Abilities.Freeze:
                EndFreeze();
                break;
            case Abilities.Slow:
                EndSlow();
                break;
            case Abilities.AtkSpeed:
                EndCoffee();
                break;
            case Abilities.Damage:
                EndDamage();
                break;
        }
    }
    
    void EndFreeze()
    {
        skillshot.GetComponent<Image>().enabled = false;
        freezeUiShown = false;
    }
    void EndSlow()
    {
        targetCircle.GetComponent<Image>().enabled = false;
        indicatorRangeCircle.GetComponent<Image>().enabled = false;
        slowUiShown = false;
    }
    void EndCoffee()
    {
        targetCircleCoffee.GetComponent<Image>().enabled = false;
        indicatorRangeCircleCoffee.GetComponent<Image>().enabled = false;
        CoffeeUiShown = false;
    }
    void EndDamage()
    {
        targetCircleDmg.GetComponent<Image>().enabled = false;
        indicatorRangeCircleDmg.GetComponent<Image>().enabled = false;
        damageUiShown = false;
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(rangeIndicator.transform.position ,maxAbility2Range);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rangeIndicator.transform.position ,maxAbility2RangeDmg);
    }

}
