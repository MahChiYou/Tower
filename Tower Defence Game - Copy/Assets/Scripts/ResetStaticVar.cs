using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticVar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ProjectileUpgradeManager.ballLevel = 0;
        ProjectileUpgradeManager.breadLevel = 0;
        ProjectileUpgradeManager.macLevel = 0;
        ProjectileUpgradeManager.macaroniAmt = 0;
        ProjectileUpgradeManager.penneLevel = 0;
        Mage.atkSpeedLevel = 0;
        Mage.dmgLevel = 0;
        Mage.freezeLevel = 0;
        Mage.slowLevel = 0;
        Mage.atkspeedCooldownBool = false;
        Mage.dmgCooldownBool = false;
        Mage.freezeCooldownBool = false;
        Mage.slowCooldownBool = false;
        Spawner.prepPhase = true;
        Spawner.waveLock = false;
        UIcontroller.isPause = false;
        TutorialManager.tutorialSwitch = false;
        TutorialManager.tutorialWaveStart = false;
    }
}
