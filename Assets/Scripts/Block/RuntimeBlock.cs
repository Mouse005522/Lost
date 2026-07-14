using System.Collections.Generic;

public class RuntimeBlock
{
    public BlockCollector Collector { get; private set; }
    public List<PortalCollector> PortalCollectors { get; private set; }
    public Item[] Items { get; private set; }

    public RuntimeBlock(BlockCollector collector, List<PortalCollector> portalCollectors, Item[] items)
    {
        Collector = collector;
        PortalCollectors = portalCollectors;
        Items = items;
    }
}
