using Microsoft.AspNetCore.Components;
using ShoppingApp.Abstractions;
using ShoppingApp.WebUI.Cart;
using ShoppingApp.WebUI.Services;

namespace ShoppingApp.WebUI.Shop;

public sealed partial class ShopPage
{
    private HashSet<ProductDetails>? _products;
    private HashSet<CartItem>? _cartItems;

    [Inject]
    public ShoppingCartService ShoppingCartService { get; set; } = null!;

    [Inject]
    public InventoryService InventoryService { get; set; } = null!;

    [Inject]
    public ComponentStateChangedObserver Observer { get; set; } = null!;

    [Inject]
    public ToastService ToastService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        _products = await InventoryService.GetAllProductsAsync();
        _cartItems = await ShoppingCartService.GetAllItemsAsync();
    }

    private async Task OnAddedToCart(string productId)
    {
        var product = _products?.FirstOrDefault(p => p.Id == productId);
        if (product is null)
        {
            return;
        }

        if (await ShoppingCartService.AddOrUpdateItemAsync(1, product))
        {
            _products = await InventoryService.GetAllProductsAsync();
            _cartItems = await ShoppingCartService.GetAllItemsAsync();

            await ToastService.ShowToastAsync(
                "Added to cart",
                $"The '{product.Name}' for {product.UnitPrice:C2} was added to your cart...");
            await Observer.NotifyStateChangedAsync();

            StateHasChanged();
        }
    }

    private bool IsProductAlreadyInCart(ProductDetails product) =>
        _cartItems?.Any(c => c.Product.Id == product.Id) ?? false;
}