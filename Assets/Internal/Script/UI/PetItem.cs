using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetItem : MonoBehaviour
{
    [SerializeField] private Image petImg;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI hpTxt;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private Button actionBtn;

    private FollowingAnimals followingAnimal = null;

    bool wasUpdate = false;
    private void Start()
    {
        actionBtn.onClick.AddListener(() =>
        {
            SetPetAction();
        });
    }
    private void Update()
    {
        if (followingAnimal != null && !wasUpdate)
        {
            wasUpdate = true;
            int level = followingAnimal.GetPetLevel();
            petImg.sprite = followingAnimal.petSprite;
            nameTxt.text = followingAnimal.petName;
            hpTxt.text = "HP: " + followingAnimal.GetHealthTxt();
            levelTxt.text = level == -1 ? "Level: Max" : "Level: " + level;
        }
    }
    public void SetFollowingAnimal(FollowingAnimals newFollowingAnimal)
    {
        followingAnimal = newFollowingAnimal;
    }
    public FollowingAnimals GetFollowingAnimal()
    {
        return followingAnimal;
    }
    private void SetPetAction()
    {
        PetAction.instance.ChangePetItem(this);
    }
}
