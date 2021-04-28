using UnityEngine;

public abstract class AliveGameUnit : MonoBehaviour
{
    public int healthPoints;
    public int maxCurrentHealthPoints;// For correct percentage recalculation
    public int shieldPoints;            
    public int maxCurrentShieldPoints;// For correct percentage recalculation
    public int deffencePoints;
    public bool isShieldOn                 = false;
    public int shieldGeneratorInfluencers  = 0;
    public float uiCanvasDissapearingTimer = 0f;


    public void InitGameUnitFromFile(int healthPoints, int maxCurrentHealthPoints, int shieldPoints, int maxCurrentShieldPoints, int deffencePoints, bool isShieldOn, int shieldGeneratorInfluencers)
    {
        this.healthPoints = healthPoints;
        this.maxCurrentHealthPoints = maxCurrentHealthPoints;  // For correct percentage recalculation

        this.shieldPoints = shieldPoints;            
        this.maxCurrentShieldPoints = maxCurrentShieldPoints;  // For correct percentage recalculation

        this.deffencePoints = deffencePoints;

        this.isShieldOn = isShieldOn;
        this.shieldGeneratorInfluencers = shieldGeneratorInfluencers;

    }

    public void CreateGameUnit(int _maxHealth, int _maxShield, int _maxDefensePoints)
    {
        healthPoints = _maxHealth;
        maxCurrentHealthPoints = _maxHealth;

        shieldPoints = _maxShield;
        maxCurrentShieldPoints = _maxShield;

        deffencePoints = _maxDefensePoints;

        isShieldOn = false;
        shieldGeneratorInfluencers = 0;
    }

    public void UpgradeStats(int newHealth, int newShield, int newDefense)
    {
        healthPoints = (newHealth * healthPoints) / maxCurrentHealthPoints;
        maxCurrentHealthPoints = newHealth;

        shieldPoints = (newHealth * shieldPoints) / maxCurrentShieldPoints;
        maxCurrentShieldPoints = newShield;

        deffencePoints = newDefense;
    }

    public void TurnShieldOn()
    {
        isShieldOn = true;
        deffencePoints += 5;
    }

    public void TurnShieldOff()
    {
        isShieldOn = false;
        deffencePoints -= 5;
    }

    public virtual void TakeDamage(int damagePoints)
    {
        if (shieldPoints != 0)
        {
            if (shieldPoints >= damagePoints)
            {
                shieldPoints -= (damagePoints);
            }

            else
            {
                damagePoints -= shieldPoints;
                shieldPoints = 0;
                healthPoints -= damagePoints;
            }
        }

        else
        {
            if (healthPoints > damagePoints)
            {
                healthPoints -= (damagePoints);
            }
            else
            {
                healthPoints = 0;
            }
        }
    }
}


public interface IBuilding
{
    void Invoke();

    void ConstructBuilding(Model model);

    void DestroyBuilding();
}