using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorController : MonoBehaviour
{
    private AntController _antController;
    private void Awake()
    {
        _antController = GetComponentInParent<AntController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            if (CompareTag("CollisionDetector")) _antController.OnFoodTouched(collision);
            else if (CompareTag("ObjDetector")) _antController.OnFoodFound(collision);
        }

        else if (collision.CompareTag("Home"))
        {
            if (CompareTag("CollisionDetector")) _antController.OnHomeTouched(collision);
            else if (CompareTag("ObjDetector")) _antController.OnHomeFound(collision);
        }

        else if (CompareTag("MarkerDetector") && (collision.CompareTag("ToHome") || collision.CompareTag("ToFood"))) _antController.OnMarkerFound(collision);

        else if (CompareTag("CollisionDetector") && collision.CompareTag("Borders")) _antController.OnWallCollide();

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CompareTag("MarkerDetector") && (collision.CompareTag("ToHome") || collision.CompareTag("ToFood"))) _antController.OnMarkerLost(collision);
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {

            
    }*/



}
