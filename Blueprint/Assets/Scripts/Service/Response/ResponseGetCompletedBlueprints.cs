using System.Collections.Generic;
using Service.Request;

namespace Service.Response {
    public class ResponseGetCompletedBlueprints {
        public List<Item> blueprints;
        
        public ResponseGetCompletedBlueprints(int id) {
            blueprints = new List<Item>();
            blueprints.Add(new Item(id));
        }
    }
}