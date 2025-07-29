using UnityEngine;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    [Header("공격 설정")]
    public GameObject normalProjectilePrefab; // 일반 발사체 프리팹 (인스펙터에서 할당)
    public GameObject chargeProjectilePrefab; // 차지 발사체 프리팹 (인스펙터에서 할당)
    public Transform projectileSpawnPoint;   // 발사 지점 오브젝트 (인스펙터에서 할당)
    public float projectileSpeed = 20f;      // 발사 속도

    [Header("발사체 제한")]
    public int maxProjectiles = 3; // 한 화면에 존재할 수 있는 최대 발사체 수

    [Header("차지샷 설정")]
    public float chargeTimeThreshold = 1.0f; // 차지샷으로 인정될 최소 누르는 시간 (초)
    public float chargeProjectileSpeedMultiplier = 1.5f; // 차지샷의 속도 배율
    
    public int normalShotDamage = 2;   // 일반샷 데미지
    public int chargeShotDamage = 4;   // 차지샷 데미지

    private List<GameObject> activeProjectiles = new List<GameObject>();
    private float mouseButtonDownTime; // 마우스 버튼을 누르기 시작한 시간
    private bool isCharging = false; // 현재 차지 중인지 여부

    void Update()
    {
        // 1. 활성화된 발사체 리스트 정리 (파괴된 발사체 제거)
        for (int i = activeProjectiles.Count - 1; i >= 0; i--)
        {
            if (activeProjectiles[i] == null)
            {
                activeProjectiles.RemoveAt(i);
            }
        }

        // 2. 마우스 왼쪽 버튼 누르기 시작 (차지 시작)
        if (Input.GetMouseButtonDown(0))
        {
            if (activeProjectiles.Count < maxProjectiles) // 발사체 제한을 넘지 않을 때만 차지 시작 가능
            {
                mouseButtonDownTime = Time.time; // 현재 시간 기록
                isCharging = true;
            }
        }

        // 3. 마우스 왼쪽 버튼 떼기 (공격 실행)
        if (Input.GetMouseButtonUp(0) && isCharging) // isCharging 플래그로 중복 발사 방지
        {
            float chargeDuration = Time.time - mouseButtonDownTime; // 누른 시간 계산

            if (chargeDuration >= chargeTimeThreshold)
            {
                // 차지샷 발사
                Attack(chargeProjectilePrefab, projectileSpeed * chargeProjectileSpeedMultiplier,chargeShotDamage);
            }
            else
            {
                // 일반샷 발사
                Attack(normalProjectilePrefab, projectileSpeed,normalShotDamage);
            }
            isCharging = false; // 차지 상태 해제
        }
        
        // (선택 사항) 차지 중 시각적 피드백 (예: 캐릭터 빛나게 하기, UI 차지 게이지 표시 등)
         if (isCharging)
         {
            float currentChargeDuration = Time.time - mouseButtonDownTime;
            Debug.Log("현재 차지 시간: " + currentChargeDuration);
            // 여기에 차지 시간에 따른 시각적 효과 코드를 추가합니다.
         }
    }

    // 발사할 프리팹과 속도를 인자로 받는 공격 함수
    void Attack(GameObject projectileToSpawn, float currentProjectileSpeed, int damage)
    {
        if (projectileToSpawn == null)
        {
            Debug.LogError("발사할 프리팹이 할당되지 않았습니다! 인스펙터에 할당해주세요.");
            return;
        }
        if (projectileSpawnPoint == null)
        {
            Debug.LogError("발사 지점이 할당되지 않았습니다! 인스펙터에 할당해주세요.");
            return;
        }

        // 발사체 인스턴스 생성
        GameObject projectile = Instantiate(projectileToSpawn, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        // 발사체 Rigidbody 가져오기
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        if (projectileRb == null)
        {
            Debug.LogError("발사체 프리팹에 Rigidbody 컴포넌트가 없습니다!");
            // 생성된 projectile을 바로 파괴하지 않으면 문제가 될 수 있으므로 주의
            Destroy(projectile); 
            return;
        }
        
        BulletMechanism projectileComponent = projectile.GetComponent<BulletMechanism>();
        if (projectileComponent != null)
        {
            projectileComponent.SetDamage(damage);
        }
        else
        {
            Debug.LogWarning("발사체 프리팹에 Projectile 스크립트가 없습니다!");
        }
        
      BulletMechanism bulletMechanismComponent = projectile.GetComponent<BulletMechanism>();
        

        // 발사체에 힘을 가하여 발사
        projectileRb.AddForce(projectileSpawnPoint.forward * currentProjectileSpeed, ForceMode.VelocityChange);
        
        // 새로 생성된 발사체를 리스트에 추가
        activeProjectiles.Add(projectile);

        Debug.Log((projectileToSpawn == chargeProjectilePrefab ? "차지샷" : "일반샷") + " 발사! 데미지: " + damage + ", 현재 발사체 수: " + activeProjectiles.Count);
    }
}