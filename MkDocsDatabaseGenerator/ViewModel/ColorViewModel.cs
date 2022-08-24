using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkDocsDatabaseGenerator.ViewModel
{
    public class ColorViewModel : BaseViewModel
    {
        #region Property
        #region Property : Name
        private string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                OnPropertyChanged(nameof(this.Name));
            }
        }
        #endregion
        #region Property : Color
        private Color color;
        public Color Color
        {
            get { return this.color; }
            set
            {
                this.color = value;
                OnPropertyChanged(nameof(this.Color));
            }
        }
        #endregion

        #endregion

    }
}
