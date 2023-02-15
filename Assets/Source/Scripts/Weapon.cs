using System.Collections;
using UnityEngine;
using System;
using Photon.Pun;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float _maxDistance = 200f;
    [SerializeField] private float _damage;
    [SerializeField] private float _reloadInterval;

    private ParticleSystem _hitEffect;

    private Coroutine _reloading;
    private Coroutine _shooting;

    public bool IsStrikeAvailable { get; private set; } = true;

    public void Init(ParticleSystem hitEffect)
    {
        _hitEffect = hitEffect;
    }

    public void StartShooting()
    {
        _shooting = StartCoroutine(Shooting());
    }

    public void StopShooting()
    {
        if (_shooting != null)
        {
            StopCoroutine(_shooting);
        }
    }

    private IEnumerator Shooting()
    {
        while (true)
        {
            if (IsStrikeAvailable)
            {
                Shoot();
            }
            yield return null;
        }
    }

    private void Reload()
    {
        _reloading = StartCoroutine(Reloading());
    }

    public void CancelReloading()
    {
        if (_reloading != null)
        {
            StopCoroutine(_reloading);
        }
    }

    private IEnumerator Reloading()
    {
        float time = 0;

        while (_reloadInterval > time)
        {
            time += Time.deltaTime;
            yield return null;
        }

        IsStrikeAvailable = true;
    }

    private void Shoot()
    {
        if (IsStrikeAvailable == false)
        {
            throw new InvalidOperationException("Gun doesn't reload. Reload gun before shooting.");
        }

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, _maxDistance))
        {
            ParticleSystem particleSystem = PhotonNetwork.Instantiate(_hitEffect.name, hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal) * Quaternion.Euler(90, 0, 0)).GetComponent<ParticleSystem>();
            StartCoroutine(DestroyImpact(particleSystem.gameObject, 2f));

            if (hitInfo.collider.TryGetComponent(out Character character))
            {
                character.TakeDamage(_damage);
            }
        }

        IsStrikeAvailable = false;
        Reload();
    }

    private IEnumerator DestroyImpact(GameObject particleSystem, float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.Destroy(particleSystem);
    }
}