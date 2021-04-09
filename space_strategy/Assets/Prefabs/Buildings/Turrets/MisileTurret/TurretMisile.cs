public class TurretMisile : Turette
{
    public bool isFired = false;
    public float coolDownTimer = 1f;

    public override void ResetCombatMode()
    {
        isFacingEnemy = false;
    }

    public override void DestroyBuilding()
    {
        ResourceManager.Instance.misileTurretsList.Remove(this);

        base.DestroyBuilding();
    }
}


    // public void InitStatsAfterBaseUpgrade()
    // {
    //     switch (turretData.level)
    //     {
    //         case 1:
    //         gameUnit.UpgradeStats(
    //         StatsManager._maxHealth_Lvl1_MisileTurret + StatsManager._baseUpgradeStep_MisileTurret, 
    //         StatsManager._maxShiled_Lvl1_MisileTurret + StatsManager._baseUpgradeStep_MisileTurret,
    //         StatsManager._defensePoints_Lvl1_MisileTurret);
    //         break;

    //         case 2:
    //         gameUnit.UpgradeStats(
    //         StatsManager._maxHealth_Lvl2_MisileTurret + StatsManager._baseUpgradeStep_MisileTurret, 
    //         StatsManager._maxShiled_Lvl2_MisileTurret + StatsManager._baseUpgradeStep_MisileTurret,
    //         StatsManager._defensePoints_Lvl2_MisileTurret);
    //         break;

    //         case 3:
    //         gameUnit.UpgradeStats(
    //         StatsManager._maxHealth_Lvl3_MisileTurret + StatsManager._baseUpgradeStep_MisileTurret, 
    //         StatsManager._maxShiled_Lvl3_MisileTurret + StatsManager._baseUpgradeStep_MisileTurret,
    //         StatsManager._defensePoints_Lvl3_MisileTurret);
    //         break;
    //     }

        
        
    //     // reload everything here
    //     // if (isMenuOpened)
    //     // {
    //     //     turretMenuReference.ReloadSlidersHP_SP();
    //     // }

    //     // Reloads HP_SP sliders if buildings manage menu opened
    //     // GameViewMenu.Instance.ReloadMisileTurretHPSP(this);

    // }

    // public void InitStaticsLevel_2()
    // {
    //     turretData.UpgradeToLvl2(); 

    //     gameUnit.UpgradeStats(StatsManager._maxHealth_Lvl2_MisileTurret, StatsManager._maxShiled_Lvl2_MisileTurret, StatsManager._defensePoints_Lvl2_MisileTurret);


    //     // Reload Sliders
    //     // If mineshaft menu was opened
    //     // If UI small panel above building was active
    //     // If buildings manage menu was opened
    //     // OnDamageTaken(gameUnit);
    //     // if (isMenuOpened)
    //     // {
    //     //     turretMenuReference.ReloadSlidersHP_SP();
    //     // }
    //     // Reloads HP_SP sliders if buildings manage menu opened
    //     // GameViewMenu.Instance.ReloadMisileTurretHPSP(this);
    // }

    // public void InitStaticsLevel_3()
    // {
    //     turretData.UpgradeToLvl3(); 

    //     gameUnit.UpgradeStats(StatsManager._maxHealth_Lvl3_MisileTurret, StatsManager._maxShiled_Lvl3_MisileTurret, StatsManager._defensePoints_Lvl3_MisileTurret);

    //     // Reload Sliders
    //     // If mineshaft menu was opened
    //     // If UI small panel above building was active
    //     // If buildings manage menu was opened
    //     // if (isMenuOpened)
    //     // {
    //     //     turretMenuReference.ReloadSlidersHP_SP();
    //     // }
    //     // Reloads HP_SP sliders if buildings manage menu opened
    //     // GameViewMenu.Instance.ReloadMisileTurretHPSP(this);
    // }
