using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstebanMartinezExamenADO.Models
{
    public class Pedido
    {
        public String CodigoPedido { get; set; }
        public String CodigoCliente { get; set; }
        public String FechaEntrega { get; set; }
        public String FormaEnvio { get; set; }
        public int Importe { get; set; }
    }
}
