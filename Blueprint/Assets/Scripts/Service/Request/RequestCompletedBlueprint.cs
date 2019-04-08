using System;
using Service.Response;
using System.Collections.Generic;

namespace Service.Request {
    [Serializable]
public class Item {
        public int item_id;
        public Item(int id) {
            item_id = id;
        }
    }

    [Serializable]
    public class RequestCompletedBlueprint {
        public List<Item> blueprints;

        public RequestCompletedBlueprint(int id) {
            blueprints = new List<Item>();
            blueprints.Add(new Item(id));
		}
	}
}
