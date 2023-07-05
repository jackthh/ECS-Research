using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace _ECS_Research.Scripts.Survival_Game.Player
{
    public class InputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [BoxGroup("Singleton"), ShowInInspector, ReadOnly] public static InputHandler Instance { get; private set; }


        [BoxGroup("References"), ShowInInspector, ReadOnly] public PlayerInput PlayerInput { get; private set; }
        [BoxGroup("References"), SerializeField] private RectTransform joyStickOutline;
        [BoxGroup("References"), SerializeField] private RectTransform joyStickHandle;


        [BoxGroup("Status Properties"), ShowInInspector, ReadOnly] public bool GotTouch { get; private set; }
        [BoxGroup("Status Properties"), ShowInInspector, ReadOnly] public Vector2 CurrentDirection { get; private set; }


        [BoxGroup("Cache Data"), ShowInInspector, ReadOnly] private Vector2 pointerDownPos;
        [BoxGroup("Cache Data"), ShowInInspector, ReadOnly] private Vector2 pointerCurrentPos;
        [BoxGroup("Cache Data"), ShowInInspector, ReadOnly] private Vector2 joyStickOriginalPos;


        #region Mono Behaviour Callbacks

        private void Awake()
        {
            Instance = this;
            PlayerInput = GetComponent<PlayerInput>();
        }


        private void Start()
        {
            joyStickOriginalPos = joyStickOutline.anchoredPosition;
        }

        #endregion


        #region Input Callbacks

        public void OnPointerDown(PointerEventData _eventData)
        {
            GotTouch = true;
            joyStickOutline.gameObject.SetActive(true);

            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform) transform, _eventData.position, _eventData.pressEventCamera, out pointerDownPos);
            joyStickOutline.anchoredPosition = pointerDownPos;
        }


        public void OnPointerUp(PointerEventData _eventData)
        {
            GotTouch = false;
            joyStickOutline.anchoredPosition = joyStickOriginalPos;
            joyStickHandle.anchoredPosition = Vector2.zero;
        }


        public void OnDrag(PointerEventData _eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform) transform, _eventData.position, _eventData.pressEventCamera, out pointerCurrentPos);
            var offset = pointerCurrentPos - pointerDownPos;
            offset = Vector2.ClampMagnitude(offset, 75f);

            joyStickHandle.anchoredPosition = offset;
            CurrentDirection = offset.normalized;
        }

        #endregion
    }
}