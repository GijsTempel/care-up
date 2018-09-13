using UnityEngine;

namespace MBS
{
    [CreateAssetMenu (fileName ="InvLocalData", menuName = "MBS Inventory System Localisation Data", order = 51)]
    public class MBSLocalisationInventory : MBSLocalisationBase
    {
        [Header("Shop")]
        [SerializeField]
        string shop_header = "Shop";

        [SerializeField]
        string
            shop_buy = "Buy",
            shop_sell = "Sell",
            shop_exit = "Leave",
            shop_stock = "Stock",
            shop_available = "Available",
            shop_inventory = "Inventory",
            shop_items = "Items",
            shop_weapons = "Weapons",
            shop_spells = "Spells",
            shop_potions = "Potions",
            shop_collectible = "Collectibles",
            shop_cards = "Cards",
            shop_armour ="Armour",
            shop_scrolls = "Scrolls",
            shop_total = "Total",
            shop_confirm = "Are you sure?",
            shop_greet_hello = "Welcome to {0}",
            shop_greet_goodbye = "Thank you, come again!",
            shop_purchase_ok = "Your transaction was successful",
            shop_purchase_fail_cash = "You cannot afford this",
            shop_purchase_fail_level = "You cannot buy this item yet",
            shop_purchase_fail_class = "That item can only be bought by {0}",
            shop_purchase_fail_race = "That item can only be bought by {0}",
            shop_purchase_fail_guild = "That item can only be bought by members of the {0} guild";

        public string ShopHeader => shop_header;
        public string Shopbuy => shop_buy;
        public string ShopSell => shop_sell;
        public string ShopExit => shop_exit;
        public string ShopStock => shop_stock;
        public string ShopAvailable => shop_available;
        public string ShopInventory => shop_inventory;
        public string ShopItems => shop_items;
        public string ShopWeapons => shop_weapons;
        public string ShopSpells => shop_spells;
        public string ShopPotions => shop_potions;
        public string ShopCollectibles => shop_collectible;
        public string ShopCards => shop_cards;
        public string ShopArmour => shop_armour;
        public string ShopScrolls => shop_scrolls;
        public string ShopTotal => shop_total;
        public string ShopConfirm => shop_confirm;
        public string ShopGreetHallo => shop_greet_hello;
        public string ShopGreetGoodBye => shop_greet_goodbye;
        public string ShopPurchaseOk => shop_purchase_ok;
        public string ShopPurchaseFailCash => shop_purchase_fail_cash;
        public string ShopPurchaseFailClass => shop_purchase_fail_class;
        public string ShopPurchaseFailLevel => shop_purchase_fail_level;
        public string ShopPurchaseFailRace => shop_purchase_fail_race;
        public string ShopPurchaseFailGuild => shop_purchase_fail_guild;


        #region inventory
        [Header( "Inventory" )]
        [SerializeField]
        string inv_equip_ok = "Equipped {0}";
        [SerializeField]
        string
            inv_stock = "You have {0} {1}",
            inv_stock_none = "You don't have any {0}",
            inv_equip = "{0} equipped",
            inv_eqiup_main_hand = "{0} equipped to main hand",
            inv_equip_off_hand = "{0} equipped to off hand",
            inv_equip_fail = "This item can only be used by {0}",
            inv_equip_fail_class = "This item can only be equipped by {0}",
            inv_equip_fail_race = "This item can only be equipped by {0}",
            inv_equip_fail_guild = "This item can only be used by members of the {0} guild",
            inv_equip_fail_level = "This item requires level {0}",
            inv_item_collected = "Collected {0}",
            inv_item_collected_qty = "Collected {0} {1}",
            inv_item_received = "Received {0}",
            inv_item_received_qty = "Received {0} {1}",
            inv_item_used = "Used {0}",
            inv_item_used_qty = "Used {0} {1}",
            inv_item_dropped = "Dropped {0}",
            inv_item_dropped_qty = "Dropped {0} {1}",
            inv_item_pickup = "Picked up {0}",
            inv_item_pickup_qty = "Picked up {0} {1}",
            inv_item_stole = "You stole {0}",
            inv_item_stole_qty = "You stole {0} {1}",
            inv_item_stolen = "{0} was stolen from you",
            inv_item_stolen_qty = "{0} {1} was stolen from you",
            inv_item_destroyed = "{0} was destroyed";
        #endregion

        #region crafting
        [Header( "Crafting" )]
        [SerializeField]
        string inv_merge_confirm = "Do you really want to merge these items?";
        [SerializeField]
        string
            inv_merge_confirm_items = "Do you really want to merge {0} and {1}?",
            inv_merge_confirm_items_qty = "Do you really want to merge {0} {1} and {2} {3}?",
            inv_merge_success = "The merge was successful",
            inv_merge_success_items = "You successfully merged {0} and {1}";

        public string InvEquipOk => inv_equip_ok;
        public string InvEquip => inv_equip;
        public string InvEquipMain => inv_eqiup_main_hand;
        public string InvEquipAlt => inv_equip_off_hand;
        public string InvEquipFail => inv_equip_fail;
        public string InvEquipFailClass => inv_equip_fail_class;
        public string InvEquipFailLevel => inv_equip_fail_level;
        public string InvEquipFailRace => inv_equip_fail_race;
        public string InvEquipFailGuild => inv_equip_fail_guild;
        public string InvStock => inv_stock;
        public string InvStockNone => inv_stock_none;
        public string InvItemCollected => inv_item_collected;
        public string InvItemCollectedQty => inv_item_collected_qty;
        public string InvItemReceived => inv_item_received;
        public string InvItemReceivedQty => inv_item_received_qty;
        public string InvItemUsed => inv_item_used;
        public string InvItemUsedQty => inv_item_used_qty;
        public string InvItemDropped => inv_item_dropped;
        public string InvItemDroppedQty => inv_item_dropped_qty;
        public string InvItemPickup => inv_item_pickup;
        public string InvItemPickupQty => inv_item_pickup_qty;
        public string InvItemStole => inv_item_stole;
        public string InvItemStoleQty => inv_item_stole_qty;
        public string InvItemStolen => inv_item_stolen;
        public string InvItemStolenQty => inv_item_stolen_qty;
        public string InvItemDestroyed => inv_item_destroyed;
        public string InvMergeConfirm => inv_merge_confirm;
        public string InvMergeConfirmItems => inv_merge_confirm_items;
        public string InvMergeConfirmItemsQty => inv_merge_confirm_items_qty;
        public string InvMergeSuccess => inv_merge_success;
        public string InvMergeSuccessItems => inv_merge_success_items;

        #endregion
    }

}