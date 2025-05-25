namespace GalleryVelvet.BLL.Enums;

public static class ProductSortOrderEnumExtensions
{
    public static string ToDisplayName(this ProductSortOrder sort)
    {
        return sort switch
        {
            ProductSortOrder.None => "Без сортировки",
            ProductSortOrder.PriceLowToHigh => "Низкая цена",
            ProductSortOrder.PriceHighToLow => "Высокая цена",
            _ => "Без сортировки"
        };
    }
}