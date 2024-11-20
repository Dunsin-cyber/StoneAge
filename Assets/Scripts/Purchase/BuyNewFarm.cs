using UnityEngine;

public class ButNewFarm : MonoBehaviour
{
    // Function to explicitly enable the GameObject
    public void EnableNewFarmingLand()
    {
        gameObject.SetActive(true);
    }

    // Function to explicitly disable the GameObject
    public void DisableNewFarmingLand()
    {
        gameObject.SetActive(false);
    }

    public void BuyLand()
    {
        int bal = UIManager.Instance.GetCoinBalance();

        if (bal > 999)
        {
            EnableNewFarmingLand();
            UIManager.Instance.RemoveFromCoinsForLand();
            PurchaseManager.Instance.RenderPurchaseManager();
        }
    }
}
