using UnityEngine;

/// <summary>
/// hp : entity의 체력
/// speed : entity의 이동 속도
/// attack_damage : 공격의 데미지
/// </summary>

[CreateAssetMenu(menuName = "pValue")]
public class BaseEntity : ScriptableObject
{
    [Header("Stats")]
    public int hp;
    public int speed;
    public int attack_damage;
    [Range(0.5f, 4.5f)] public float attack_range;
}