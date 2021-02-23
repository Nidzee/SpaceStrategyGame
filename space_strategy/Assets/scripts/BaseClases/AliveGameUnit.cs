﻿using UnityEngine;

public abstract class AliveGameUnit : MonoBehaviour
{
    public int healthPoints;
    public int maxCurrentHealthPoints;

    public int shieldPoints;
    public int maxCurrentShieldPoints;

    public int deffencePoints;









    public bool isShieldOn = false;
    public int shieldGeneratorInfluencers = 0;

    public virtual void TurnShieldOn()
    {
        isShieldOn = true;
        deffencePoints += 5;
    }

    public virtual void TurnShieldOff()
    {
        isShieldOn = false;
        deffencePoints -= 5;
    }

    public virtual void TakeDamage(int damagePoints)
    {
        healthPoints -= damagePoints;
        // if (shieldPoints != 0)
        // {
        //     if (shieldPoints >= damagePoints)
        //     {
        //         shieldPoints -= (damagePoints - deffencePoints);
        //     }

        //     else
        //     {
        //         damagePoints -= shieldPoints;
        //         shieldPoints = 0;
        //         healthPoints -= damagePoints;
        //     }
        // }

        // else
        // {
        //     if (healthPoints > damagePoints)
        //     {
        //         healthPoints -= (damagePoints - deffencePoints);
        //     }
        //     else
        //     {
        //         healthPoints = 0;
        //     }
        // }
    }

    public virtual void AddHP(int healthPoints)
    {
        this.healthPoints += healthPoints;
    }
    
    public virtual void AddSP(int shieldPoints)
    {
        this.shieldPoints += shieldPoints;
    }

}












public interface IBuilding
{
    void Invoke(); // Executes building's menu panel
}
