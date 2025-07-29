    using UnityEngine;

public class BulletMechanism : MonoBehaviour

{
    public float lifeTime = 3f; // 발사체가 사라지는 시간 (초)
    private int _damageAmount;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    
    public void SetDamage(int damage)
    {
        _damageAmount = damage;
    }

    // 다른 오브젝트와 충돌했을 때 (Rigidbody와 Collider가 모두 Is Trigger가 아닐 때 호출)
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(gameObject.name + "이(가) " + collision.gameObject.name + "과 충돌했습니다!");
        // TODO: 여기에 적에게 데미지를 주거나 특정 효과를 발생시키는 로직을 추가할 수 있습니다.
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 적의 EnemyHealth 스크립트를 가져옵니다.
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null) // 스크립트가 있다면
            {
                enemyHealth.TakeDamage(_damageAmount); // 데미지 적용
            }
        }

        // 충돌 후 발사체 파괴 (선택 사항)
        Destroy(gameObject); 
    }

    // 트리거 충돌했을 때 (둘 중 하나라도 Is Trigger가 체크되어 있을 때 호출)
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.name + "이(가) " + other.gameObject.name + "과 트리거 충돌했습니다!");

        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // 설정된 _damageAmount 값을 사용하여 데미지 적용
                enemyHealth.TakeDamage(_damageAmount);
            }
        }
        Destroy(gameObject);
    }
    
    
}
