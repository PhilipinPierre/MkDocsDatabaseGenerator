using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkDocsDatabaseGenerator.ViewModel.Interface
{
    public interface IRenderingViewModelCaller
    {
        public string PlantUmlPath { get; }
        public string GraphvizDotPath { get; }
        public string JavaPath { get; }
        public int NbOperation { get;  set; }

        public string SubOperation { set; }
    }
}
