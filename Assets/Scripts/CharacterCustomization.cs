using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using Mirror;
public class CharacterCustomization : NetworkBehaviour
{
    [SerializeField] private Transform CharacterContain, AccessoryContain;
    private Transform currentCharacter;
    [SerializeField] private int charactherNumber, skinNumber, clothesNumber,accesoryNumber;
    [SerializeField] private Material[] materialsLightSkin;
    [SerializeField] private Material[] materialsMediumSkin;
    [SerializeField] private Material[] materialsDarkSkin;
     private SkinnedMeshRenderer Meshrenderer;
    [SerializeField] private Text CharacterName;
    [SerializeField] private string Character_Name;
    [SerializeField] public GameObject[] accessoryPrefab;
    private CustomNetworkManager manager;
    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null)
                return manager;
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }

    }
    public string GetName
    {
        get { return Character_Name; }
        set { Character_Name = value; }
    }
    void Start()
    {
        SetCharacter(2, 0, 0);
        SetAccessory(0);
    }
    void SetCharacter(int a, int b, int c)
    {
        for (int i = 2; i < CharacterContain.childCount; i++)
        {
            CharacterContain.GetChild(i).gameObject.SetActive(false);
        }
        charactherNumber = a;
        skinNumber = b;
        clothesNumber = c;
        currentCharacter = CharacterContain.GetChild(charactherNumber);
        currentCharacter.gameObject.SetActive(true);
        Meshrenderer = currentCharacter.GetComponent<SkinnedMeshRenderer>();        
        CharacterName.text = currentCharacter.name;
        switch (skinNumber)
        {
            case 0:
                Meshrenderer.material = materialsLightSkin[clothesNumber];
                break;
            case 1:
                Meshrenderer.material = materialsMediumSkin[clothesNumber];
                break;
            case 2:
                Meshrenderer.material = materialsDarkSkin[clothesNumber];
                break;
        }
    }
    public void NextBtn()
    {
        if (charactherNumber < CharacterContain.childCount - 1)
            charactherNumber++;

        SetCharacter(charactherNumber, skinNumber, clothesNumber);
    }
    public void PrevBtn()
    {
        if (charactherNumber > 2)
            charactherNumber--;

        SetCharacter(charactherNumber, skinNumber, clothesNumber);
    }
    public void ClothesBtn(int value)
    {
        clothesNumber = value;
        SetCharacter(charactherNumber, skinNumber, clothesNumber);
    }
    public void SkinBtn(int value)
    {
        skinNumber = value;
        SetCharacter(charactherNumber, skinNumber, clothesNumber);
    }
    public void ConfirmBtn()
    {
        PlayerObjectController playerObjectController = GameObject.Find("LocalGamePlayer").transform.GetComponent<PlayerObjectController>();
        if (playerObjectController.Ready)
        {
            if (GameObject.Find("LocalGamePlayer").transform.GetChild(0).childCount > 0)
                Destroy(GameObject.Find("LocalGamePlayer").transform.GetChild(0).GetChild(0).gameObject);
            GameObject prefab = (GameObject)Resources.Load(currentCharacter.name);
            GameObject player = Instantiate(prefab);
            player.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material = Meshrenderer.material;
            if (accesoryNumber > 0)
                Instantiate(accessoryPrefab[accesoryNumber - 1], player.transform.GetChild(1));
            player.transform.SetParent(GameObject.Find("LocalGamePlayer").transform.GetChild(0));
            player.transform.position = Vector3.zero;
            player.transform.rotation = Quaternion.Euler(Vector3.zero);
            player.transform.localScale = Vector3.one;
        }
    }
    public void ChangeName()
    {
        currentCharacter.name = Character_Name;
        CharacterName.text = currentCharacter.name;
    }
    public void AccessoryNextBtn()
    {
        if (accesoryNumber < AccessoryContain.childCount - 1)
        {
            accesoryNumber++;
        }           
        SetAccessory(accesoryNumber);
    }    
    public void AccessoryPrevBtn()
    {
        if (accesoryNumber > 0)
        {
            accesoryNumber--;
        }            
        SetAccessory(accesoryNumber);
    }
    public void SetAccessory(int number)
    {
        for (int i = 0; i < AccessoryContain.childCount; i++)
        {
            AccessoryContain.GetChild(i).gameObject.SetActive(false);
        }
        AccessoryContain.GetChild(number).gameObject.SetActive(true);
    }
}
