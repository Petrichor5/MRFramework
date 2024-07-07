using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MRFramework
{
    public class ButtonClickTween : MonoBehaviour
    {
        private Button mButton;
        private Vector3 mOriginalScale;

        public float Duration = 0.2f;
        public float ScaleFactor = 0.8f;

        private void Start()
        {
            mButton = GetComponent<Button>();
            if (mButton == null)
            {
                Debug.LogError("Button component not found!");
                return;
            }

            mOriginalScale = transform.localScale;

            AddEventTrigger(mButton.gameObject);
        }

        private void AddEventTrigger(GameObject buttonObj)
        {
            UnityEngine.EventSystems.EventTrigger trigger = buttonObj.GetComponent<UnityEngine.EventSystems.EventTrigger>();
            if (trigger == null)
            {
                trigger = buttonObj.AddComponent<UnityEngine.EventSystems.EventTrigger>();
            }

            var pointerDown = new UnityEngine.EventSystems.EventTrigger.Entry
            {
                eventID = UnityEngine.EventSystems.EventTriggerType.PointerDown
            };
            pointerDown.callback.AddListener((data) => { OnPointerDown(); });
            trigger.triggers.Add(pointerDown);

            var pointerUp = new UnityEngine.EventSystems.EventTrigger.Entry
            {
                eventID = UnityEngine.EventSystems.EventTriggerType.PointerUp
            };
            pointerUp.callback.AddListener((data) => { OnPointerUp(); });
            trigger.triggers.Add(pointerUp);
        }

        private void OnPointerDown()
        {
            transform.DOScale(mOriginalScale * ScaleFactor, Duration);
        }

        private void OnPointerUp()
        {
            transform.DOScale(mOriginalScale, Duration);
        }
    }
}