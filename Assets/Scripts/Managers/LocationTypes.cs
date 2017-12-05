public sealed class LocationTypes {
    public enum LocationType {
        BuildingCenter, Goal, Entrance, Exit
    }

    private readonly LocationType type;
    private readonly string value;

    public static readonly LocationTypes GOAL = new LocationTypes(LocationType.Goal, "Goal");
    public static readonly LocationTypes ENTRANCE = new LocationTypes(LocationType.Goal, "Entrance");
    public static readonly LocationTypes EXIT = new LocationTypes(LocationType.Goal, "Exit");
    public static readonly LocationTypes BUILDING_CENTER = new LocationTypes(LocationType.Goal, "BuildingCenter");

    private LocationTypes(LocationType type, string value) {
        this.type = type;
        this.value = value;
    }

    public override string ToString() {
        return value;
    }
}