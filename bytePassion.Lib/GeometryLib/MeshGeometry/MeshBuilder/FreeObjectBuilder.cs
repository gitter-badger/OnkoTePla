using bytePassion.Lib.GeometryLib.MeshGeometry.MeshBase;
using bytePassion.Lib.GeometryLib.MeshGeometry.Objects;
using bytePassion.Lib.MathLib;


namespace bytePassion.Lib.GeometryLib.MeshGeometry.MeshBuilder
{

    internal class FreeObjectBuilder : IMeshBuilder
    {
        private readonly FreeObject freeObject;

        public FreeObjectBuilder(FreeObject freeObject)
        {
            this.freeObject = freeObject;
        }
        
        public Vec3[] GetPositions() => freeObject.PositionsObjectSpace;
        public int[]  GetIndecies()  => freeObject.Indecies;
    }

}
