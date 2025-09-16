using ALISTAMIENTO_IE.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace ALISTAMIENTO_IE.Services;

public class EtiquetaBusquedaResult
{
    public bool EsValida { get; set; }
    public bool Existe { get; set; }
    public bool ExisteEnKardex { get; set; }
    public string TipoEtiqueta { get; set; } // "ETIQUETA" o "ETIQUETA_LINER"
    public Etiqueta Etiqueta { get; set; }
    public EtiquetaLiner EtiquetaLiner { get; set; }
    public string Mensaje { get; set; }
}

public class EtiquetaService
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["stringConexionLocal"].ConnectionString;
    private readonly string _connectionStringUnoE = ConfigurationManager.ConnectionStrings["stringConexionUnoe"].ConnectionString;

    public EtiquetaService() { }
    public async Task<Etiqueta?> ObtenerEtiquetaPorCodigoAsync(string codigoEtiqueta)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        const string sql = @"SELECT * FROM ETIQUETA WHERE COD_ETIQUETA = @codigo";

        return await connection.QueryFirstOrDefaultAsync<Etiqueta>(sql, new { codigo = codigoEtiqueta });
    }







    public EtiquetaBusquedaResult ValidarEtiquetaEnKardex(string codigoEtiqueta)
    {
        var result = new EtiquetaBusquedaResult();
        result.EsValida = !string.IsNullOrWhiteSpace(codigoEtiqueta) && codigoEtiqueta.Length == 10 && codigoEtiqueta.Any(char.IsLetter);
        if (!result.EsValida)
        {
            result.Mensaje = "La etiqueta debe tener 10 caracteres y al menos una letra.";
            return result;
        }

        using (var connection = new SqlConnection(_connectionString))
        {
            // Buscar en ETIQUETA
            var etiqueta = connection.QueryFirstOrDefault<Etiqueta>(
                "SELECT * FROM ETIQUETA WHERE COD_ETIQUETA = @codigo", new { codigo = codigoEtiqueta });
            // Buscar en ETIQUETA_LINER
            var etiquetaLiner = connection.QueryFirstOrDefault<EtiquetaLiner>(
                "SELECT * FROM ETIQUETA_LINER WHERE COD_ETIQUETA_LINER = @codigo", new { codigo = codigoEtiqueta });

            // Buscar en KARDEX_BODEGA
            var kardex = connection.QueryFirstOrDefault<dynamic>(
                "SELECT * FROM KARDEX_BODEGA WHERE etiqueta = @codigo", new { codigo = codigoEtiqueta });
            result.ExisteEnKardex = kardex != null;

            if ((etiqueta != null || etiquetaLiner != null))
            {
                result.Existe = true;
                result.TipoEtiqueta = etiqueta != null ? "ETIQUETA" : "ETIQUETA_LINER";
                result.Etiqueta = etiqueta;
                result.EtiquetaLiner = etiquetaLiner;
                if (!result.ExisteEnKardex)
                {
                    // Insertar en KARDEX_BODEGA
                    connection.Execute(@"INSERT INTO KARDEX_BODEGA (etiqueta, idBodega, area, fechaIngreso, TipoEntrada) VALUES (@etiqueta, @idBodega, @area, @fechaIngreso, @tipoEntrada)",
                        new
                        {
                            etiqueta = codigoEtiqueta,
                            idBodega = 1,
                            area = "ALISTAMIENTO",
                            fechaIngreso = DateTime.Now,
                            tipoEntrada = "M"
                        });
                    result.Mensaje = "Etiqueta insertada en KARDEX_BODEGA.";
                }
                else
                {
                    result.Mensaje = "Etiqueta ya existe en KARDEX_BODEGA.";
                }
                return result;
            }
            else if (!result.ExisteEnKardex)
            {
                result.Existe = false;
                result.Mensaje = $"N.E – Etiqueta: {codigoEtiqueta}";
                // Aquí se debe frenar la operación y mostrar el error
                return result;
            }
            else
            {
                result.Existe = false;
                result.Mensaje = $"N.E – Etiqueta: {codigoEtiqueta}";
                return result;
            }
        }
    }

    // Async wrappers using Dapper async APIs to avoid bloquear la UI
    public async Task<EtiquetaBusquedaResult> ValidarEtiquetaEnKardexAsync(string codigoEtiqueta)
    {
        var result = new EtiquetaBusquedaResult();
        result.EsValida = !string.IsNullOrWhiteSpace(codigoEtiqueta) && codigoEtiqueta.Length == 10 && codigoEtiqueta.Any(char.IsLetter);
        if (!result.EsValida)
        {
            result.Mensaje = "La etiqueta debe tener 10 caracteres y al menos una letra.";
            return result;
        }

        await using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var etiqueta = await connection.QueryFirstOrDefaultAsync<Etiqueta>(
                "SELECT * FROM ETIQUETA WHERE COD_ETIQUETA = @codigo", new { codigo = codigoEtiqueta });
            var etiquetaLiner = await connection.QueryFirstOrDefaultAsync<EtiquetaLiner>(
                "SELECT * FROM ETIQUETA_LINER WHERE COD_ETIQUETA_LINER = @codigo", new { codigo = codigoEtiqueta });

            var kardex = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SELECT * FROM KARDEX_BODEGA WHERE enBodega = 1 and etiqueta = @codigo", new { codigo = codigoEtiqueta });
            result.ExisteEnKardex = kardex != null;

            if ((etiqueta != null || etiquetaLiner != null))
            {
                result.Existe = true;
                result.TipoEtiqueta = etiqueta != null ? "ETIQUETA" : "ETIQUETA_LINER";
                result.Etiqueta = etiqueta;
                result.EtiquetaLiner = etiquetaLiner;
                if (!result.ExisteEnKardex)
                {
                    await connection.ExecuteAsync(@"INSERT INTO KARDEX_BODEGA (etiqueta, idBodega, area, fechaIngreso, TipoEntrada) VALUES (@etiqueta, @idBodega, @area, @fechaIngreso, @tipoEntrada)",
                        new
                        {
                            etiqueta = codigoEtiqueta,
                            idBodega = 1,
                            area = "ALISTAMIENTO",
                            fechaIngreso = DateTime.Now,
                            tipoEntrada = "M"
                        });
                    result.Mensaje = "Etiqueta insertada en KARDEX_BODEGA.";
                }
                else
                {
                    result.Mensaje = "Etiqueta ya existe en KARDEX_BODEGA.";
                }
                return result;
            }
            else if (!result.ExisteEnKardex)
            {
                result.Existe = false;
                result.Mensaje = $"N.E – Etiqueta: {codigoEtiqueta}";
                return result;
            }
            else
            {
                result.Existe = false;
                result.Mensaje = $"N.E – Etiqueta: {codigoEtiqueta}";
                return result;
            }
        }
    }



}