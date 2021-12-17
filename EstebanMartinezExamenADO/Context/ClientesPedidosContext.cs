using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using EstebanMartinezExamenADO.Models;
using System.Data;

#region PROCEDIMIENTOS ALMACENADOS
/*
 PROCEDIMIENTO PARA MODIFICAR

create procedure modificarcliente
(@CODCLIENTE nvarchar(MAX), @EMPRESA nvarchar(MAX), @CONTACTO nvarchar(MAX), @CARGO nvarchar(MAX), @CIUDAD nvarchar(MAX), @TELEFONO int)
as
	update clientes set Empresa = @EMPRESA, Contacto = @CONTACTO, Cargo = @CARGO, Ciudad = @CIUDAD, Telefono = @TELEFONO where CodigoCliente = @CODCLIENTE	
go



PROCEDIMIENTO PARA OBTENER LOS PEDIDOS A TRAVES DEL CODIGO DE UN CLIENTE

create procedure getpedidoscliente
(@CODCLIENTE nvarchar(MAX))
as
	select * from pedidos where CodigoCliente = @CODCLIENTE
go




PROCEDIMIENTO PARA INSERTAR CLIENTE

create procedure insertarclientes
(@EMPRESA nvarchar(MAX), @CONTACTO nvarchar(MAX), @CARGO nvarchar(MAX), @CIUDAD nvarchar(MAX), @TELEFONO int)
as
	declare @CODCLIENTE nvarchar(MAX)
	set @CODCLIENTE = LEFT(@EMPRESA,3)

	insert into clientes values (@CODCLIENTE,@EMPRESA,@CONTACTO,@CARGO,@CIUDAD,@TELEFONO)
go

 */
#endregion

namespace EstebanMartinezExamenADO.Context
{ 
    public class ClientesPedidosContext
    {
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;

        public ClientesPedidosContext()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("config.json");
            IConfigurationRoot root = builder.Build();

            this.cn = new SqlConnection(root["cadenaPedidosClientes"]);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;               
        }

        public List<Cliente> GetClientes()
        {
            List<Cliente> clientes = new List<Cliente>();
            this.com.CommandText = "select * from clientes";
            this.com.CommandType = CommandType.Text;

            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            while (this.reader.Read())
            {
                Cliente c = new Cliente();
                c.CodigoCliente = this.reader["CodigoCliente"].ToString();            
                c.Empresa = this.reader["Empresa"].ToString();
                c.Contacto = this.reader["Contacto"].ToString();
                c.Cargo = this.reader["Cargo"].ToString();
                c.Ciudad = this.reader["Ciudad"].ToString();
                c.Telefono = int.Parse(this.reader["Telefono"].ToString());
                clientes.Add(c);
            }

            this.reader.Close();
            this.cn.Close();
            
            return clientes;
        }

        public int ModificarCliente(Cliente c)
        {
            this.com.Parameters.AddWithValue("@CODCLIENTE",c.CodigoCliente);
            this.com.Parameters.AddWithValue("@EMPRESA",c.Empresa);
            this.com.Parameters.AddWithValue("@CONTACTO", c.Contacto);
            this.com.Parameters.AddWithValue("@CARGO",c.Cargo);
            this.com.Parameters.AddWithValue("@CIUDAD",c.Ciudad);
            this.com.Parameters.AddWithValue("@TELEFONO",c.Telefono);            
            this.com.CommandText = "modificarcliente";
            this.com.CommandType = CommandType.StoredProcedure;
            this.cn.Open();

            int modificados = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();

            return modificados;
        }

        public List<Pedido> GetPedidosCliente(String codCliente)
        {
            List<Pedido> pedidos = new List<Pedido>();
            this.com.Parameters.AddWithValue("@CODCLIENTE", codCliente);
            this.com.CommandText = "getpedidoscliente";
            this.com.CommandType = CommandType.StoredProcedure;
                        
            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            while(this.reader.Read())
            {
                Pedido pedido = new Pedido();
                pedido.CodigoPedido = this.reader["CodigoPedido"].ToString();
                pedido.CodigoCliente = this.reader["CodigoCliente"].ToString();
                pedido.FechaEntrega = this.reader["FechaEntrega"].ToString();
                pedido.FormaEnvio = this.reader["FormaEnvio"].ToString();
                pedido.Importe = int.Parse(this.reader["Importe"].ToString());
                pedidos.Add(pedido);
            }

            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();

            return pedidos;
        }

        public int InsertarPedido(Pedido p ) 
        {
            this.com.CommandText = "INSERT INTO PEDIDOS VALUES(@CODPEDIDO,@CODCLIENTE,@FECHAENTREGA,@FORMAENVIO,@IMPORTE)";
            this.com.Parameters.AddWithValue("@CODPEDIDO",p.CodigoPedido);
            this.com.Parameters.AddWithValue("@CODCLIENTE",p.CodigoCliente);
            this.com.Parameters.AddWithValue("@FECHAENTREGA", p.FechaEntrega);
            this.com.Parameters.AddWithValue("@FORMAENVIO",p.FormaEnvio);
            this.com.Parameters.AddWithValue("@IMPORTE",p.Importe);
            this.com.CommandType = CommandType.Text;
            
            this.cn.Open();

            int insertados = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();

            return insertados;
        }

        public int DeletePedido(String codPedido, String codCliente)
        {
            this.com.Parameters.AddWithValue("@CODPEDIDO",codPedido);
            this.com.Parameters.AddWithValue("@CODCLIENTE",codCliente);
            this.com.CommandText = "DELETE FROM PEDIDOS WHERE CodigoPedido = @CODPEDIDO and CodigoCliente = @CODCLIENTE";
            this.com.CommandType = CommandType.Text;

            this.cn.Open();

            int eliminados = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();

            return eliminados;
        }

        public int InsertarCliente(Cliente c)
        {
            this.com.CommandText = "insertarclientes";
            this.com.Parameters.AddWithValue("@EMPRESA",c.Empresa);
            this.com.Parameters.AddWithValue("@CONTACTO",c.Contacto);
            this.com.Parameters.AddWithValue("@CARGO",c.Cargo);
            this.com.Parameters.AddWithValue("@CIUDAD", c.Ciudad);
            this.com.Parameters.AddWithValue("@TELEFONO", c.Telefono);
            this.com.CommandType = CommandType.StoredProcedure;

            this.cn.Open();

            int insertados = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();

            return insertados;
        }
    }
}
