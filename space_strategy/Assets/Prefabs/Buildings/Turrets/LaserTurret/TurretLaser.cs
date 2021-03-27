public class TurretLaser : Turette
{
    public LTData laserTurretData;

    // public static void GetResourcesNeedToExpand(out int crystalNeed, out int ironNeed, out int gelNeed, TurretLaser lt)
    // {
    //     if (lt.turretData.level == 1)
    //     {
    //         crystalNeed = StatsManager._crystalNeedForExpand_ToLvl2_LaserTurret;
    //         ironNeed = StatsManager._ironNeedForForExpand_ToLvl2_LaserTurret;
    //         gelNeed = StatsManager._gelNeedForForExpand_ToLvl2_LaserTurret;
    //     }
    //     else
    //     {
    //         crystalNeed = StatsManager._crystalNeedForExpand_ToLvl3_LaserTurret;
    //         ironNeed = StatsManager._ironNeedForForExpand_ToLvl3_LaserTurret;
    //         gelNeed = StatsManager._gelNeedForForExpand_ToLvl3_LaserTurret;
    //     }
    // }

    // public void InitStatsAfterBaseUpgrade()
    // {
    //     switch (turretData.level)
    //     {
    //         case 1:
    //         gameUnit.UpgradeStats(StatsManager._maxHealth_Lvl1_LaserTurret + StatsManager._baseUpgradeStep_LaserTurret, 
    //         StatsManager._maxShiled_Lvl1_LaserTurret + StatsManager._baseUpgradeStep_LaserTurret,
    //         StatsManager._defensePoints_Lvl1_LaserTurret);
    //         break;

    //         case 2:
    //         gameUnit.UpgradeStats(StatsManager._maxHealth_Lvl2_LaserTurret + StatsManager._baseUpgradeStep_LaserTurret, 
    //         StatsManager._maxShiled_Lvl2_LaserTurret + StatsManager._baseUpgradeStep_LaserTurret,
    //         StatsManager._defensePoints_Lvl2_LaserTurret);
    //         break;

    //         case 3:
    //         gameUnit.UpgradeStats(StatsManager._maxHealth_Lvl3_LaserTurret + StatsManager._baseUpgradeStep_LaserTurret, 
    //         StatsManager._maxShiled_Lvl3_LaserTurret + StatsManager._baseUpgradeStep_LaserTurret,
    //         StatsManager._defensePoints_Lvl3_LaserTurret);
    //         break;
    //     }
        

    //     // reload everything here
    //     // if (isMenuOpened)
    //     // {
    //     //     turretMenuReference.ReloadSlidersHP_SP();
    //     // }
    //     // Reloads HP_SP sliders if buildings manage menu opened
    //     // GameViewMenu.Instance.ReloadLaserTurretHPSP(this);
    // }

    // public void InitStaticsLevel_1()
    // {
    //     // turretData.InitStaticsLevel1();


    // }

    // public void InitStaticsLevel_2()
    // {
    //     // turretData.UpgradeToLvl2(); 

    //     gameUnit.UpgradeStats(StatsManager._maxHealth_Lvl2_LaserTurret, StatsManager._maxShiled_Lvl2_LaserTurret, StatsManager._defensePoints_Lvl2_LaserTurret);

    //     // Reload Sliders
    //     // If mineshaft menu was opened
    //     // If UI small panel above building was active
    //     // If buildings manage menu was opened

    //     // if (isMenuOpened)
    //     // {
    //     //     turretMenuReference.ReloadSlidersHP_SP();
    //     // }

    //     // Reloads HP_SP sliders if buildings manage menu opened
    //     // GameViewMenu.Instance.ReloadLaserTurretHPSP(this);
    // }

    // public void InitStaticsLevel_3()
    // {
    //     // turretData.UpgradeToLvl3(); 
        
    //     gameUnit.UpgradeStats(StatsManager._maxHealth_Lvl3_LaserTurret, StatsManager._maxShiled_Lvl3_LaserTurret, StatsManager._defensePoints_Lvl3_LaserTurret);


    //     // Reload Sliders
    //     // If mineshaft menu was opened
    //     // If UI small panel above building was active
    //     // If buildings manage menu was opened
    //     // if (isMenuOpened)
    //     // {
    //     //     turretMenuReference.ReloadSlidersHP_SP();
    //     // }
    //     // Reloads HP_SP sliders if buildings manage menu opened
    //     // GameViewMenu.Instance.ReloadLaserTurretHPSP(this);
    // }

    public override void DestroyBuilding()
    {
        base.DestroyBuilding();

        ResourceManager.Instance.laserTurretsList.Remove(this);

        Destroy(gameObject);
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }
}