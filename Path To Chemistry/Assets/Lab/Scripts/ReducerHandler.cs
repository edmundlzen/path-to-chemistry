using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class ReducerData
{
    public static int reduceNum;
    public static bool hasDone = false;
}

public class ReducerHandler : MonoBehaviour
{
    private void Start()
    {
        reducerCheck();
    }

    public void Slider(float Value)
    {
        GameObject.Find("sliderValue").GetComponent<Text>().text = Convert.ToString(Math.Floor(Value));
    }

    public void maxQuantity()
    {
        GameObject.Find("Slider").GetComponent<Slider>().value = GameObject.Find("Slider").GetComponent<Slider>().maxValue;
    }

    public void addQuantity()
    {
        if (GameObject.Find("Slider").GetComponent<Slider>().value + 1 <= GameObject.Find("Slider").GetComponent<Slider>().maxValue)
        {
            GameObject.Find("Slider").GetComponent<Slider>().value += 1;
        }
    }
    public void removeQuantity()
    {
        if (GameObject.Find("Slider").GetComponent<Slider>().value - 1 >= GameObject.Find("Slider").GetComponent<Slider>().minValue)
        {
            GameObject.Find("Slider").GetComponent<Slider>().value -= 1;
        }
    }

    public void addCompound(GameObject addreducerQuantity)
    {
        var playerData = PlayerData.Instance();
        if ((playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] != null) && (playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] != null))
        {
            if (Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) > 1)
            {
                addreducerQuantity.SetActive(true);
                GameObject.Find("addCompoundQuantity/Slider").GetComponent<Slider>().maxValue = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]);
            }
            else
            {
                if (playerData.Compound.ContainsKey(Convert.ToString(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"])) && playerData.Compound[Convert.ToString(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"])] + 1 <= 64)
                {
                    if (playerData.Compound[Convert.ToString(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"])] + 1 <= 64)
                    {
                        playerData.Compound[Convert.ToString(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"])] += 1;
                    }
                    else
                    {
                        addAlert($"Alert: The compound reducer's hotbar will only exist one stack of {playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]} Elements!");
                        return;
                    }
                }
                else
                {
                    if (playerData.Compound.Count < 1)
                    {
                        if (Chemidex.moleculeRecipes["Symbol"].ContainsKey(Convert.ToString(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"])))
                        {
                            playerData.Compound.Add(Convert.ToString(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]), 1);
                        }
                        else
                        {
                            addAlert("Alert: Only compounds are allow to put in the compound reducer's hotbar!");
                            return;
                        }
                    }
                    else
                    {
                        addAlert("Alert: The compound reducer's hotbar are full!");
                        return;
                    }
                }
                playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] = null;
                playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = null;
                slotCheck();
                reducerCheck();
            }
        }
    }

    public void addCompoundQuantity(GameObject addreducerQuantity)
    {
        var playerData = PlayerData.Instance();
        var sliderValue = Convert.ToInt32(Math.Floor(GameObject.Find("addCompoundQuantity/Slider").GetComponent<Slider>().value));
        GameObject.Find("addCompoundQuantity/Slider").GetComponent<Slider>().value = 0;
        addreducerQuantity.SetActive(false);
        if (sliderValue > 0)
        {
            if (playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] != null && playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] != null && playerData.Compound.Count <= 1)
            {
                if (playerData.Compound.ContainsKey(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()))
                {
                    var Balance = Convert.ToInt32(playerData.Compound[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()]);
                    if (playerData.Compound[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] + sliderValue <= 64)
                    {
                        playerData.Compound[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] = Balance + sliderValue;
                    }
                    else
                    {
                        playerData.Compound[playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString()] = Balance + (64 - Balance);
                        playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) - (64 - Balance);
                        addAlert($"Alert: The compound reducer's hotbar will only exist one stack of {playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]} Elements!");
                        addReducer();
                        return;
                    }
                }
                else
                {
                    if (playerData.Compound.Count < 1)
                    {
                        if (Chemidex.moleculeRecipes["Symbol"].ContainsKey(Convert.ToString(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"])))
                        {
                            if (sliderValue <= 64)
                            {
                                playerData.Compound.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString(), sliderValue);
                            }
                            else
                            {
                                sliderValue = 64;
                                playerData.Compound.Add(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"].ToString(), sliderValue);
                                addAlert($"Alert: The maximum quantity in one stack of {playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]} elements is 64!");
                            }
                        }
                        else
                        {
                            addAlert("Alert: Only compounds are allow to put in the compound reducer's hotbar!");
                            return;
                        }
                    }
                    else
                    {
                        addAlert("Alert: The compound reducer's hotbar are full!");
                        return;
                    }
                }
                playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) - sliderValue;
                addReducer();
            }
        }
    }

    public void getReducer(GameObject getReducerQuantity)
    {
        var playerData = PlayerData.Instance();
        if (playerData.Compound.Count >= 1)
        {
            if (playerData.Compound.Values.ElementAt(0) > 1)
            {
                getReducerQuantity.SetActive(true);
                GameObject.Find("getCompoundQuantity/Slider").GetComponent<Slider>().maxValue = playerData.Compound.Values.ElementAt(0);
                GameObject.Find("getCompoundQuantity/Slider").GetComponent<Slider>().value = 0;
            }
            else
            {
                for (var i = 1; i <= 9; i = i + 1)
                {
                    if (Convert.ToString(playerData.slotItem[$"Slot{i}"]["Element"]) == playerData.Compound.Keys.ElementAt(0))
                    {
                        if (Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) + 1 <= 64)
                        {
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) + 1;
                            ReducerData.hasDone = true;
                            break;
                        }
                        else
                        {
                            addAlert($"Alert: The player's hotbar will only exist one stack of {playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]} Elements!");
                            return;
                        }
                    }
                }
                if (!ReducerData.hasDone)
                {
                    for (var i = 1; i <= 9; i = i + 1)
                    {
                        if (playerData.slotItem[$"Slot{i}"]["Element"] == null && playerData.slotItem[$"Slot{i}"]["Quantity"] == null)
                        {
                            playerData.slotItem[$"Slot{i}"]["Element"] = playerData.Compound.Keys.ElementAt(0);
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = 1;
                            ReducerData.hasDone = true;
                            break;
                        }
                    }
                }
                if (!ReducerData.hasDone)
                {
                    for (var i = 1; i <= 9; i = i + 1)
                    {
                        if (playerData.slotItem[$"Slot{i}"]["Element"] != null && playerData.slotItem[$"Slot{i}"]["Quantity"] != null)
                        {
                            addAlert("Alert: The player's hotbar are full!");
                            return;
                        }
                    }
                }
                ReducerData.hasDone = false;
                playerData.Compound.Remove(playerData.Compound.Keys.ElementAt(0));
                slotCheck();
                reducerCheck();
            }
        }
    }

    public void getCompoundQuantity(GameObject getReducerQuantity)
    {
        var playerData = PlayerData.Instance();
        var sliderValue = Convert.ToInt32(Math.Floor(GameObject.Find("getCompoundQuantity/Slider").GetComponent<Slider>().value));
        getReducerQuantity.SetActive(false);
        if (sliderValue > 0)
        {
            if (playerData.Compound.Count >= 1)
            {
                for (var i = 1; i <= 9; i = i + 1)
                {
                    if (Convert.ToString(playerData.slotItem[$"Slot{i}"]["Element"]) == playerData.Compound.Keys.ElementAt(0))
                    {
                        var Balance = Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]);
                        if (Balance + sliderValue <= 64)
                        {
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = Balance + sliderValue;
                            ReducerData.hasDone = true;
                            break;
                        }
                        else
                        {
                            playerData.slotItem[$"Slot{i}"]["Quantity"] = Balance + (64 - Balance);
                            playerData.Compound[playerData.Compound.Keys.ElementAt(0)] -= 64 - Balance;
                            addAlert($"Alert: The player's hotbar will only exist one stack of {playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]} Elements!");
                            getReducer();
                            return;
                        }
                    }
                }
                if (!ReducerData.hasDone)
                {
                    for (var i = 1; i <= 9; i = i + 1)
                    {
                        if (playerData.slotItem[$"Slot{i}"]["Element"] == null && playerData.slotItem[$"Slot{i}"]["Quantity"] == null)
                        {
                            if (sliderValue <= 64)
                            {
                                playerData.slotItem[$"Slot{i}"]["Element"] = playerData.Compound.Keys.ElementAt(0);
                                playerData.slotItem[$"Slot{i}"]["Quantity"] = sliderValue;
                                ReducerData.hasDone = true;
                                break;
                            }
                            else
                            {
                                sliderValue = 64;
                                playerData.slotItem[$"Slot{i}"]["Element"] = playerData.Compound.Keys.ElementAt(0);
                                playerData.slotItem[$"Slot{i}"]["Quantity"] = sliderValue;
                                addAlert($"Alert: The maximum quantity in one stack of {playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"]} elements is 64!");
                                ReducerData.hasDone = true;
                                break;
                            }
                        }
                    }
                }
                if (!ReducerData.hasDone)
                {
                    for (var i = 1; i <= 9; i = i + 1)
                    {
                        if (playerData.slotItem[$"Slot{i}"]["Element"] != null && playerData.slotItem[$"Slot{i}"]["Quantity"] != null)
                        {
                            addAlert("Alert: The player's hotbar are full!");
                            getReducerQuantity.SetActive(false);
                            return;
                        }
                    }
                }
                ReducerData.hasDone = false;
                playerData.Compound[playerData.Compound.Keys.ElementAt(0)] -= sliderValue;
                getReducer();
            }
        }
    }

    public void Reduce()
    {
        var playerData = PlayerData.Instance();
        if (playerData.Compound.Keys.ElementAt(0) == "H2O")
        {
            playerData.Elements.Add("H", 2);
            playerData.Elements.Add("O", 1);
            playerData.Compound.Clear();
        }
        else if (playerData.Compound.Keys.ElementAt(0) == "HCl")
        {
            playerData.Elements.Add("H", 1);
            playerData.Elements.Add("Cl", 1);
            playerData.Compound.Clear();
        }
        else if (playerData.Compound.Keys.ElementAt(0) == "NH3")
        {
            playerData.Elements.Add("N", 1);
            playerData.Elements.Add("H", 3);
            playerData.Compound.Clear();
        }
        else if (playerData.Compound.Keys.ElementAt(0) == "H2O2")
        {
            playerData.Elements.Add("H", 2);
            playerData.Elements.Add("O", 2);
            playerData.Compound.Clear();
        }
        else if (playerData.Compound.Keys.ElementAt(0) == "NaI")
        {
            playerData.Elements.Add("Na", 1);
            playerData.Elements.Add("I", 1);
            playerData.Compound.Clear();
        }
        else if (playerData.Compound.Keys.ElementAt(0) == "Na2S")
        {
            playerData.Elements.Add("Na", 2);
            playerData.Elements.Add("S", 1);
            playerData.Compound.Clear();
        }
        else if (playerData.Compound.Keys.ElementAt(0) == "KI")
        {
            playerData.Elements.Add("K", 1);
            playerData.Elements.Add("I", 1);
            playerData.Compound.Clear();
        }
        else if (playerData.Compound.Keys.ElementAt(0) == "N2H4")
        {
            playerData.Elements.Add("N", 2);
            playerData.Elements.Add("H", 4);
            playerData.Compound.Clear();
        }
        else if (playerData.Compound.Keys.ElementAt(0) == "AgNO3")
        {
            playerData.Elements.Add("Ag", 1);
            playerData.Elements.Add("N", 1);
            playerData.Elements.Add("O", 3);
            playerData.Compound.Clear();
        }
        else if (playerData.Compound.Keys.ElementAt(0) == "Na3P")
        {
            playerData.Elements.Add("Na", 3);
            playerData.Elements.Add("P", 1);
            playerData.Compound.Clear();
        }
        else if (playerData.Compound.Keys.ElementAt(0) == "NaH")
        {
            playerData.Elements.Add("Na", 1);
            playerData.Elements.Add("H", 1);
            playerData.Compound.Clear();
        }
        else if (playerData.Compound.Keys.ElementAt(0) == "IO3")
        {
            playerData.Elements.Add("I", 1);
            playerData.Elements.Add("O", 3);
            playerData.Compound.Clear();
        }
        else if (playerData.Compound.Keys.ElementAt(0) == "C3H8O")
        {
            playerData.Elements.Add("C", 3);
            playerData.Elements.Add("H", 8);
            playerData.Elements.Add("O", 1);
            playerData.Compound.Clear();
        }
        else if (playerData.Compound.Keys.ElementAt(0) == "HNO3")
        {
            playerData.Elements.Add("H", 1);
            playerData.Elements.Add("N", 1);
            playerData.Elements.Add("O", 3);
            playerData.Compound.Clear();
        }
        else if (playerData.Compound.Keys.ElementAt(0) == "NaClO")
        {
            playerData.Elements.Add("Na", 1);
            playerData.Elements.Add("Cl", 1);
            playerData.Elements.Add("O", 1);
            playerData.Compound.Clear();
        }
        else if (playerData.Compound.Keys.ElementAt(0) == "NaCN")
        {
            playerData.Elements.Add("Na", 1);
            playerData.Elements.Add("C", 1);
            playerData.Elements.Add("N", 1);
            playerData.Compound.Clear();
        }
        else if (playerData.Compound.Keys.ElementAt(0) == "C2H3NaO2")
        {
            playerData.Elements.Add("C", 2);
            playerData.Elements.Add("H", 3);
            playerData.Elements.Add("Na", 1);
            playerData.Elements.Add("O", 2);
            playerData.Compound.Clear();
        }
        else if (playerData.Compound.Keys.ElementAt(0) == "C18H35NaO2")
        {
            playerData.Elements.Add("C", 18);
            playerData.Elements.Add("H", 35);
            playerData.Elements.Add("Na", 1);
            playerData.Elements.Add("O", 2);
            playerData.Compound.Clear();
        }
        print(playerData.Elements.Count);
        for (int i = 1; i <= playerData.Elements.Count; i++)
        {
            GameObject.Find("Branches").GetComponent<RectTransform>().sizeDelta = new Vector2(10 + 95 * (i - 1), GameObject.Find("Branches").GetComponent<RectTransform>().sizeDelta.y);
            GameObject Branch = Instantiate(Resources.Load<GameObject>($"Lab/Branch"));
            Branch.name = $"Branch ({i})";
            Branch.transform.SetParent(GameObject.Find($"Branches").transform);
            GameObject slotImage = Instantiate(Resources.Load<GameObject>($"Lab/reduceSlot"));
            slotImage.name = $"Slot ({i})";
            slotImage.transform.SetParent(GameObject.Find($"Original").transform);
            GameObject.Find($"Original/Slot ({i})/Origin").GetComponent<Text>().text = playerData.Elements.Keys.ElementAt(i - 1);
            GameObject.Find($"Original/Slot ({i})/Originnum").GetComponent<Text>().text = Convert.ToString(playerData.Elements.Values.ElementAt(i - 1));
        }
        reducerCheck();
    }

    private void addReducer()
    {
        var playerData = PlayerData.Instance();
        if (Convert.ToInt32(playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"]) < 1)
        {
            playerData.slotItem[$"Slot{hotbar.slotNum}"]["Element"] = null;
            playerData.slotItem[$"Slot{hotbar.slotNum}"]["Quantity"] = null;
        }
        slotCheck();
        reducerCheck();
    }

    private void getReducer()
    {
        var playerData = PlayerData.Instance();
        if (playerData.Compound.Values.ElementAt(0) < 1)
        {
            playerData.Compound.Remove(playerData.Compound.Keys.ElementAt(0));
        }
        slotCheck();
        reducerCheck();
    }

    private void addAlert(string Alert)
    {
        int maxQuantity = 50;
        player.History.Add(Alert);
        if (player.History.Count > maxQuantity)
        {
            player.History.RemoveAt(0);
        }
    }

    private void slotCheck()
    {
        var playerData = PlayerData.Instance();
        for (var i = 1; i <= 9; i = i + 1)
        {
            if (playerData.slotItem[$"Slot{i}"]["Element"] != null && Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) == 1)
            {
                slotDeepCheck(i);
                GameObject.Find($"HotbarSlot ({i})/ItemNum").GetComponent<Text>().text = "";
            }
            else if (playerData.slotItem[$"Slot{i}"]["Element"] != null && Convert.ToInt32(playerData.slotItem[$"Slot{i}"]["Quantity"]) > 1)
            {
                slotDeepCheck(i);
                GameObject.Find($"HotbarSlot ({i})/ItemNum").GetComponent<Text>().text = playerData.slotItem[$"Slot{i}"]["Quantity"].ToString();
            }
            else if (playerData.slotItem[$"Slot{i}"]["Element"] == null && playerData.slotItem[$"Slot{i}"]["Quantity"] == null)
            {
                Destroy(GameObject.Find($"HotbarSlot ({i})/Item/Image"));
                GameObject.Find($"ItemName").GetComponent<Text>().text = "";
                GameObject.Find($"HotbarSlot ({i})/ItemNum").GetComponent<Text>().text = "";
            }
        }
    }

    private void slotDeepCheck(int i)
    {
        var playerData = PlayerData.Instance();
        if (Chemidex.moleculeRecipes["Symbol"].ContainsKey(Convert.ToString(playerData.slotItem[$"Slot{i}"]["Element"])))
        {
            if (GameObject.Find($"HotbarSlot ({i})/Item/Image") == null)
            {
                GameObject slotImage = Instantiate(Resources.Load<GameObject>($"Lab/H Image"));
                slotImage.name = "Image";
                slotImage.transform.SetParent(GameObject.Find($"HotbarSlot ({i})/Item").transform);
            }
            GameObject.Find($"HotbarSlot ({i})/Item/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Lab/{playerData.slotItem[$"Slot{i}"]["Element"]}");
        }
        else
        {
            Destroy(GameObject.Find($"HotbarSlot ({i})/Item/Image"));
            GameObject.Find($"HotbarSlot ({i})/Symbol").GetComponent<Text>().text = $"{playerData.slotItem[$"Slot{i}"]["Element"]}";
        }
        GameObject.Find("ItemName").GetComponent<Text>().text = Convert.ToString(playerData.slotItem[$"Slot{Convert.ToInt32(hotbar.slotNum)}"]["Element"]);
    }

    private void reducerCheck()
    {
        var playerData = PlayerData.Instance();
        if (playerData.Compound.Count < 1)
        {
            Destroy(GameObject.Find($"Compound/Slot/Item/Image"));
            GameObject.Find($"Compound/Slot/Compoundnum").GetComponent<Text>().text = "";
        }
        else
        {
            if (playerData.Compound.Values.ElementAt(0) > 1)
            {
                deepReducerCheck();
                GameObject.Find($"Compound/Slot/Compoundnum").GetComponent<Text>().text = Convert.ToString(playerData.Compound.Values.ElementAt(0));
            }
            else
            {
                deepReducerCheck();
                GameObject.Find($"Compound/Slot/Compoundnum").GetComponent<Text>().text = "";
            }
        }
    }

    private void deepReducerCheck()
    {
        var playerData = PlayerData.Instance();
        if (GameObject.Find($"Compound/Slot/Item/Image") == null)
        {
            GameObject slotImage = Instantiate(Resources.Load<GameObject>($"Lab/C Image"));
            slotImage.name = "Image";
            slotImage.transform.SetParent(GameObject.Find($"Compound/Slot/Item").transform);
        }
        GameObject.Find($"Compound/Slot/Item/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Lab/{playerData.Compound.Keys.ElementAt(0)}");
    }
}
