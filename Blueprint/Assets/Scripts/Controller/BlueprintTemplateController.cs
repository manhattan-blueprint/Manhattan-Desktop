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

        void Start() {
            title   = GameObject.Find("TemplateTitle");
            outline = GameObject.Find("TemplateOutline");
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

            int quantityComplete = 0;

            for (int i = 0; i < 3; i++) {
                if (i < currentSI.blueprint.Count) {
                    RecipeElement currentRE = currentSI.blueprint[i];
                    componentNames[i].text = GameManager.Instance().sm.GameObjs.items
                        .Find(x => x.item_id == currentRE.item_id)
                        .name;
                }
            }
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
                    componentQuantities[i].text = acquired + "/" + currentRE.quantity;
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
