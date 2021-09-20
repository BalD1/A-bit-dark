using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrb : MonoBehaviour
{
    [Header("Light")]
    [SerializeField] private GameObject movableParts;

    [SerializeField] private Renderer movableSphereRenderer;
    public Renderer MovableSphereRenderer { get => movableSphereRenderer; }

    [SerializeField] private float lightSpeed;
    [SerializeField] private float lightSensitivity;
    [SerializeField] private float timeToReattach;

    private Vector3 mouseAxis;

    private bool attached;
    public bool Attached { get => attached; }

    [Header("Misc")]
    [SerializeField] private Player player;

    // Start is called before the first frame update
    void Start()
    {
        Reattach();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && movableSphereRenderer.isVisible)
            LightControl();
        if(Input.GetMouseButtonUp(1))
            Reattach();
    }

    private void LightControl()
    {
        attached = false;
        movableParts.transform.parent = null;
        mouseAxis.x = Input.GetAxis("Mouse X") * lightSensitivity * Time.deltaTime;
        mouseAxis.y = Input.GetAxis("Mouse Y") * lightSensitivity * Time.deltaTime;
        mouseAxis.z = Input.mouseScrollDelta.y * lightSensitivity * Time.deltaTime;

        movableParts.transform.Translate(mouseAxis, relativeTo: player.transform);
    }

    private void Reattach()
    {
        attached = true;
        movableParts.transform.parent = this.transform.parent;
        StartCoroutine(reattachPart(1));
    }

    private IEnumerator reattachPart(float timeToReattach)
    {
        float elapsedTime = 0;

        while(elapsedTime < timeToReattach)
        {
            movableParts.transform.position = Vector3.Lerp(movableParts.transform.position,
                                                           player.LightAttachPoint.transform.position,
                                                           (elapsedTime / timeToReattach));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(timeToReattach);
    }
}
