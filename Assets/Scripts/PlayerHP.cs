using UnityEngine;
using System.Collections; // 코루틴(Coroutine)을 사용하기 위해 필요합니다.
using UnityEngine.UI; // UI 요소를 사용하기 위해 필요합니다. (선택 사항: 체력 바)

public class PlayerHealth : MonoBehaviour
{
    [Header("플레이어 체력 설정")]
    public int maxHealth = 28; // 최대 체력
    private int currentHealth;  // 현재 체력

    [Header("무적 시간 설정")]
    public float invincibilityDuration = 1.0f; // 무적 시간 (초)
    private bool isInvincible = false; // 현재 무적 상태인지 여부

    [Header("보호막 이펙트 설정")]
    public GameObject shieldEffectPrefab; // 보호막 이펙트 게임 오브젝트 (프리팹 또는 씬 내 오브젝트)
    private GameObject activeShieldEffect; // 현재 활성화된 보호막 이펙트 인스턴스

    [Header("UI 연동 (선택 사항)")]
    public Slider healthBarSlider; // 체력 바 슬라이더 UI (할당 시 연동)
    public Text healthText; // 체력 텍스트 UI (할당 시 연동)

    void Start()
    {
        currentHealth = maxHealth; // 시작 시 체력 초기화
        UpdateHealthUI(); // UI 업데이트

        // 보호막 이펙트 프리팹이 할당되었는지 확인 및 인스턴스 생성
        if (shieldEffectPrefab == null)
        {
            Debug.LogWarning(gameObject.name + ": Shield Effect Prefab이 할당되지 않았습니다. 보호막 시각 효과가 나타나지 않습니다.");
        }
        else
        {
            // 보호막 이펙트를 플레이어의 자식으로 생성하여 플레이어와 함께 이동하게 합니다.
            activeShieldEffect = Instantiate(shieldEffectPrefab, transform.position, transform.rotation, transform);
            activeShieldEffect.SetActive(false); // 초기에는 비활성화
        }
    }

    // 데미지를 받는 함수
    public void TakeDamage(int amount)
    {
        if (isInvincible) // 무적 상태일 때는 데미지를 받지 않습니다.
        {
            Debug.Log(gameObject.name + "은(는) 무적 상태입니다. 데미지 무시!");
            return;
        }

        currentHealth -= amount; // 체력 감소
        currentHealth = Mathf.Max(currentHealth, 0); // 체력이 0 미만으로 내려가지 않도록 함
        Debug.Log(gameObject.name + "이(가) " + amount + "의 데미지를 받았습니다. 남은 체력: " + currentHealth);
        UpdateHealthUI(); // UI 업데이트

        if (currentHealth <= 0)
        {
            Die(); // 체력이 0 이하면 사망 처리
        }
        else // 데미지를 받았고 죽지 않았다면 무적 상태를 시작합니다.
        {
            StartInvincibility();
        }
    }

    // 플레이어 사망 처리 함수
    void Die()
    {
        Debug.Log(gameObject.name + "이(가) 사망했습니다!");
        // TODO:
        // 1. 사망 애니메이션 재생
        // 2. 게임 오버 UI 표시
        // 3. 게임 재시작 또는 메인 메뉴로 돌아가기
        // 4. 플레이어 오브젝트 비활성화 또는 파괴

        // 예시: 플레이어 오브젝트 비활성화 (씬에서 제거하지 않고 숨김)
        gameObject.SetActive(false); 

        // 간단한 게임 오버 메시지 출력 (UI 구현 시 이 부분 대체)
        Debug.Log("게임 오버!");
        // Time.timeScale = 0; // 게임 일시 정지 (선택 사항)
    }

    // 무적 상태를 시작하는 함수
    void StartInvincibility()
    {
        isInvincible = true; // 무적 상태 활성화

        // 보호막 이펙트 활성화
        if (activeShieldEffect != null)
        {
            activeShieldEffect.SetActive(true);
        }

        // 무적 시간 코루틴을 시작합니다.
        StartCoroutine(InvincibilityCoroutine());
    }

    // 무적 시간 동안의 로직 (보호막 이펙트 활성화 유지)
    IEnumerator InvincibilityCoroutine()
    {
        // 무적 시간 동안 대기
        yield return new WaitForSeconds(invincibilityDuration);

        EndInvincibility(); // 무적 상태를 종료합니다.
    }

    // 무적 상태를 종료하는 함수
    void EndInvincibility()
    {
        isInvincible = false; // 무적 상태 비활성화
        Debug.Log(gameObject.name + "의 무적 시간이 종료되었습니다.");

        // 보호막 이펙트 비활성화
        if (activeShieldEffect != null)
        {
            activeShieldEffect.SetActive(false);
        }
    }

    // 체력 UI를 업데이트하는 함수 (선택 사항)
    void UpdateHealthUI()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth + " / " + maxHealth;
        }
    }
}
