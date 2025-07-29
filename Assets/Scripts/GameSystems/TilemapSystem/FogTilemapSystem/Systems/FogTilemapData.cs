namespace GameSystems.TilemapSystem.FogTilemap
{
    public enum FogState { None, Hidden, Revealed, Visible }

    public class FogTilemapData
    {
        private FogState[,] fogStates;

        public void InitialSetting(int width, int height)
        {
            this.fogStates = new FogState[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    this.fogStates[x, y] = FogState.Hidden;
                }
            }
        }

        public (int, int) GetLength()
        {
            return (this.fogStates.GetLength(0), this.fogStates.GetLength(1));
        }

        public bool TryGetFogState(UnityEngine.Vector2Int girdPosition, out FogState FogState)
        {
            return TryGetFogState(girdPosition.x, girdPosition.y, out FogState);
        }
        public bool TryGetFogState(int x, int y, out FogState FogState)
        {
            FogState = FogState.None;

            var length = this.GetLength();
            if (x < 0 || length.Item1 <= x || y < 0 || length.Item2 <= y) return false;

            FogState = this.fogStates[x, y];
            return true;
        }
        public void SetFogState(int x, int y, FogState fogState)
        {
            var length = this.GetLength();
            if (x < 0 || length.Item1 <= x || y < 0 || length.Item2 <= y) return;

            this.fogStates[x, y] = fogState;
        }

        public void ClearFogTilemapData()
        {
            this.fogStates = null;
        }
    }
}
