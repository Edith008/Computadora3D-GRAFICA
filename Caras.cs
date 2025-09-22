using Computadora3D;

public class Cara
{
    public string Nombre { get; set; } = string.Empty;
    public List<double[]> VerticesArray { get; set; } = new();
    public List<Vertice> Vertices { get; set; } = new();
    public float[] Color { get; set; } = new float[] { 1f, 1f, 1f };

    public void InicializarVertices()
    {
        //VerticeClear();
        foreach (var v in VerticesArray)
        {
            if (v.Length >= 3)
                Vertices.Add(new Vertice(v[0], v[1], v[2]));
        }
    }
}
