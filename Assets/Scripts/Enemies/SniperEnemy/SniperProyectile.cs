using System.Collections;
using UnityEngine;

public class SniperProyectile : MonoBehaviour
{
    SniperEnemy sniperEnemy;
    Vector3 playerPosWhenShooted;
    float proyectileSpeed = 15f;
    bool desapearing = false;
    void Start()
    {
        sniperEnemy = transform.parent.GetComponent<SniperEnemy>();
        transform.parent = null;
        playerPosWhenShooted = Player.Instance.transform.position;
    }
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerPosWhenShooted, proyectileSpeed * Time.deltaTime);
        if (desapearing) return;
        StartCoroutine(CooldownForDisapearing());
    }
    IEnumerator CooldownForDisapearing()
    {
        desapearing = true;
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            sniperEnemy.GetGameNoticer().Call(new OnMessageSentBasicBuild
            {
                typeOfMessage = TypeOfMessage.PlayerHitted,
                gameObject = Player.Instance.gameObject,
                damageToDealPlayer = sniperEnemy.GetSniperDamage() * Player.Instance.GetPlayerShield()
            });
        }
        else if (other.CompareTag("PlayerDamageCube"))
        {
            Destroy(gameObject);
        }
    }
}
