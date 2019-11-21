namespace ForestryTest
{
    public class TestNode
    {
        public string Id;
        public string ParentId;
        public TestNode(string parentId, string id)
        {
            Id = id;
            ParentId = parentId;
        }
    }
}
