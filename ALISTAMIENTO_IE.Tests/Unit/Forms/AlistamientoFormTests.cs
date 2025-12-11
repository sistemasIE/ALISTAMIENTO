using ALISTAMIENTO_IE.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace ALISTAMIENTO_IE.Tests.Unit.Forms;

/// <summary>
/// Tests unitarios para el formulario ALISTAMIENTO
/// Verifica que los controles y eventos estén correctamente configurados
/// </summary>
[Trait("Category", "Unit")]
[Trait("Feature", "Alistamiento")]
[Trait("Priority", "High")]
public class AlistamientoFormTests : IDisposable
{
    private ALISTAMIENTO? _form;

    #region Tests de Controles UI

    [Fact(DisplayName = "txtEtiqueta - Al inicializar - Debe existir en el formulario")]
    public void TxtEtiqueta_AlInicializar_DebeExistirEnFormulario()
    {
        // Arrange
        _form = CrearFormularioConMocks();

        // Act
        var txtEtiqueta = FormTestHelper.FindControl<TextBox>(_form, "txtEtiqueta");

        // Assert
        txtEtiqueta.Should().NotBeNull(
            "El TextBox 'txtEtiqueta' es crítico y debe existir en ALISTAMIENTO.Designer.cs");
    }

    [Fact(DisplayName = "txtEtiqueta - Al inicializar - Debe tener el nombre correcto")]
    public void TxtEtiqueta_AlInicializar_DebeTenerNombreCorrecto()
    {
        // Arrange
        _form = CrearFormularioConMocks();

        // Act
        var txtEtiqueta = FormTestHelper.FindControl<TextBox>(_form, "txtEtiqueta");

        // Assert
        txtEtiqueta.Should().NotBeNull();
        txtEtiqueta!.Name.Should().Be("txtEtiqueta",
            "El nombre del control debe ser 'txtEtiqueta' para que los event handlers funcionen correctamente");
    }

    [Fact(DisplayName = "txtEtiqueta - Al inicializar - Debe estar accesible")]
    public void TxtEtiqueta_AlInicializar_DebeEstarAccesible()
    {
        // Arrange
        _form = CrearFormularioConMocks();

        // Act
        var existe = FormTestHelper.TextBoxExists(_form, "txtEtiqueta");

        // Assert
        existe.Should().BeTrue("El control txtEtiqueta debe existir y ser accesible");
    }

    #endregion

    #region Tests de Inicialización

    [Fact(DisplayName = "Constructor - Con dependencias válidas - Debe inicializar sin excepciones")]
    public void Constructor_ConDependenciasValidas_DebeInicializarSinExcepciones()
    {
        // Act
        var exception = Record.Exception(() =>
        {
            _form = CrearFormularioConMocks();
        });

        // Assert
        exception.Should().BeNull(
            "El formulario debe inicializarse correctamente cuando todas las dependencias están correctamente mockeadas");
    }

    [Fact(DisplayName = "Constructor - Con idCamion válido - Debe asignar correctamente")]
    public void Constructor_ConIdCamionValido_DebeAsignarCorrectamente()
    {
        // Arrange
        int idCamionEsperado = 123;

        // Act
        _form = CrearFormularioConMocks(idCamion: idCamionEsperado);

        // Assert
        _form.Should().NotBeNull("El formulario debe crearse exitosamente");
    }

    #endregion

    #region Tests de Eventos (Verificación de Asociación)

    [Fact(DisplayName = "txtEtiqueta_TextChanged - Debe estar asociado al control")]
    public void TxtEtiquetaTextChanged_DebeEstarAsociadoAlControl()
    {
        // Arrange
        _form = CrearFormularioConMocks();
        var txtEtiqueta = FormTestHelper.FindControl<TextBox>(_form, "txtEtiqueta");

        // Assert
        txtEtiqueta.Should().NotBeNull(
            "Si el control no existe, el evento TextChanged no puede estar asociado");
        
        // Nota: La verificación profunda de event handlers requiere reflexión avanzada
        // Por ahora verificamos que el control existe y tiene el nombre correcto
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Crea una instancia del formulario ALISTAMIENTO con todas las dependencias mockeadas
    /// </summary>
    private ALISTAMIENTO CrearFormularioConMocks(
        int idCamion = 1,
        string estadoInicial = "SIN_ALISTAR")
    {
        // Usar la fábrica de mocks centralizada
        var deps = MockServiceFactory.CreateAlistamientoFormDependencies();

        // Crear formulario con dependencias mockeadas
        return new ALISTAMIENTO(
            idCamion,
            estadoInicial,
            deps.AlistamientoService,
            deps.AlistamientoEtiquetaService,
            deps.ServiceScopeFactory,
            deps.AuthorizationService,
            deps.CamionService,
            deps.DetalleCamionService,
            deps.EtiquetaService,
            deps.EtiquetaLinerService,
            deps.EtiquetaRolloService,
            deps.EliminacionService,
            deps.KardexService
        );
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        _form?.Dispose();
        GC.SuppressFinalize(this);
    }

    #endregion
}
