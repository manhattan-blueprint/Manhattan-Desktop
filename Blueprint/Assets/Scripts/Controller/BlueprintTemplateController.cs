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
        
        private int                     currentID = -1;
        private SchemaItem              currentSI;
        private readonly Dictionary<int, string> notesDict = new Dictionary<int, string> {
            {11, "Fuel with wood or charcoal"},
            {20, "Fuel with wood or charcoal"},
            {25, "Generates electricity"},
            {22, "Place to connect solar panels to machines that require electricity"},
            {26, "Power with electricity"},
            {29, "Power with electricity"},
            {28, "Component of the communication beacon"},
            {31, "Component of the communication beacon"}
        };
        private readonly Dictionary<int, string> developerNameDict = new Dictionary<int, string> {
            {11, "Will JV Smith"},
            {19, "Jay Lees"},
            {18, "Adam fox"},
            {20, "Andrei Nitu"},
            {25, "Ben Lee"},
            {23, "Elias K R"},
            {22, "Will JV Smith"},
            {24, "Jay Lees"},
            {26, "Adam Fox"},
            {29, "Andrei Nitu"},
            {28, "Ben Lee"},
            {31, "Elias K R"}
        };
        
        // Blueprint item UI elements
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Image outline;
        
        // Components box UI elements
        [SerializeField] private List<TextMeshProUGUI> componentsNames;
        [SerializeField] private List<TextMeshProUGUI> componentsQuantities;
        [SerializeField] private List<Image>           componentsChecks;
        [SerializeField] private Button                componentsButton;
        
        // Recipe box UI elements
        [SerializeField] private TextMeshProUGUI       recipesTitle;
        [SerializeField] private List<Image>           recipesItemLeft;
        [SerializeField] private List<Image>           recipesItemMiddle;
        [SerializeField] private List<Image>           recipesItemRight;
        [SerializeField] private List<Image>           recipesPlus;
        [SerializeField] private List<Image>           recipesEquals;
        [SerializeField] private List<TextMeshProUGUI> recipesQuantityLeft;
        [SerializeField] private List<TextMeshProUGUI> recipesQuantityMiddle;
        [SerializeField] private Image                 recipesBox;
        
        // Notes box UI elements
        [SerializeField] private TextMeshProUGUI notesTitle;
        [SerializeField] private TextMeshProUGUI notesText;
        [SerializeField] private Image           notesBox;
        
        // Developer box UI elements
        [SerializeField] private TextMeshProUGUI developerName;

        void Start() {
            GameManager.Instance().uiStore.Subscribe(this);
            GameManager.Instance().inventoryStore.Subscribe(this);
        }

        public void StateDidUpdate(UIState state) {
            // Only process if Blueprint UI button pressed
            if (state.SelectedBlueprintID == currentID) return;
            currentID = state.SelectedBlueprintID;
            currentSI = GameManager.Instance().sm.GameObjs.items.Find(x => x.item_id == currentID);
            
            // Populate blueprint item UI elements
            title.text = currentSI.name;
            outline.sprite = AssetManager.Instance().GetBlueprintOutline(currentID);

            // Populate recipe box UI elements
            int recipeIndex = 0;
            
            for (int i = 0; i < GameManager.Instance().sm.GameObjs.items.Count; i++) {
                SchemaItem usesSI = GameManager.Instance().sm.GameObjs.items[i];
                if (usesSI.machine_id == currentID) {
                    if (usesSI.recipe.Count == 2) {
                        // Recipe requires two input items
                        recipesItemLeft[recipeIndex].enabled = true;
                        recipesItemLeft[recipeIndex].sprite = AssetManager.Instance().GetItemSprite(usesSI.recipe[0].item_id);
                        recipesItemMiddle[recipeIndex].enabled = true;
                        recipesItemMiddle[recipeIndex].sprite = AssetManager.Instance().GetItemSprite(usesSI.recipe[1].item_id);
                        recipesQuantityMiddle[recipeIndex].enabled = true;
                        recipesQuantityMiddle[recipeIndex].text = usesSI.recipe[1].quantity.ToString();
                        recipesQuantityLeft[recipeIndex].enabled = true;
                        recipesQuantityLeft[recipeIndex].text = usesSI.recipe[0].quantity.ToString();
                        recipesPlus[recipeIndex].enabled = true;
                        recipesEquals[recipeIndex].enabled = true;
                    } else {
                        // Recipe requires one input item
                        recipesItemLeft[recipeIndex].enabled = false;
                        recipesItemMiddle[recipeIndex].enabled = true;
                        recipesItemMiddle[recipeIndex].sprite = AssetManager.Instance().GetItemSprite(usesSI.recipe[0].item_id);
                        recipesQuantityLeft[recipeIndex].enabled = false;
                        recipesQuantityMiddle[recipeIndex].enabled = true;
                        recipesQuantityMiddle[recipeIndex].text = usesSI.recipe[0].quantity.ToString();
                        recipesPlus[recipeIndex].enabled = false;
                        recipesEquals[recipeIndex].enabled = true;
                    }
                    recipesItemRight[recipeIndex].enabled = true;
                    recipesItemRight[recipeIndex].sprite = AssetManager.Instance().GetItemSprite(usesSI.item_id);
                    recipeIndex++;
                }
            }
            
            // Hide the remaining recipe elements
            for (int i = recipeIndex; i < 6; i++) {
                recipesItemLeft[i].enabled = false;
                recipesItemMiddle[i].enabled = false;
                recipesItemRight[i].enabled = false;
                recipesQuantityLeft[i].enabled = false;
                recipesQuantityMiddle[i].enabled = false;
                recipesPlus[i].enabled = false;
                recipesEquals[i].enabled = false;
            }
            
            // Hide recipes box if no recipes
            if (recipeIndex == 0) {
                recipesTitle.enabled = false;
                recipesBox.enabled = false;
            } else {
                recipesTitle.enabled = true;
                recipesBox.enabled = true;
            }

            // Populate components box UI elements
            for (int i = 0; i < 3; i++) {
                if (i < currentSI.blueprint.Count) {
                    RecipeElement currentRE = currentSI.blueprint[i];
                    componentsNames[i].text = GameManager.Instance().sm.GameObjs.items
                        .Find(x => x.item_id == currentRE.item_id)
                        .name;
                }
            }
            updateComponents();
            
            // Populate notes box
            if (notesDict.ContainsKey(currentID)) {
                notesTitle.enabled = true;
                notesText.enabled = true;
                notesText.text = notesDict[currentID];
                notesBox.enabled = true;
            } else {
                notesTitle.enabled = false;
                notesText.enabled = false;
                notesBox.enabled = false;
            }
            
            // Change developer name
            developerName.text = developerNameDict[currentID];
        }

        public void StateDidUpdate(InventoryState state) {
            if (GameManager.Instance().uiStore.GetState().SelectedBlueprintID == -1) return;
            // TODO: Stop updating component UI on every inventory state change
            updateComponents();
        }

        private void updateComponents() {
            int quantityComplete = 0;
            for (int i = 0; i < 3; i++) {
                if (i < currentSI.blueprint.Count) {
                    RecipeElement currentRE = currentSI.blueprint[i];
                    int acquired = getQuantity(currentRE.item_id);
                    componentsQuantities[i].text = acquired + " / " + currentRE.quantity;
                    componentsChecks[i].enabled = true;
                    if (acquired >= currentRE.quantity) {
                        componentsChecks[i].sprite = AssetManager.Instance().blueprintTemplateTick;
                        quantityComplete++;
                    } else {
                        componentsChecks[i].sprite = AssetManager.Instance().blueprintTemplateCross;
                    }
                } else {
                    componentsNames[i].text = "";
                    componentsQuantities[i].text = "";
                    componentsChecks[i].enabled = false;
                }
            }

            if (quantityComplete == currentSI.blueprint.Count) {
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

        // Craft a blueprint, quantities validated in updateComponents
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
            // This probably has side effects ngl
            EventSystem.current.SetSelectedGameObject(null);
            GameManager.Instance().uiStore.Dispatch(new CloseUI());
        }
    }
}
