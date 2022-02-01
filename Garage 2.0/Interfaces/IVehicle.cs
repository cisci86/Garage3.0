namespace Garage_2._0.Interfaces
{
    public interface IVehicle
    {
        VehicleTypes Type { get; }
        //Unique
        string License { get; }
        string Color { get; }
        string Make { get; }
        string Model { get; }
        //No negative values
        int Wheels { get; }
        DateTime Arrival { get; }
    }

    public enum VehicleTypes
    {
        Car,
        Bus,
        Motorcycle
    }
}
