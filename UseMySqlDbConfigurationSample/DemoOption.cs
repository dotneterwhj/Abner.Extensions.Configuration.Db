


public class DemoOption
{
    public Position Position { get; set; }
    public string MyKey { get; set; }
    public Logging Logging { get; set; }
    public string AllowedHosts { get; set; }
}

public class Position
{
    public string Title { get; set; }
    public string Name { get; set; }
}

public class Logging
{
    public Loglevel LogLevel { get; set; }
}

public class Loglevel
{
    public string Default { get; set; }
    public string Microsoft { get; set; }
    public string MicrosoftHostingLifetime { get; set; }
}
