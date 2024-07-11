namespace Chipin_Rewrite.Models.ViewModel
{
    public class ListViewModel
    {
        public string ListName { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string ExistingListName { get; set; }
        public List<string> ExistingLists { get; set; } = new List<string>();
    }
}
