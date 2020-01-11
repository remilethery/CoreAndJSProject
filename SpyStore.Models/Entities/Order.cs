using System;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Orders", Schema = "Store")]
public class Order : OrderBase
{
    [InverseProperty(nameof(OrderDetail.OrderNavigation))]
    public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey(nameof(CustomerId))]
    public Customer CustomerNavigation { get; set; }

}