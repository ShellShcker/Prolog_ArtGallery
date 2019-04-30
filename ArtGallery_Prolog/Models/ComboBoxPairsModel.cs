
namespace ArtGallery_Prolog.Models
{
    public class ComboBoxPairsModel
    {
        public string _Key { get; set; }
        public string _Value { get; set; }

        public ComboBoxPairsModel(string _key, string _value)
        {
            _Key = _key;
            _Value = _value;
        }
    }
}
