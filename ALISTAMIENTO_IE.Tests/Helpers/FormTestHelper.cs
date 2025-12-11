using System.ComponentModel;
using System.Reflection;

namespace ALISTAMIENTO_IE.Tests.Helpers;

/// <summary>
/// Utilidades para facilitar el testing de formularios WinForms
/// </summary>
public static class FormTestHelper
{
    /// <summary>
    /// Busca un control en el formulario por su nombre (búsqueda recursiva)
    /// </summary>
    /// <typeparam name="T">Tipo del control a buscar</typeparam>
    /// <param name="form">Formulario donde buscar</param>
    /// <param name="controlName">Nombre del control (propiedad Name)</param>
    /// <returns>El control encontrado o null si no existe</returns>
    public static T? FindControl<T>(Form form, string controlName) where T : Control
    {
        return form.Controls.Find(controlName, searchAllChildren: true).FirstOrDefault() as T;
    }

    /// <summary>
    /// Verifica si un botón existe en el formulario
    /// </summary>
    public static bool ButtonExists(Form form, string buttonName)
    {
        return FindControl<Button>(form, buttonName) != null;
    }

    /// <summary>
    /// Verifica si un TextBox existe en el formulario
    /// </summary>
    public static bool TextBoxExists(Form form, string textBoxName)
    {
        return FindControl<TextBox>(form, textBoxName) != null;
    }

    /// <summary>
    /// Obtiene todos los botones del formulario
    /// </summary>
    public static IEnumerable<Button> GetAllButtons(Form form)
    {
        return GetAllControls<Button>(form);
    }

    /// <summary>
    /// Obtiene todos los controles de un tipo específico
    /// </summary>
    private static IEnumerable<T> GetAllControls<T>(Control parent) where T : Control
    {
        foreach (Control control in parent.Controls)
        {
            if (control is T typedControl)
            {
                yield return typedControl;
            }

            foreach (var child in GetAllControls<T>(control))
            {
                yield return child;
            }
        }
    }

    /// <summary>
    /// Verifica si un evento está asociado a un control (usando reflexión)
    /// ADVERTENCIA: Este método usa reflexión y puede no funcionar con todos los tipos de eventos
    /// </summary>
    public static bool HasEventHandler(Control control, string eventName)
    {
        try
        {
            var eventsField = typeof(Component).GetField(
                "events",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (eventsField == null) return false;

            var eventHandlerList = eventsField.GetValue(control) as System.ComponentModel.EventHandlerList;
            if (eventHandlerList == null) return false;

            // Obtener la clave del evento
            var eventKey = GetEventKey(control.GetType(), eventName);
            if (eventKey == null) return false;

            // Verificar si hay handlers asociados
            var eventDelegate = eventHandlerList[eventKey] as Delegate;
            return eventDelegate != null && eventDelegate.GetInvocationList().Length > 0;
        }
        catch
        {
            // Si falla la reflexión, asumimos que no podemos verificar
            return false;
        }
    }

    private static object? GetEventKey(Type controlType, string eventName)
    {
        var currentType = controlType;
        while (currentType != null)
        {
            var eventInfo = currentType.GetEvent(
                eventName,
                BindingFlags.Public | BindingFlags.Instance);

            if (eventInfo != null)
            {
                var eventKeyField = currentType.GetField(
                    $"Event{eventName}",
                    BindingFlags.NonPublic | BindingFlags.Static);

                if (eventKeyField != null)
                    return eventKeyField.GetValue(null);
            }

            currentType = currentType.BaseType;
        }

        return null;
    }

    /// <summary>
    /// Simula escribir texto en un TextBox
    /// </summary>
    public static void SimulateTextInput(TextBox textBox, string text)
    {
        textBox.Text = text;
    }

    /// <summary>
    /// Simula un click en un botón
    /// </summary>
    public static void SimulateButtonClick(Button button)
    {
        button.PerformClick();
    }
}
