using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PlayerTarget))]
[RequireComponent(typeof(AITarget))]
public class AIBehaviour : MonoBehaviour
{
    public int damage;

    public float moveSpeed;

    public float fireRate;

    public float minDistanceToShoot;

    private float _nextTimeToFire;

    public float radius;

    public Transform gunTip;

    [SerializeField] private List<GameObject> _targets = new List<GameObject>();

    [SerializeField] private List<GameObject> _newTargets = new List<GameObject>();

    [SerializeField] private Transform _target;

    [SerializeField] private Transform _newTarget;

    private NavMeshAgent _agent;

    private Animator _anim;

    private bool _shooting;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        GatheringTargetsInformation();

        _agent.speed = moveSpeed;
        _countdownToSearch = nextTimeToSearch;
        _nextTimeToFire = 0;
    }

    private void Update()
    {
        FindTarget();
    }

    void FindTarget()
    {
        if (_target != null)
        {
            MoveandRotateAgent(_target);
            if (InShootingRange())
            {
                if (!_shooting)
                {
                    _shooting = true;
                    Shoot();
                }
            }
            else return;

            /*
            if (NewTargetAppearsWhileMoving())
            {
                MoveandRotateAgent(_newTarget);
                if (InShootingRange())
                {
                    //StartCoroutine(Aim());
                    if (!_shooting)
                    {
                        _shooting = true;

                        Shoot();
                    }
                }
                else return;

            }
            */
        }

        else
        {
            GatheringTargetsInformation();
            MoveandRotateAgent(_target);
            if (InShootingRange())
            {
                if (!_shooting)
                {
                    _shooting = true;
                    Shoot();
                }
            }
            else return;
        }

    }

    void Shoot()
    {
        Debug.Log("Fire!");
        _anim.SetBool("Fire", true);

        if (_nextTimeToFire <= Time.time)
        {
            FindObjectOfType<AudioManager>().Play("Rifle");

            _nextTimeToFire = Time.time + 1 / fireRate;
            RaycastHit hit;
            if (Physics.Raycast(gunTip.position, gunTip.forward, out hit, Mathf.Infinity))
            {
                PlayerTarget playerTarget = hit.transform.gameObject.GetComponent<PlayerTarget>();
                AITarget aiTarget = hit.transform.gameObject.GetComponent<AITarget>();

                if (playerTarget != null)
                {
                    playerTarget.Hurt(damage, this.gameObject.transform);
                }

                if (aiTarget != null)
                {
                    SphereCollider aiHead = hit.transform.gameObject.GetComponent<SphereCollider>();
                    CapsuleCollider aiBody = hit.transform.gameObject.GetComponent<CapsuleCollider>();
                    if (aiHead != null && hit.collider == aiHead)
                    {
                        aiTarget.HeadShot();
                        GameObject electricHit = Instantiate(aiTarget.electricHitPrefab, hit.point, Quaternion.identity);
                    }
                    else if (aiBody != null && hit.collider == aiBody)
                    {
                        aiTarget.TakeDamage(damage);
                    }
                }
            }
        }
    }

    /*Problem */

    bool NewTargetAppearsWhileMoving()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject == this.gameObject && colliders[i].gameObject.CompareTag("Target"))
            {
                return false;
            }

            if (colliders[i].gameObject != this.gameObject)
            {
                if (!colliders[i].gameObject.CompareTag("Target"))
                {
                    continue;
                }
                _newTargets.Add(colliders[i].gameObject);
            }
        }

        if (_newTargets.Count == 0)
        {
            return false;
        }
        else
        {
            _newTarget = _newTargets[Random.Range(0, _newTargets.Count)].transform;
            if (_newTarget != null)
            {
                Debug.Log("Found new Target!");
                return true;
            }
            else Debug.Log("No new target is found while moving");
        }
        return false;
    }

    /*Problem */

    void GatheringTargetsInformation()
    {
        GameObject[] howManyTargets = GameObject.FindGameObjectsWithTag("Target");
        if (howManyTargets != null)
        {
            for (int i = 0; i < howManyTargets.Length; i++)
            {
                if (howManyTargets[i] == this.gameObject)
                {
                    continue;
                }
                Debug.Log(howManyTargets[i].name);
                _targets.Add(howManyTargets[i]);
            }
            _target = _targets[Random.Range(0, _targets.Count)].transform;
        }

        else Debug.Log("All enemies have been killed!");
    }

    void MoveandRotateAgent(Transform target)
    {
        _agent.speed = moveSpeed;
        _anim.SetFloat("Speed", _agent.speed);
        _anim.SetBool("Fire", false);
        _agent.SetDestination(target.position);
        _shooting = false;
        _agent.isStopped = false;

        Vector3 lookPos = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(lookPos.x, 0, lookPos.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (_agent.remainingDistance <= minDistanceToShoot)
        {
            _agent.isStopped = true;
            _agent.speed = 0;
            _anim.SetFloat("Speed", _agent.speed);
        }
        else return;
    }

    bool InShootingRange()
    {
        RaycastHit raycasthit;
        if (Physics.Raycast(gunTip.position, gunTip.forward, out raycasthit, Mathf.Infinity))
        {
            if (raycasthit.transform.gameObject.tag == "Target")
            {
                Debug.Log("Found Target! Getting ready to have a clear shot!");
                _agent.isStopped = true;
                _agent.speed = 0;
                _anim.SetFloat("Speed", _agent.speed);
                return true;
            }
            else
            {
                _agent.isStopped = false;
                _agent.speed = moveSpeed;
                _anim.SetFloat("Speed", _agent.speed);
                return false;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}