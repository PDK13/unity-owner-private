using System.Collections;
using UnityEngine;

public class SampleGunBounce : MonoBehaviour
{
    [SerializeField] private Transform m_objectGun;

    [SerializeField] private Transform m_startPoint;
    [SerializeField] private Transform m_endPoint;

    [SerializeField] private SampleBulletBounce m_objectBullet;

    private void Start()
    {
        StartCoroutine(ISetShoot());
    }

    private void FixedUpdate()
    {
        Vector2 posStart = m_startPoint.position;
        Vector2 posEnd = m_endPoint.position;
        QTransform.SetRotate2D(m_objectGun, m_startPoint.position, m_endPoint.position);

        Debug.DrawLine(posStart, posEnd);
    }

    private IEnumerator ISetShoot()
    {
        do
        {
            yield return new WaitForSeconds(2f);

            GameObject bulletClone = QGameObject.SetCreate(m_objectBullet.gameObject);

            Vector2 posStart = m_startPoint.position;
            Vector2 posEnd = m_endPoint.position;
            Vector2 Dir = (posEnd - posStart).normalized;

            bulletClone.SetActive(true);
            bulletClone.GetComponent<SampleBulletBounce>().SetInit(Dir);
        }
        while (true);
    }
}