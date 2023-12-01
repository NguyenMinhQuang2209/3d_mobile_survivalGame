using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetAction : MonoBehaviour
{
    public static PetAction instance;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI inforTxt;
    [SerializeField] private Button stateBtn;
    [SerializeField] private TextMeshProUGUI stateTxt;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private TextMeshProUGUI upgradeTxt;
    [SerializeField] private Button closeBtn;

    [SerializeField] private GameObject petActionContainer;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }


    private PetItem currentPetItem = null;
    private void Start()
    {
        petActionContainer.SetActive(false);
        stateBtn.onClick.AddListener(() =>
        {
            ChangeMode();
        });
        upgradeBtn.onClick.AddListener(() =>
        {
            UpgradePet();
        });
        closeBtn.onClick.AddListener(() =>
        {
            ChangePetItem(null);
        });
    }
    public void ChangePetItem(PetItem newItem = null)
    {
        currentPetItem = newItem;
        petActionContainer.SetActive(currentPetItem != null);
        if (currentPetItem != null)
        {
            FollowingAnimals pet = currentPetItem.GetFollowingAnimal();
            int level = pet.GetPetLevel();
            nameTxt.text = pet.petName;
            stateTxt.text = pet.GetNextMode();
            inforTxt.text = "Trạng thái: " + pet.GetCurrentMode() + "\n"
                + "Máu:" + pet.GetHealthTxt() + "\n"
                + "Damage: " + pet.GetDamage() + "\n"
                + "Tốc độ: " + pet.GetSpeed() + "\n"
                + "Attack Time: " + pet.GetTimeBwtAttack();
            levelTxt.text = level == -1 ? "Level: Max" : "Level: " + level;
            upgradeTxt.text = "Nâng cấp \n(" + pet.GetNextPrice() + ")";
            upgradeBtn.gameObject.SetActive(level != -1);
        }
    }
    public void ChangeMode()
    {
        if (currentPetItem != null)
        {
            FollowingAnimals pet = currentPetItem.GetFollowingAnimal();
            pet.ChangeMode();
            stateTxt.text = pet.GetNextMode();
            inforTxt.text = "Trạng thái: " + pet.GetCurrentMode() + "\n"
                + "Máu:" + pet.GetHealthTxt() + "\n"
                + "Damage: " + pet.GetDamage() + "\n"
                + "Tốc độ: " + pet.GetSpeed() + "\n"
                + "Attack Time: " + pet.GetTimeBwtAttack();
        }
    }
    public void UpgradePet()
    {
        if (currentPetItem != null)
        {
            FollowingAnimals pet = currentPetItem.GetFollowingAnimal();
            bool updateState = pet.UpgradeLevel();
            if (updateState)
            {
                ChangePetItem(currentPetItem);
            }
        }
    }
}
