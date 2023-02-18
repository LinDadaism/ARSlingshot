namespace ARSlingshot
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Implements UI button functionality. See <see cref="UnityEngine.XR.ARFoundation.Samples.BackButton"/> for the back button implementation.
    /// </summary>
    public class UIButtons : MonoBehaviour
    {
        [SerializeField]
        private GameObject canvas;

        [SerializeField]
        private GameObject togglePlacementButton;

        //[SerializeField]
        //private GameObject toggleManipulateButton;

        public SlingshotTouchResponder slingshotTouchResponder;
        public TextMeshProUGUI debugTextUI;

        private bool isPlacing;
        //private bool isManipulating;


        /// <summary>
        /// Gets a value indicating whether the user is currently idle, i.e., no special UI mode is active.
        /// </summary>
        public bool IsIdle => !this.isPlacing;  // && !this.isManipulating;

        public void TogglePlacementButtonPressed()
        {
            //this.SetManipulateState(false);
            this.SetPlacementState(!this.isPlacing);
        }

        //public void ToggleManipulateButtonPressed()
        //{
        //    //slingshotTouchResponder.gameObject.SetActive(!this.isPlacing); // && !this.isManipulating);
        //    this.SetPlacementState(false);
        //    //this.SetManipulateState(!this.isManipulating);
        //}

        /// <summary>
        /// Checks whether <paramref name="point"/> is over any UI element.
        /// </summary>
        /// <param name="point">The point in screen space.</param>
        /// <returns>True if the point is over any UI element, false otherwise.</returns>
        public bool IsPointOverUI(Vector2 point)
        {
            if (this.canvas.TryGetComponent<GraphicRaycaster>(out var graphicRaycaster))
            {
                var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current)
                {
                    position = point
                };
                var results = new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();
                graphicRaycaster.Raycast(pointerEventData, results);
                if (results.Count > 0)
                {
                    Debug.LogError(results[0].gameObject.name);
                    if (results[0].gameObject.name.Equals("SlingshotTouchResponder"))
                        return false;
                    return true;
                }
            }

            return false;
        }

        private void SetPlacementState(bool state)
        {
            this.isPlacing = state;
            var placeOnPlane = FindObjectOfType<PickUpResources>();
            if (placeOnPlane != null)
            {
                placeOnPlane.CanPlace = this.isPlacing;
                this.SetButtonState(this.togglePlacementButton, this.isPlacing);
            }
        }

        //private void SetManipulateState(bool state)
        //{
        //    this.isManipulating = state;
        //    var placeOnPlane = FindObjectOfType<PickUpResources>();
        //    if (placeOnPlane != null)
        //    {
        //        var spawnedObect = placeOnPlane.AirplanePrefab;
        //        if (spawnedObect != null)
        //        {
        //            if (spawnedObect.TryGetComponent<ARSelectionInteractable>(out var arInteractable))
        //            {
        //                arInteractable.enabled = this.isManipulating;
        //                this.SetButtonState(this.toggleManipulateButton, this.isManipulating);
        //            }
        //        }
        //    }
        //}

        private void SetButtonState(GameObject button, bool state)
        {
            button.GetComponent<Image>().color = state ? Color.green : Color.white;
            //slingshotTouchResponder.gameObject.SetActive(!state);
        }
    }
}
