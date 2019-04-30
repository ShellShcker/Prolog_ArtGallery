using System;
using ArtGallery_Prolog.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery_Prolog.ViewModels
{
    public class ComboBoxViewModel
    {
        List<ComboBoxPairsModel> comboBoxPair = new List<ComboBoxPairsModel>();

        public List<ComboBoxPairsModel> ArtistComboBox()
        {
            comboBoxPair.Add(new ComboBoxPairsModel("Frida Kahlo", "fridaKahlo"));
            comboBoxPair.Add(new ComboBoxPairsModel("Diego Rivera", "diegoRivera"));

            return comboBoxPair;
        }
    }
}
