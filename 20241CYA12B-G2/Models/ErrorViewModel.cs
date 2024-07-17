namespace _20241CYA12B_G2.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public const string CampoRequerido = "{0} es un campo obligatorio.";
        public const string CaracteresMinimos = "{0} debe tener al menos {1} caracteres.";
        public const string CaracteresMaximos = "{0} debe tener menos de {1} caracteres.";
        public const string ValorMinMax = "{0} debe tener un valor entre {1} y {2}.";
    }
}
