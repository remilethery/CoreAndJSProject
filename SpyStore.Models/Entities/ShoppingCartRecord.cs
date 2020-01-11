using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

[Table("ShoppingCartRecords", Schema = "Store")]
public class ShoppingCartRecord : ShoppingCartRecordBase
{

    [JsonIgnore]
    [ForeignKey(nameof(CustomerId))]
    public Customer CustomerNavigation { get; set; }

    [JsonIgnore]
    [ForeignKey(nameof(ProductId))]
    public Customer ProductNavigation { get; set; }

}