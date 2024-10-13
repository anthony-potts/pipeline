namespace Pipeline.Pipelines.OrderParserPipeline.Models;

public class Order
{
    public string OrderID { get; set; }
    public string Product { get; set; }
    public int Quantity { get; set; }
}