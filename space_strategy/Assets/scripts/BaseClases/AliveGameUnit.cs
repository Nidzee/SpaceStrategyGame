using UnityEngine;

public abstract class AliveGameUnit : MonoBehaviour
{
    public float HealthPoints;
    public float ShieldPoints;
    public bool isShieldOn = false;

    public virtual void SetShieldOn()
    {
        this.isShieldOn = true;
    }

    public virtual void SetShieldOff()
    {
        this.isShieldOn = false;
    }

    public virtual void TakeDamage(float DamagePoints)
    {
        // Damage Logic
    }
    public virtual void AddHP(float HealthPoints)
    {
        this.HealthPoints += HealthPoints;
    }
    public virtual void AddSP(float ShieldPoints)
    {
        this.ShieldPoints += ShieldPoints;
    }
}

public abstract class BuildingClass : AliveGameUnit
{
    public virtual void ActivateWithTouch()
    {
        
    }
}

public interface IBuilding
{
    void Invoke(); // Executes building's menu panel
}
