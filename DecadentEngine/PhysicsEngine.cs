namespace DecadentEngine
{
    public class PhysicsEngine
    {
        public const double ACCEL_GRAVITY = 9.8;

        public static float ApplyGravity(int gameTime, float currentPosition)
        {
            double updateTime = gameTime;
            double timeScalar = updateTime / 75.0;
            var velocity = ACCEL_GRAVITY * timeScalar;
            var position = velocity * timeScalar;
            var newY = currentPosition + (float)position;
            return newY;
        }
    }
}
