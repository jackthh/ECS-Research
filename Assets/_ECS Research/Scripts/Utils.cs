using Unity.Mathematics;


namespace _ECS_Research.Scripts
{
    public static class Utils
    {
        /// <summary>
        /// Random pos in UI coordinate 
        /// </summary>
        public static float3 GetRandomPosition(ref Random _rdm, float2 _xBounds, float2 _yBounds, float _depth = 0f)
        {
            return new float3(_rdm.NextFloat(_xBounds.x, _xBounds.y), _rdm.NextFloat(_yBounds.x, _yBounds.y), _depth);
        }


        /// <summary>
        /// Random pos in platform coordinate with offset condition 
        /// </summary>
        public static float3 GetRandomPosition(ref Random _rdm, float2 _offsetRange, float _height)
        {
            var rawRand = _rdm.NextFloat2(-_offsetRange.y, _offsetRange.y);
            var rawLength = math.length(rawRand);

            while (rawLength < _offsetRange.x || rawLength > _offsetRange.y)
            {
                rawRand = _rdm.NextFloat2(-_offsetRange.y, _offsetRange.y);
            }

            var result = new float3(rawRand.x, _height, rawRand.y);
            return result;
        }
    }
}