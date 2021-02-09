using System;
using System.Collections.Generic;
using System.Text;

namespace DizuAutomation.Core
{

    public class Settings
    {
        public Dizu Dizu { get; set; }
        public Dizu GanharNoInsta { get; set; }

    }

    public class Dizu
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }

        public DizuConta[] Contas { get; set; }
    }

    public class DizuConta
    {
        public string Tipo { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string[] Codigos_Seguranca { get; set; }
    }



}
