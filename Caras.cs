using Computadora3D;

public class Cara
{
    public string Nombre { get; set; } = string.Empty;
    public List<Vertice> Vertices { get; set; } = new();
    public float[] Color { get; set; } = new float[] { 1f, 1f, 1f };

}
