using UnityEngine;

public abstract class AliveGameUnit : MonoBehaviour
{
    public float HealthPoints = 100;
    public float ShieldPoints = 100;
    
    public bool isShieldOn = false;

    public int shieldGeneratorInfluencers = 0;

    public virtual void TurnShieldOn()
    {
        isShieldOn = true;


    }

    public virtual void TurnShieldOff()
    {
        isShieldOn = false;

        
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

public interface IBuilding
{
    void Invoke(); // Executes building's menu panel
}
