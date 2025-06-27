using System.Threading.Tasks;
using UnityEngine;

public class RewardCanvas : InteractiveCanvas
{
    [SerializeField] private Reward _reward;
    [SerializeField] private ModelView _modelView;
    
    protected override void HandleButtonClick()
    {
        // await _reward.ShowRewardAnimation();
        // _modelView.gameObject.SetActive(true);
    }
}

public class ModelView : MonoBehaviour
{
    
} 