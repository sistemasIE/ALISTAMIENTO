using ALISTAMIENTO_IE.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ALISTAMIENTO_IE.Tests.Helpers;

/// <summary>
/// Fábrica de mocks para servicios comúnmente usados en tests
/// Centraliza la creación de mocks para evitar duplicación de código
/// </summary>
public static class MockServiceFactory
{
    /// <summary>
    /// Crea un mock básico de IAlistamientoService
    /// </summary>
    public static IAlistamientoService CreateAlistamientoService()
    {
        return Mock.Of<IAlistamientoService>();
    }

    /// <summary>
    /// Crea un mock de IEtiquetaService
    /// </summary>
    public static IEtiquetaService CreateEtiquetaService()
    {
        return Mock.Of<IEtiquetaService>();
    }

    /// <summary>
    /// Crea un mock de IEtiquetaLinerService
    /// </summary>
    public static IEtiquetaLinerService CreateEtiquetaLinerService()
    {
        return Mock.Of<IEtiquetaLinerService>();
    }

    /// <summary>
    /// Crea un mock de IEtiquetaRolloService
    /// </summary>
    public static IEtiquetaRolloService CreateEtiquetaRolloService()
    {
        return Mock.Of<IEtiquetaRolloService>();
    }

    /// <summary>
    /// Crea un mock de IAlistamientoEtiquetaService
    /// </summary>
    public static IAlistamientoEtiquetaService CreateAlistamientoEtiquetaService()
    {
        return Mock.Of<IAlistamientoEtiquetaService>();
    }

    /// <summary>
    /// Crea un mock de IAuthorizationService
    /// </summary>
    public static IAuthorizationService CreateAuthorizationService()
    {
        return Mock.Of<IAuthorizationService>();
    }

    /// <summary>
    /// Crea un mock de ICamionService
    /// </summary>
    public static ICamionService CreateCamionService()
    {
        return Mock.Of<ICamionService>();
    }

    /// <summary>
    /// Crea un mock de IDetalleCamionXDiaService
    /// </summary>
    public static IDetalleCamionXDiaService CreateDetalleCamionXDiaService()
    {
        return Mock.Of<IDetalleCamionXDiaService>();
    }

    /// <summary>
    /// Crea un mock de IEliminacionAlistamientoEtiquetaService
    /// </summary>
    public static IEliminacionAlistamientoEtiquetaService CreateEliminacionService()
    {
        return Mock.Of<IEliminacionAlistamientoEtiquetaService>();
    }

    /// <summary>
    /// Crea un mock de IKardexService
    /// </summary>
    public static IKardexService CreateKardexService()
    {
        return Mock.Of<IKardexService>();
    }

    /// <summary>
    /// Crea un mock de IServiceScopeFactory
    /// </summary>
    public static IServiceScopeFactory CreateServiceScopeFactory()
    {
        return Mock.Of<IServiceScopeFactory>();
    }

    /// <summary>
    /// Crea todos los mocks necesarios para el formulario ALISTAMIENTO
    /// </summary>
    public static AlistamientoFormDependencies CreateAlistamientoFormDependencies()
    {
        return new AlistamientoFormDependencies
        {
            EtiquetaLinerService = CreateEtiquetaLinerService(),
            EtiquetaRolloService = CreateEtiquetaRolloService(),
            EtiquetaService = CreateEtiquetaService(),
            AlistamientoEtiquetaService = CreateAlistamientoEtiquetaService(),
            AlistamientoService = CreateAlistamientoService(),
            AuthorizationService = CreateAuthorizationService(),
            CamionService = CreateCamionService(),
            DetalleCamionService = CreateDetalleCamionXDiaService(),
            EliminacionService = CreateEliminacionService(),
            KardexService = CreateKardexService(),
            ServiceScopeFactory = CreateServiceScopeFactory()
        };
    }
}

/// <summary>
/// Contenedor de todas las dependencias mockeadas para el formulario ALISTAMIENTO
/// </summary>
public class AlistamientoFormDependencies
{
    public required IEtiquetaLinerService EtiquetaLinerService { get; init; }
    public required IEtiquetaRolloService EtiquetaRolloService { get; init; }
    public required IEtiquetaService EtiquetaService { get; init; }
    public required IAlistamientoEtiquetaService AlistamientoEtiquetaService { get; init; }
    public required IAlistamientoService AlistamientoService { get; init; }
    public required IAuthorizationService AuthorizationService { get; init; }
    public required ICamionService CamionService { get; init; }
    public required IDetalleCamionXDiaService DetalleCamionService { get; init; }
    public required IEliminacionAlistamientoEtiquetaService EliminacionService { get; init; }
    public required IKardexService KardexService { get; init; }
    public required IServiceScopeFactory ServiceScopeFactory { get; init; }
}
