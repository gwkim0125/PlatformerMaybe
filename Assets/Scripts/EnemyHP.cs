using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 28; // 최대 체력
    private int currentHealth;  // 현재 체력

    void Start()
    {
        currentHealth = maxHealth; // 시작 시 체력 초기화
        Debug.Log(gameObject.name + "의 체력: " + currentHealth);
    }

    // 데미지를 받는 함수
    public void TakeDamage(int amount)
    {
        currentHealth -= amount; // 체력 감소
        Debug.Log(gameObject.name + "이(가) " + amount + "의 데미지를 받았습니다. 남은 체력: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die(); // 체력이 0 이하면 사망 처리
        }
    }

    // 사망 처리 함수
    void Die()
    {
        Debug.Log(gameObject.name + "이(가) 파괴되었습니다.");
        // TODO: 사망 애니메이션, 파티클 효과, 드롭 아이템 등 추가
        Destroy(gameObject); // 적 오브젝트 파괴
    }
}
