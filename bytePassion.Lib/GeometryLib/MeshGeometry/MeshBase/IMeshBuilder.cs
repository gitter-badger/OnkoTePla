using bytePassion.Lib.MathLib;


namespace bytePassion.Lib.GeometryLib.MeshGeometry.MeshBase
{

    public interface IMeshBuilder
    {
        Vec3[] GetPositions();
        int[] GetIndecies();
    }
}