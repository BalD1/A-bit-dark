using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [System.Serializable]
    public struct PointTransform
    {
        public Vector3 pos;
        public Vector3 rot;
    }
    [SerializeField] private PointTransform pointTransform;

    private void Awake()
    {
        pointTransform.pos = this.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.respawnTransform = pointTransform;
    }
}
