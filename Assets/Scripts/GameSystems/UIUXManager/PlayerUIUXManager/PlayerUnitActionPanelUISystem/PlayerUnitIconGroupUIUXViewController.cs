using UnityEngine;
using Foundations.Architecture.ReferencesHandler;

using GameSystems.UtilitySystem;

namespace GameSystems.UIUXSystem
{
    public class PlayerUnitIconGroupUIUXViewController : MonoBehaviour
    {
        private UIAnimationHandler UIAnimationHandler_;

        [SerializeField] private RectTransform PlayerUnitIconGroupUIUXRectTransform;

        [SerializeField] private UIAnimationData ShowUIAnimationData_All_;
        [SerializeField] private UIAnimationData ShowUIAnimationData_Half_;
        [SerializeField] private UIAnimationData HideUIAnimationData_All_;
        [SerializeField] private UIAnimationData HideUIAnimationData_Half_;

        private PlayerUnitIconGroupUIUXDataGroup myPlayerUnitIconGroupUIUXDataGroup;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;

            this.UIAnimationHandler_ = HandlerManager.GetUtilityComponentHandler<UIAnimationHandler>();
        }

        public void InitialSetting(PlayerUnitIconGroupUIUXDataGroup playerUnitIconGroupUIUXDataGroup)
        {
            this.myPlayerUnitIconGroupUIUXDataGroup = playerUnitIconGroupUIUXDataGroup;
        }

        public void TogglePlayerBehaviourUIUX(int uniqueID)
        {
            if (!this.myPlayerUnitIconGroupUIUXDataGroup.PlayerUnitIconGroupUIUXDatas.TryGetValue(uniqueID, out var data01)) return;

            foreach (var data02 in this.myPlayerUnitIconGroupUIUXDataGroup.PlayerUnitIconGroupUIUXDatas.Values)
            {
                data02.IPlayerUnitIconUIUXViewController.ApplyInactiveColor();
                data02.PlayerUnitBehaviourIconGroupUIUXGameObject.SetActive(false);
            }

            data01.IPlayerUnitIconUIUXViewController.ApplyActiveColor();
            data01.PlayerUnitBehaviourIconGroupUIUXGameObject.SetActive(true);
        }

        public void ShowUIAnimationData_All()
        {
            this.UIAnimationHandler_.DoAnimation_Override(this.PlayerUnitIconGroupUIUXRectTransform, this.ShowUIAnimationData_All_);
        }
        public void HideUIAnimationData_All()
        {
            this.UIAnimationHandler_.DoAnimation_Override(this.PlayerUnitIconGroupUIUXRectTransform, this.HideUIAnimationData_All_);
        }
        public void ShowUIAnimationData_Half()
        {
            this.UIAnimationHandler_.DoAnimation_Override(this.PlayerUnitIconGroupUIUXRectTransform, this.ShowUIAnimationData_Half_);
        }
        public void HideUIAnimationData_Half()
        {
            this.UIAnimationHandler_.DoAnimation_Override(this.PlayerUnitIconGroupUIUXRectTransform, this.HideUIAnimationData_Half_);
        }
    }
}