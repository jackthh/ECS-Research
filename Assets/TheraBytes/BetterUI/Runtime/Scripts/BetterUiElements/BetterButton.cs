using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TheraBytes.BetterUi
{   
    [AddComponentMenu("Better UI/Controls/Better Button", 30)]
    public class BetterButton : Button, IBetterTransitionUiElement
    {
        public bool sfxBtn = true;
        [ShowIf("sfxBtn")]
        // public string btnSound = AudioGroupConstrant.ui_click;
        public List<Transitions> BetterTransitions { get { return betterTransitions; } }

        [SerializeField]
        List<Transitions> betterTransitions = new List<Transitions>();

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            // if (sfxBtn &&  !TQCSoundManager.Instance.IsDestroyed())
            // {
            //     TQCSoundManager.Instance.PlaySound(btnSound);
            // }
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (!(base.gameObject.activeInHierarchy))
                return;

            foreach (var info in betterTransitions)
            {
                info.SetState(state.ToString(), instant);
            }
        }
    }
}
