using UnityEngine;

public class ShieldRange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == TagConstants.buildingTag) // if Unit intersects our collider
        {
            Debug.Log("Collided Building! - " + collider.name);

            collider.GetComponent<AliveGameUnit>().shieldGeneratorInfluencers++;
            
            if (!collider.GetComponent<AliveGameUnit>().isShieldOn)
            {
                collider.GetComponent<AliveGameUnit>().TurnShieldOn();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == TagConstants.buildingTag) // if Unit intersects our collider
        {
            collider.GetComponent<AliveGameUnit>().shieldGeneratorInfluencers--;
            
            if (collider.GetComponent<AliveGameUnit>().shieldGeneratorInfluencers == 0)
            {
                collider.GetComponent<AliveGameUnit>().TurnShieldOff();
            }
        }
    }
}
