using UnityEngine;

public class BossBattle1 : MonoBehaviour
{
    [SerializeField] Transform theBossItself;
    [SerializeField] Transform camLockPos;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bossShotPrefab;
    [SerializeField] Animator animator;

    [Header("Boss PlayerPrefs Key")]
    [SerializeField] string bossPPKey;

    [Space]
    [SerializeField] Transform[] spawnPoints;

    [Space]
    [SerializeField] float camMoveSpeed;

    [Space]
    [SerializeField] float bossMoveSpeed;
    [SerializeField] float activeTime;
    [SerializeField] float fadeOutTime;
    [SerializeField] float inactiveTime;

    [Header("Time Between Shots")]
    [SerializeField] float timeBetweenShotsPhase1;
    [SerializeField] float timeBetweenShotsPhase2;
    [SerializeField] float timeBetweenShotsPhase3;

    [Header("Health Thresholds For Phases")]
    [SerializeField] int phase2Threshold;
    [SerializeField] int phase3Threshold;

    [Header("Win Objects")]
    [SerializeField] GameObject winObjectsParent;

    CameraController cameraController;
    Transform player;
    Transform currentTargetedPoint;
    Transform previousPosition;

    float activeCounter;
    float fadeOutCounter;
    float inactiveCounter;
    float shotCounter;
    bool battleEnded;

    private void OnEnable()
    {
        player = PlayerHealthController.instance.transform;
        cameraController = FindObjectOfType<CameraController>();
        cameraController.enabled = false;

        // phase 2 will begin when the boss's health is below 75%
        phase2Threshold = Mathf.FloorToInt(BossHealthController.instance.Health * 0.75f);

        // phase 3 will begin when the boss's health is below 75%
        phase3Threshold = Mathf.FloorToInt(BossHealthController.instance.Health * 0.25f);

        shotCounter = timeBetweenShotsPhase1;
        activeCounter = activeTime;

        AudioManager.instance.PlayBossMusic();
    }

    private void Update()
    {
        // moving the camera and locking it into place when the boss battle begins:
        cameraController.transform.position = Vector3.MoveTowards
                                              (current: cameraController.transform.position,
                                              target: camLockPos.position,
                                              maxDistanceDelta: camMoveSpeed * Time.deltaTime);

        // changing the direction that the boss faces, relative to the player:
        {
            if (player.position.x < theBossItself.position.x)
                theBossItself.localScale = Vector2.one;

            else
                theBossItself.localScale = new Vector2(-1, 1);
        }

        if (!battleEnded)
        {
            #region First Phase
            // the first phase of the boss fight...
            // if the boss's health is currently greater than the threshold needed to be crossed for the 2nd phase:
            if (BossHealthController.instance.Health > phase2Threshold)
            {
                if (activeCounter > 0)
                {
                    // the activeCounter starts counting down:
                    activeCounter -= Time.deltaTime;

                    // as soon as the activeCounter reacher 0, make the boss vanish (play anim.)
                    if (activeCounter <= 0)
                    {
                        // the fadeOutCounter's value is set here:
                        fadeOutCounter = fadeOutTime;
                        animator.SetTrigger("vanish");
                    }

                    shotCounter -= Time.deltaTime;

                    // only shoots when active...
                    // shoot at intervals defined by the timeBetweenShotsPhase1 value:
                    if (shotCounter <= 0)
                    {
                        shotCounter = timeBetweenShotsPhase1;
                        Instantiate(bossShotPrefab, firePoint.position, Quaternion.identity);
                    }
                }

                else if (fadeOutCounter > 0)
                {
                    // then the fadeOutCounter starts counting down:
                    fadeOutCounter -= Time.deltaTime;

                    // when it reaches zero, the boss itself is going to be disabled
                    if (fadeOutCounter <= 0)
                    {
                        theBossItself.gameObject.SetActive(false);

                        // the inactiveCounter's value is set here:
                        inactiveCounter = inactiveTime;
                    }
                }

                else if (inactiveCounter > 0)
                {
                    // then the inactiveCounter starts counting down:
                    inactiveCounter -= Time.deltaTime;

                    if (inactiveCounter <= 0)
                    {
                        // the boss picks a random position to spawn at and appears again:
                        theBossItself.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

                        // this bit of code is going to ensure that the boss never spawns
                        // in the same spot twice in first phase:
                        if (previousPosition != null)
                        {
                            int whileBreaker = 0;

                            while (theBossItself.position == previousPosition.position && whileBreaker < 50)
                            {
                                theBossItself.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                                whileBreaker++;
                            }
                        }

                        previousPosition = theBossItself;
                        theBossItself.gameObject.SetActive(true);

                        // the logic loops around and sets the activeCounter back to the activeTime:
                        activeCounter = activeTime;

                        // reset the shot counter to max:
                        shotCounter = timeBetweenShotsPhase1;
                    }
                }
            }
            #endregion

            #region Second Phase
            // the second phase:
            else if (BossHealthController.instance.Health <= phase2Threshold && BossHealthController.instance.Health > phase3Threshold)
            {
                // if the boss has no targeted point, set it to be its own pos.
                // Set the fadeOutCounter to max and cause it to vanish:
                if (currentTargetedPoint == null)
                {
                    currentTargetedPoint = theBossItself;
                    fadeOutCounter = fadeOutTime;
                    animator.SetTrigger("vanish");
                }

                else
                {
                    // if the boss is away from a targetPoint:
                    if (Vector3.Distance(theBossItself.position, currentTargetedPoint.position) > 0.02f)
                    {
                        theBossItself.position = Vector3.MoveTowards(theBossItself.position, currentTargetedPoint.position, bossMoveSpeed * Time.deltaTime);
                        //activeCounter -= Time.deltaTime;

                        // as soon as the activeCounter reacher 0, make the boss vanish (play anim.)
                        if (Vector3.Distance(theBossItself.position, currentTargetedPoint.position) <= 0.02f)
                        {
                            // the fadeOutCounter's value is set here:
                            fadeOutCounter = fadeOutTime;
                            animator.SetTrigger("vanish");
                        }

                        shotCounter -= Time.deltaTime;

                        // shoot at intervals defined by the timeBetweenShotsPhase1 value:
                        if (shotCounter <= 0)
                        {
                            shotCounter = timeBetweenShotsPhase2;
                            Instantiate(bossShotPrefab, firePoint.position, Quaternion.identity);
                        }
                    }

                    else if (fadeOutCounter > 0)
                    {
                        // then the fadeOutCounter starts counting down:
                        fadeOutCounter -= Time.deltaTime;

                        // when it reaches zero, the boss itself is going to be disabled
                        if (fadeOutCounter <= 0)
                        {
                            theBossItself.gameObject.SetActive(false);

                            // the inactiveCounter's value is set here:
                            inactiveCounter = inactiveTime;
                        }
                    }

                    else if (inactiveCounter > 0)
                    {
                        // then the inactiveCounter starts counting down:
                        inactiveCounter -= Time.deltaTime;

                        if (inactiveCounter <= 0)
                        {
                            // the boss picks a random position to spawn at and appears again:
                            theBossItself.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                            currentTargetedPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                            int whileBreak = 0;

                            while (currentTargetedPoint.position == theBossItself.position && whileBreak < 50)
                            {
                                currentTargetedPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                                whileBreak++;
                            }

                            theBossItself.gameObject.SetActive(true);
                        }
                    }
                }
            }
            #endregion

            #region Third Phase
            // the third phase:
            else if (BossHealthController.instance.Health <= phase3Threshold)
            {
                animator.SetBool("phaseThree", true);

                if (currentTargetedPoint == null)
                {
                    currentTargetedPoint = theBossItself;
                    fadeOutCounter = fadeOutTime;
                    animator.SetTrigger("vanish");
                }

                else
                {
                    // if the boss is away from a targetPoint:
                    if (Vector3.Distance(theBossItself.position, currentTargetedPoint.position) > 0.02f)
                    {
                        theBossItself.position = Vector3.MoveTowards(theBossItself.position, currentTargetedPoint.position, bossMoveSpeed * Time.deltaTime);
                        //activeCounter -= Time.deltaTime;

                        // as soon as the activeCounter reacher 0, make the boss vanish (play anim.)
                        if (Vector3.Distance(theBossItself.position, currentTargetedPoint.position) <= 0.02f)
                        {
                            // the fadeOutCounter's value is set here:
                            fadeOutCounter = fadeOutTime;
                            animator.SetTrigger("vanish");
                        }

                        shotCounter -= Time.deltaTime;

                        // shoot at intervals defined by the timeBetweenShotsPhase1 value:
                        if (shotCounter <= 0)
                        {
                            shotCounter = timeBetweenShotsPhase3;
                            Instantiate(bossShotPrefab, firePoint.position, Quaternion.identity);
                        }
                    }

                    else if (fadeOutCounter > 0)
                    {
                        // then the fadeOutCounter starts counting down:
                        fadeOutCounter -= Time.deltaTime;

                        // when it reaches zero, the boss itself is going to be disabled
                        if (fadeOutCounter <= 0)
                        {
                            theBossItself.gameObject.SetActive(false);

                            // the inactiveCounter's value is set here:
                            inactiveCounter = inactiveTime;
                        }
                    }

                    else if (inactiveCounter > 0)
                    {
                        // then the inactiveCounter starts counting down:
                        inactiveCounter -= Time.deltaTime;

                        if (inactiveCounter <= 0)
                        {
                            // the boss picks a random position to spawn at and appears again:
                            theBossItself.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                            currentTargetedPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                            int whileBreak = 0;

                            while (currentTargetedPoint.position == theBossItself.position && whileBreak < 50)
                            {
                                currentTargetedPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                                whileBreak++;
                            }

                            theBossItself.gameObject.SetActive(true);
                        }
                    }
                }
            }
            #endregion 
        }

        else
        {
            fadeOutCounter -= Time.deltaTime;

            if (fadeOutCounter <= 0)
            {
                if (winObjectsParent)
                {
                    winObjectsParent.SetActive(true);
                    winObjectsParent.transform.SetParent(transform.parent);
                }

                cameraController.enabled = true;
                gameObject.SetActive(false);
            }
        }
    }

    public void EndBossBattle()
    {
        battleEnded = true;

        fadeOutCounter = fadeOutTime;
        animator.SetTrigger("vanish");
        theBossItself.GetComponent<Collider2D>().enabled = false;

        BossBullet[] bossBulletsActive = FindObjectsOfType<BossBullet>();

        foreach (BossBullet bullet in bossBulletsActive)
            Destroy(bullet.gameObject);

        PlayerPrefs.SetInt(bossPPKey, 1);
        AudioManager.instance.PlayLevelMusic();
    }
}
