using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class bossManager : MonoBehaviour
{
    public List<GameObject> Enemies = new List<GameObject>(); // Список врагов.
    public Animator BossAnimator;
    public static bossManager BossManagerCls;
    private float attackMode;
    public bool LockOnTarget, BossIsAlive;
    private Transform target; // Цель для атаки.
    public Slider HealthBar;
    public TextMeshProUGUI Health_bar_amount;
    public int Health;
    public GameObject Particle_Death;
    public float maxDistance, minDistance;
    public GameObject victoryMenu;
    public GameObject loseMenu;

    void Start()
    {
        BossManagerCls = this; // Инициализация статической переменной.

        var enemy = GameObject.FindGameObjectsWithTag("add");

        foreach (var stickMan in enemy)
            Enemies.Add(stickMan);

        BossAnimator = GetComponent<Animator>();

        BossIsAlive = true;

        HealthBar.value = HealthBar.maxValue = Health = 150;

        Health_bar_amount.text = Health.ToString();
    }

    void Update()
    {

        HealthBar.transform.rotation = Quaternion.Euler(HealthBar.transform.rotation.x, 0f, HealthBar.transform.rotation.y);

        if (Enemies.Count > 0)
            foreach (var stickMan in Enemies) // Проверка дистанции до врагов.
            {
                var stickManDistance = stickMan.transform.position - transform.position;

                if (stickManDistance.sqrMagnitude < maxDistance * maxDistance && !LockOnTarget)
                {
                    target = stickMan.transform;
                    BossAnimator.SetBool("fight", true);

                    transform.position = Vector3.MoveTowards(transform.position, target.position, 1f * Time.deltaTime);
                }

                if (stickManDistance.sqrMagnitude < minDistance * minDistance)
                    LockOnTarget = true; // Установка флага "Цель на прицеле".

            }

        if (LockOnTarget)
        {   
            var bossRotation = new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position; // Поворот босса к цели и удаление врагов без компонента memeberManager.

            transform.rotation = Quaternion.Slerp(transform.rotation, quaternion.LookRotation(bossRotation, Vector3.up), 10f * Time.deltaTime);

            for (int i = 0; i < Enemies.Count; i++)
                if (!Enemies.ElementAt(i).GetComponent<memeberManager>().member)
                    Enemies.RemoveAt(i);

        }

        if (Enemies.Count == 0)
        {
            BossAnimator.SetBool("fight", false);
            BossAnimator.SetFloat("attackmode", 4f);
            loseMenu.SetActive(true);
        }

        if (Health <= 0 && BossIsAlive)
        {
            gameObject.SetActive(false);
            BossIsAlive = false;
            Instantiate(Particle_Death, transform.position, Quaternion.identity);
            victoryMenu.SetActive(true);
        }
    }

    public void ChangeTheBossAttackMode() // Метод для изменения режима атаки босса.
    {
        BossAnimator.SetFloat("attackmode", Random.Range(2, 4));
    }
}
