using System;
using Service.Response;
using System.Collections.Generic;

namespace Service.Request {
public class Item {
        int item_id;
        public Item(int id) {
            item_id = id;
        }
    }

    public class PayloadLeaderboard {
        List<Item> blueprints;

        public PayloadLeaderboard(int id) {
            blueprints = new List<Item>();
            blueprints.Add(new Item(id));
		}
	}
}
