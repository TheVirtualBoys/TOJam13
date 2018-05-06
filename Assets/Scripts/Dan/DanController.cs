using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The dan controller controls dan.
/// </summary>
public class DanController : MonoBehaviour {

    #region Data

    /// <summary>
    /// The animation controller for controlling Dan.
    /// </summary>
    [SerializeField]
    private SpineAnimMecanimController danAnimationController = null;

    [SerializeField, Header("----- Movement -----")]
    private float danSpeed = 0f;

    /// <summary>
    /// Flag that will be set if the Dan is currently moving.
    /// </summary>
    private bool isMoving = false;

    /// <summary>
    /// The coroutine 
    /// </summary>
    private Coroutine movementCoroutine = null;

    #endregion

    #region Monobehaviour

    private void Start() {
        AdventureLog.Instance.OnEventAnimationRequest += this.SetDanimation;
        AdventureLog.Instance.OnDanOutfitRequest += this.SetDanClothes;
    }

    private void OnDestroy() {
        AdventureLog.Instance.OnEventAnimationRequest -= this.SetDanimation;
        AdventureLog.Instance.OnDanOutfitRequest -= this.SetDanClothes;
    }

    #endregion

    #region Dan Logic

    /// <summary>
    /// This will move the Dan from it's current location to the 
    /// </summary>
    /// <param name="point">Point.</param>
    public void MoveToPoint(InteractionPoint point) {
        if (this.isMoving) {
            this.StopCoroutine(this.movementCoroutine);
        }

        this.isMoving = true;
        // Moves the dan from his current location to the new location.
        this.movementCoroutine = this.StartCoroutine(this.DanMovementCoroutine(point));
    }

    /// <summary>
    /// Sets the animation state of the Dan.
    /// </summary>
    /// <param name="animationState">Animation state.</param>
    public void SetDanimation(string animationState) {
        this.danAnimationController.Trigger(animationState);
    }

    /// <summary>
    /// Sets the clothes that dan is wearing.
    /// </summary>
    /// <param name="danOutfitID">Dan outfit.</param>
    public void SetDanClothes(string danOutfitID) {
        Debug.Log("setting dan clothes: " + danOutfitID);
        this.danAnimationController.SetAnimationSkin(danOutfitID);
    }

    /// <summary>
    /// Dans the movement coroutine.
    /// </summary>
    /// <returns>The movement coroutine.</returns>
    private IEnumerator DanMovementCoroutine(InteractionPoint moveToPoint) {
        Vector3 danDestinationWorldPos = moveToPoint.DanLocation.position;


        if (!Mathf.Approximately(this.transform.position.x, moveToPoint.DanLocation.position.x) ||
            !Mathf.Approximately(this.transform.position.y, moveToPoint.DanLocation.position.y)) {
            this.isMoving = true;
            string walkingAnimation = "FrontWalk";
            // Figure out if the point is higher or lower than the current dan position.
            if (danDestinationWorldPos.y > this.transform.position.y) {
                walkingAnimation = "BackWalk";
            }

            this.SetDanimation(walkingAnimation);
            while (true) {
                Vector2 direction = danDestinationWorldPos - this.transform.position;
                if (direction.sqrMagnitude < ((this.danSpeed * Time.deltaTime) * (this.danSpeed * Time.deltaTime))) {
                    break;
                }
                direction.Normalize();
                this.transform.Translate((direction * danSpeed) * Time.deltaTime, Space.World);
                yield return new WaitForEndOfFrame();
            }

            this.transform.position = danDestinationWorldPos;
        }

        this.SetDanimation(moveToPoint.InteractionAreaIdleAnimation);
        this.isMoving = false;
    }

    #endregion

}
