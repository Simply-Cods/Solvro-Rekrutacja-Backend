namespace Solvro_Backend.Logic
{
    public static class Fibonacci
    {
        private static Dictionary<int, int> _cache = new();

        public static int GetFibo(int n)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException(nameof(n), "Term must be a non-negative intiger");

            if(n < 2)
                return n;

            if(_cache.TryGetValue(n, out int output))
                return output;

            output = GetFibo(n - 1) + GetFibo(n - 2);
            _cache.Add(n, output);
            return output;
        }

        public static bool IsFibo(int x)
        {
            if (x < 0)
                throw new ArgumentOutOfRangeException(nameof(x), "Number be a non-negative intiger");

            if (x < 2) return true;
            if(_cache.ContainsValue(x)) return true;

            int testNumber = 0;
            int nextFibo = 3;
            while (x > testNumber)
            {
                testNumber = GetFibo(nextFibo);
                if(x == testNumber)
                    return true;
                nextFibo++;
            }

            return false;
        }
    }
}
