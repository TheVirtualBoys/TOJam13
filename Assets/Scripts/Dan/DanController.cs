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

    /// <summary>
    /// Flag that will be set if the Dan is currently moving.
    /// </summary>
    private bool isMoving = false;

    /// <summary>
    /// The coroutine 
    /// </summary>
    private Coroutine movementCoroutine = null;

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
        this.movementCoroutine = this.StartCoroutine(this.DanMovementCoroutine(point.DanLocation.position));
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
        this.danAnimationController.SetAnimationSkin(danOutfitID);
    }

    /// <summary>
    /// Dans the movement coroutine.
    /// </summary>
    /// <returns>The movement coroutine.</returns>
    private IEnumerator DanMovementCoroutine(Vector3 danDestinationWorldPos) {
        this.isMoving = true;

        // Figure out if the point is higher or lower than the current dan position.

        // Do the dan movement. For now just teleport.
        this.transform.position = danDestinationWorldPos;
        yield return new WaitForEndOfFrame();

        this.isMoving = false;
    }

    #endregion

}
