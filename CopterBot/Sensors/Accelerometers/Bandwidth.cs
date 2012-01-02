namespace CopterBot.Sensors.Accelerometers
{
    public enum Bandwidth : byte
    {
        Hz10 = 0,
        Hz20 = 1,
        Hz40 = 2,
        Hz75 = 3,
        Hz150 = 4,
        Hz300 = 5,
        Hz600 = 6,
        Hz1200 = 7,
        HighPass = 8,
        BandPass = 9
    }
}