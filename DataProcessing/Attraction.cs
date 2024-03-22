using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataProcessing;

/// <summary>
/// Data transfering class for attractions.
/// </summary>
public class Attraction
{
    private readonly string name;
    private readonly string photo;
    private readonly string admArea;
    private readonly string district;
    private readonly string location;
    private readonly string registrationNumber;
    private readonly string state;
    private readonly string locationType;
    private readonly string global_id;
    private readonly string geodata_center;
    private readonly string geoarea;

    [JsonConstructor]
    public Attraction(string name, string photo, string admArea, string district, string location, string registrationNumber, string state, string locationType, string globalId, string geodataCenter, string geoArea)
    {
        this.name = name;
        this.photo = photo;
        this.admArea = admArea;
        this.district = district;
        this.location = location;
        this.registrationNumber = registrationNumber;
        this.state = state;
        this.locationType = locationType;
        this.global_id = globalId;
        this.geodata_center = geodataCenter;
        this.geoarea = geoArea;
    }

    public Attraction(List<string> initialData)
    {
        name = initialData[0];
        photo = initialData[1];
        admArea = initialData[2];
        district = initialData[3];
        location = initialData[4];
        registrationNumber = initialData[5];
        state = initialData[6];
        locationType = initialData[7];
        global_id = initialData[8];
        geodata_center = initialData[9];
        geoarea = initialData[10];
    }

    public Attraction() { }

    [JsonPropertyName(nameof(name))]
    public string Name { get { return name; } }

    [JsonPropertyName(nameof(photo))]
    public string Photo { get { return photo; } }

    [JsonPropertyName(nameof(admArea))]
    public string AdmArea { get { return admArea; } }

    [JsonPropertyName(nameof(district))]
    public string District { get { return district; } }

    [JsonPropertyName(nameof(location))]
    public string Location { get { return location; } }

    [JsonPropertyName(nameof(registrationNumber))]
    public string RegistrationNumber { get { return registrationNumber; } }

    [JsonPropertyName(nameof(state))]
    public string State { get { return state; } }

    [JsonPropertyName(nameof(locationType))]
    public string LocationType { get { return locationType; } }

    [JsonPropertyName(nameof(global_id))]
    public string GlobalId { get { return global_id; } }

    [JsonPropertyName(nameof(geodata_center))]
    public string GeodataCenter { get { return geodata_center; } }

    [JsonPropertyName(nameof(geoarea))]
    public string GeoArea { get { return geoarea; } }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public override string ToString() => $"\"{name}\";\"{photo}\";" +
            $"\"{admArea}\";\"{district}\";" +
            $"\"{location}\";\"{registrationNumber}\";\"{state}\";" +
            $"\"{locationType}\";\"{global_id}\";" +
            $"\"{geodata_center}\";\"{geoarea}\";";

}