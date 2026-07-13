using System.Collections.Generic;

public class RuntimeBlockEntry
{
    public BlockCollector Collector { get; private set; }
    public Item[] Items { get; private set; }

    public RuntimeBlockEntry(BlockCollector collector, Item[] items)
    {
        Collector = collector;
        Items = items;
    }
}
