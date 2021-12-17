using EstebanMartinezExamenADO.Context;
using EstebanMartinezExamenADO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoADO
{
    public partial class FormPractica : Form
    {
        ClientesPedidosContext context;
        List<Cliente> clientes;
        List<Pedido> pedidos;

        public FormPractica()
        {
            InitializeComponent();
            this.context = new ClientesPedidosContext();
            this.clientes = new List<Cliente>();
            this.pedidos = new List<Pedido>();
            this.CargarClientes();
        }

        private void CargarClientes()
        {
            this.cmbclientes.Items.Clear();
            this.clientes.Clear();
            this.clientes = this.context.GetClientes();

            foreach(Cliente c in this.clientes)
            {
                this.cmbclientes.Items.Add(c.Empresa);
            }
            
        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cliente seleccionado = this.clientes[this.cmbclientes.SelectedIndex];
            this.cmbclientes.Tag = seleccionado.CodigoCliente;
            this.txtempresa.Text = seleccionado.Empresa;
            this.txtcontacto.Text = seleccionado.Contacto;
            this.txtcargo.Text = seleccionado.Cargo;
            this.txtciudad.Text = seleccionado.Ciudad;
            this.txttelefono.Text = seleccionado.Telefono.ToString();

            this.CargarPedidos();
        }

        private void CargarPedidos()
        {
            Cliente seleccionado = this.clientes[this.cmbclientes.SelectedIndex];
            this.lstpedidos.Items.Clear();
            this.pedidos.Clear();
            pedidos = this.context.GetPedidosCliente(seleccionado.CodigoCliente);
            foreach (Pedido p in pedidos)
            {
                this.lstpedidos.Items.Add(p.CodigoPedido);
            }
        }

        private void btnmodificarcliente_Click(object sender, EventArgs e)
        {
            Cliente modif = new Cliente();
            modif.CodigoCliente = this.cmbclientes.Tag.ToString();
            modif.Empresa = this.txtempresa.Text;
            modif.Contacto = this.txtcontacto.Text;
            modif.Cargo = this.txtcargo.Text;
            modif.Ciudad = this.txtciudad.Text;
            modif.Telefono = int.Parse(this.txttelefono.Text);

            int modificados = this.context.ModificarCliente(modif);
            MessageBox.Show("Se ha modificado " + modificados + " registro");
            this.CargarClientes();
            this.VaciarCajasCliente();
            this.VaciarCajasPedidos();
            this.lstpedidos.Items.Clear();
        }

        private void VaciarCajasCliente()
        {
            this.txtempresa.Text = "";
            this.txtcontacto.Text = "";
            this.txtcargo.Text = "";
            this.txtciudad.Text = "";
            this.txttelefono.Text = "";
            this.cmbclientes.Tag = "";

        }
        private void VaciarCajasPedidos()
        {
            
            this.txtcodigopedido.Text = "";
            this.txtfechaentrega.Text = "";
            this.txtformaenvio.Text = "";
            this.txtimporte.Text = "";

        }


        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstpedidos.SelectedItems.Count != 0)
            {
                Pedido seleccionado = this.pedidos[this.lstpedidos.SelectedIndex];
                this.txtcodigopedido.Text = seleccionado.CodigoPedido;
                this.txtfechaentrega.Text = seleccionado.FechaEntrega;
                this.txtformaenvio.Text = seleccionado.FormaEnvio;
                this.txtimporte.Text = seleccionado.Importe.ToString();
            }
            
        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {
            Pedido p = new Pedido();
            p.CodigoPedido = this.txtcodigopedido.Text;
            p.CodigoCliente = this.cmbclientes.Tag.ToString();
            p.FechaEntrega = this.txtfechaentrega.Text;
            p.FormaEnvio = this.txtformaenvio.Text;
            p.Importe = int.Parse(this.txtimporte.Text);
            int insertados = this.context.InsertarPedido(p);
            MessageBox.Show("Se ha insertado " + insertados);
            this.VaciarCajasPedidos();
            this.CargarPedidos();
        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            if (this.lstpedidos.SelectedItems.Count != 0)
            {
                int eliminados = this.context.DeletePedido(this.lstpedidos.SelectedItem.ToString(),this.cmbclientes.Tag.ToString());
                MessageBox.Show("Se han eliminado " + eliminados + " registros");
                this.CargarPedidos();
                this.VaciarCajasPedidos();
            } else
            {
                MessageBox.Show("Debe seleccionar un pedido para eliminarlo");
            }
        }

        private void btnInsertarCliente_Click(object sender, EventArgs e)
        {
            Cliente cliente = new Cliente();            
            cliente.Empresa = this.txtempresa.Text;
            cliente.Contacto = this.txtcontacto.Text;
            cliente.Cargo = this.txtcargo.Text;
            cliente.Ciudad = this.txtciudad.Text;
            cliente.Telefono = int.Parse(this.txttelefono.Text);

            int insertados = this.context.InsertarCliente(cliente);
            MessageBox.Show("Se ha insertado " + insertados);
            this.VaciarCajasCliente();
            this.CargarClientes();
        }
    }
}
