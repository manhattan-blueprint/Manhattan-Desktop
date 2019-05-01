using System.Collections;
using System.Collections.Generic;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using Service;
using Service.Request;
using Service.Response;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Controller {
    public class BlueprintTemplateController : MonoBehaviour, Subscriber<UIState>, Subscriber<InventoryState> {
        
        private int currentID = -1;
        private bool firstLoad = true;
        private SchemaItem currentSI;
        private GameObject title;
        private GameObject outline;
        private List<TextMeshProUGUI> componentNames;
        private List<TextMeshProUGUI> componentQuantities;
        private List<Image> componentChecks;
        private Button componentsButton;
        private TextMeshProUGUI usesTitle;
        private Image usesTable;
        private List<Image> usesLeft;
        private List<Image> usesMiddle;
        private List<Image> usesRight;
        private List<TextMeshProUGUI> usesLeftQuantities;
        private List<TextMeshProUGUI> usesMiddleQuantities;
        private List<Image> usesPlus;
        private List<Image> usesEquals;
        private GameObject uses;
        private TextMeshProUGUI easterEggName;
        private Dictionary<int, string> easterEggNameDict;

        void Start() {
            easterEggNameDict = new Dictionary<int, string> {{11, "Ben Lee"},
                {19, "Will JV Smith"},
                {18, "Jay Lees"},
                {20, "Adam Fox"},
                {25, "Andrei Nitu"},
                {23, "Elias K R"},
                {22, "Ben Lee"},
                {24, "Will JV Smith"},
                {26, "Jay Lees"},
                {29, "Adam Fox"},
                {28, "Andrei Nitu"},
                {31, "Elias K R"}
            };
            
            title   = GameObject.Find("TemplateTitle");
            outline = GameObject.Find("TemplateOutline");
            uses    = GameObject.Find("TemplateUses");
            easterEggName = GameObject.Find("TemplateEasterEggName").GetComponent<TextMeshProUGUI>();
            componentNames = new List<TextMeshProUGUI>();
            componentNames.Add(GameObject.Find("TemplateComponentNameOne").GetComponent<TextMeshProUGUI>());
            componentNames.Add(GameObject.Find("TemplateComponentNameTwo").GetComponent<TextMeshProUGUI>());
            componentNames.Add(GameObject.Find("TemplateComponentNameThree").GetComponent<TextMeshProUGUI>());
            componentQuantities = new List<TextMeshProUGUI>();
            componentQuantities.Add(GameObject.Find("TemplateComponentQuantityOne").GetComponent<TextMeshProUGUI>());
            componentQuantities.Add(GameObject.Find("TemplateComponentQuantityTwo").GetComponent<TextMeshProUGUI>());
            componentQuantities.Add(GameObject.Find("TemplateComponentQuantityThree").GetComponent<TextMeshProUGUI>());
            componentChecks = new List<Image>();
            componentChecks.Add(GameObject.Find("TemplateComponentCheckOne").GetComponent<Image>());
            componentChecks.Add(GameObject.Find("TemplateComponentCheckTwo").GetComponent<Image>());
            componentChecks.Add(GameObject.Find("TemplateComponentCheckThree").GetComponent<Image>());
            componentsButton = GameObject.Find("TemplateComponentsButton").GetComponent<Button>();
            usesTitle = GameObject.Find("TemplateUsesTitle").GetComponent<TextMeshProUGUI>();
            usesTable = GameObject.Find("TemplateUsesTable").GetComponent<Image>();
            usesLeft = new List<Image>();
            usesLeft.Add(GameObject.Find("TemplateUsesOneImageLeft").GetComponent<Image>());
            usesLeft.Add(GameObject.Find("TemplateUsesTwoImageLeft").GetComponent<Image>());
            usesLeft.Add(GameObject.Find("TemplateUsesThreeImageLeft").GetComponent<Image>());
            usesLeft.Add(GameObject.Find("TemplateUsesFourImageLeft").GetComponent<Image>());
            usesLeft.Add(GameObject.Find("TemplateUsesFiveImageLeft").GetComponent<Image>());
            usesLeft.Add(GameObject.Find("TemplateUsesSixImageLeft").GetComponent<Image>());
            usesMiddle = new List<Image>();
            usesMiddle.Add(GameObject.Find("TemplateUsesOneImageMiddle").GetComponent<Image>());
            usesMiddle.Add(GameObject.Find("TemplateUsesTwoImageMiddle").GetComponent<Image>());
            usesMiddle.Add(GameObject.Find("TemplateUsesThreeImageMiddle").GetComponent<Image>());
            usesMiddle.Add(GameObject.Find("TemplateUsesFourImageMiddle").GetComponent<Image>());
            usesMiddle.Add(GameObject.Find("TemplateUsesFiveImageMiddle").GetComponent<Image>());
            usesMiddle.Add(GameObject.Find("TemplateUsesSixImageMiddle").GetComponent<Image>());
            usesRight = new List<Image>();
            usesRight.Add(GameObject.Find("TemplateUsesOneImageRight").GetComponent<Image>());
            usesRight.Add(GameObject.Find("TemplateUsesTwoImageRight").GetComponent<Image>());
            usesRight.Add(GameObject.Find("TemplateUsesThreeImageRight").GetComponent<Image>());
            usesRight.Add(GameObject.Find("TemplateUsesFourImageRight").GetComponent<Image>());
            usesRight.Add(GameObject.Find("TemplateUsesFiveImageRight").GetComponent<Image>());
            usesRight.Add(GameObject.Find("TemplateUsesSixImageRight").GetComponent<Image>());
            usesLeftQuantities = new List<TextMeshProUGUI>();
            usesLeftQuantities.Add(GameObject.Find("TemplateUsesOneLeftQuantity").GetComponent<TextMeshProUGUI>());
            usesLeftQuantities.Add(GameObject.Find("TemplateUsesTwoLeftQuantity").GetComponent<TextMeshProUGUI>());
            usesLeftQuantities.Add(GameObject.Find("TemplateUsesThreeLeftQuantity").GetComponent<TextMeshProUGUI>());
            usesLeftQuantities.Add(GameObject.Find("TemplateUsesFourLeftQuantity").GetComponent<TextMeshProUGUI>());
            usesLeftQuantities.Add(GameObject.Find("TemplateUsesFiveLeftQuantity").GetComponent<TextMeshProUGUI>());
            usesLeftQuantities.Add(GameObject.Find("TemplateUsesSixLeftQuantity").GetComponent<TextMeshProUGUI>());
            usesMiddleQuantities = new List<TextMeshProUGUI>();
            usesMiddleQuantities.Add(GameObject.Find("TemplateUsesOneMiddleQuantity").GetComponent<TextMeshProUGUI>());
            usesMiddleQuantities.Add(GameObject.Find("TemplateUsesTwoMiddleQuantity").GetComponent<TextMeshProUGUI>());
            usesMiddleQuantities.Add(GameObject.Find("TemplateUsesThreeMiddleQuantity").GetComponent<TextMeshProUGUI>());
            usesMiddleQuantities.Add(GameObject.Find("TemplateUsesFourMiddleQuantity").GetComponent<TextMeshProUGUI>());
            usesMiddleQuantities.Add(GameObject.Find("TemplateUsesFiveMiddleQuantity").GetComponent<TextMeshProUGUI>());
            usesMiddleQuantities.Add(GameObject.Find("TemplateUsesSixMiddleQuantity").GetComponent<TextMeshProUGUI>());
            usesPlus = new List<Image>();
            usesPlus.Add(GameObject.Find("TemplateUsesOnePlus").GetComponent<Image>());
            usesPlus.Add(GameObject.Find("TemplateUsesTwoPlus").GetComponent<Image>());
            usesPlus.Add(GameObject.Find("TemplateUsesThreePlus").GetComponent<Image>());
            usesPlus.Add(GameObject.Find("TemplateUsesFourPlus").GetComponent<Image>());
            usesPlus.Add(GameObject.Find("TemplateUsesFivePlus").GetComponent<Image>());
            usesPlus.Add(GameObject.Find("TemplateUsesSixPlus").GetComponent<Image>());
            usesEquals = new List<Image>();
            usesEquals.Add(GameObject.Find("TemplateUsesOneEquals").GetComponent<Image>());
            usesEquals.Add(GameObject.Find("TemplateUsesTwoEquals").GetComponent<Image>());
            usesEquals.Add(GameObject.Find("TemplateUsesThreeEquals").GetComponent<Image>());
            usesEquals.Add(GameObject.Find("TemplateUsesFourEquals").GetComponent<Image>());
            usesEquals.Add(GameObject.Find("TemplateUsesFiveEquals").GetComponent<Image>());
            usesEquals.Add(GameObject.Find("TemplateUsesSixEquals").GetComponent<Image>());
            GameManager.Instance().uiStore.Subscribe(this);
            GameManager.Instance().inventoryStore.Subscribe(this);
        }

        public void StateDidUpdate(UIState state) {
            // Only process if Blueprint UI button pressed
            if (state.SelectedBlueprintID == currentID) return;
            currentID = state.SelectedBlueprintID;

            currentSI = GameManager.Instance().sm.GameObjs.items.Find(x => x.item_id == currentID);
            
            title.GetComponent<TextMeshProUGUI>().text = currentSI.name;
            outline.GetComponent<Image>().sprite = AssetManager.Instance().GetBlueprintOutline(currentID);

            int use = 0;
            
            // Find recipes
            for (int i = 0; i < GameManager.Instance().sm.GameObjs.items.Count; i++) {
                SchemaItem usesSI = GameManager.Instance().sm.GameObjs.items[i];
                if (usesSI.machine_id == currentID) {
                    if (usesSI.recipe.Count == 2) {
                        usesLeft[use].enabled = true;
                        usesLeft[use].sprite = AssetManager.Instance().GetItemSprite(usesSI.recipe[0].item_id);
                        usesLeftQuantities[use].enabled = true;
                        usesLeftQuantities[use].text = usesSI.recipe[0].quantity.ToString();
                        usesPlus[use].enabled = true;
                        usesMiddle[use].enabled = true;
                        usesMiddle[use].sprite = AssetManager.Instance().GetItemSprite(usesSI.recipe[1].item_id);
                        usesMiddleQuantities[use].enabled = true;
                        usesMiddleQuantities[use].text = usesSI.recipe[1].quantity.ToString();
                        usesEquals[use].enabled = true;
                    } else {
                        usesLeft[use].enabled = false;
                        usesLeftQuantities[use].enabled = false;
                        usesPlus[use].enabled = false;
                        usesMiddle[use].enabled = true;
                        usesMiddle[use].sprite = AssetManager.Instance().GetItemSprite(usesSI.recipe[0].item_id);
                        usesMiddleQuantities[use].enabled = true;
                        usesMiddleQuantities[use].text = usesSI.recipe[0].quantity.ToString();
                        usesEquals[use].enabled = true;
                    }
                    usesRight[use].enabled = true;
                    usesRight[use].sprite = AssetManager.Instance().GetItemSprite(usesSI.item_id);
                    use++;
                }
            }
            
            // Erase the rest
            for (int i = use; i < 6; i++) {
                usesLeft[i].enabled = false;
                usesLeftQuantities[i].enabled = false;
                usesPlus[i].enabled = false;
                usesMiddle[i].enabled = false;
                usesMiddleQuantities[i].enabled = false;
                usesEquals[i].enabled = false;
                usesRight[i].enabled = false;
            }
            
            // Hide recipes table if no recipes
            if (use == 0) {
                usesTitle.enabled = false;
                usesTable.enabled = false;
            } else {
                usesTitle.enabled = true;
                usesTable.enabled = true;
            }

            // Fill in component names
            for (int i = 0; i < 3; i++) {
                if (i < currentSI.blueprint.Count) {
                    RecipeElement currentRE = currentSI.blueprint[i];
                    componentNames[i].text = GameManager.Instance().sm.GameObjs.items
                        .Find(x => x.item_id == currentRE.item_id)
                        .name;
                }
            }
            
            // Change easter egg name
            easterEggName.text = easterEggNameDict[currentID];
            
            updateComponents();
        }

        public void StateDidUpdate(InventoryState state) {
            if (GameManager.Instance().uiStore.GetState().SelectedBlueprintID == -1) return;
            updateComponents();
        }

        private void updateComponents() {
            int quantityComplete = 0;
            for (int i = 0; i < 3; i++) {
                if (i < currentSI.blueprint.Count) {
                    RecipeElement currentRE = currentSI.blueprint[i];
                    int acquired = getQuantity(currentRE.item_id);
                    componentQuantities[i].text = acquired + " / " + currentRE.quantity;
                    componentChecks[i].enabled = true;
                    if (acquired >= currentRE.quantity) {
                        componentChecks[i].sprite = AssetManager.Instance().blueprintTemplateTick;
                        quantityComplete++;
                    } else {
                        componentChecks[i].sprite = AssetManager.Instance().blueprintTemplateCross;
                    }
                } else {
                    componentNames[i].text = "";
                    componentQuantities[i].text = "";
                    componentChecks[i].enabled = false;
                }
            }

            if (quantityComplete >= currentSI.blueprint.Count) {
                componentsButton.interactable = true;
            } else {
                componentsButton.interactable = false;
            }
        }
            

        // Get quantity of an item in inventory
        private int getQuantity(int id) {
            int sum = 0;

            foreach (KeyValuePair<int, List<HexLocation>> item in GameManager.Instance().inventoryStore.GetState()
                .inventoryContents) {
                if (item.Key == id) {
                    foreach (HexLocation hexLocation in item.Value) {
                        sum += hexLocation.quantity;
                    }
                }
            }

            return sum;
        }

        // Craft a blueprint, quantities already validated
        public void onCraftClick() {
            string name = GameManager.Instance().sm.GameObjs.items
                .Find(x => x.item_id == currentSI.item_id)
                .name;
            for (int i = 0; i < currentSI.blueprint.Count; i++) {
                GameManager.Instance().inventoryStore.Dispatch(new RemoveItemFromInventory(currentSI.blueprint[i].item_id,
                    currentSI.blueprint[i].quantity));
            }
            GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventory(currentSI.item_id, 1, name));
            
            // Update progress
            if (!GameManager.Instance().completedBlueprints.Contains(new Item(currentSI.item_id))) {
                AccessToken accessToken = GameManager.Instance().GetAccessToken();
                StartCoroutine(BlueprintAPI.AddCompletedBlueprints(accessToken,
                    new RequestCompletedBlueprint(currentSI.item_id),
                    blueprintsResult => {
                        if (!blueprintsResult.isSuccess()) {
                            // TODO: Handle error
                        } else {
                            GameManager.Instance().completedBlueprints.Add(new Item(currentSI.item_id));
                            
                            // Update Blueprint tree graphics in a naughty way
                            GameObject cell = GameObject.Find(name + "BlueprintCell");
                            cell.GetComponent<Image>().sprite = AssetManager.Instance().blueprintUICellPrimary;
                            SpriteState ss = new SpriteState();
                            ss.highlightedSprite = AssetManager.Instance().blueprintUICellPrimaryHighlight;
                            cell.GetComponent<Button>().spriteState = ss;

                            GameObject sprite = GameObject.Find(name + "BlueprintSprite");
                            sprite.GetComponent<Image>().sprite =
                                AssetManager.Instance().GetItemSprite(currentSI.item_id);
                        }
                }));
            }
        }

        public void onBackClick() {
            EventSystem.current.SetSelectedGameObject(null);
            GameManager.Instance().uiStore.Dispatch(new CloseUI());
        }
    }
}
